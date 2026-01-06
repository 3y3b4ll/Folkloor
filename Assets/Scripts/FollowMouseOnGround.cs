using UnityEngine;

[RequireComponent(typeof(Transform))]
public class FollowMouseOnGround : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;

    [Header("Ground projection")]
    public bool usePhysicsRaycast = true;
    public LayerMask groundLayer;          // set this to MouseGround
    public float groundY = 0f;              // fallback only

    [Header("CharacterController support")]
    public CharacterController controller;

    [Header("Click Indicator")]
    public GameObject clickIndicator;      // 2D sprite, world space

    [Header("Click vs Hold")]
    public float holdThreshold = 0.15f;     // seconds to count as hold

    [Header("Debug")]
    public bool debugLogs = false;
    public bool drawGizmos = true;
    public Camera worldCamera;

    Camera cam;

    Vector3? currentTarget;

    // click / hold detection
    Vector3? mouseDownPoint;
    float holdTime = 0f;
    bool wasHolding = false;

    void Start()
    {
        cam = worldCamera != null ? worldCamera : Camera.main;

        if (cam == null)
            Debug.LogError("[FollowMouseOnGround] No camera assigned and Camera.main is null.");

        if (controller == null)
            controller = GetComponent<CharacterController>();

        if (clickIndicator != null)
            clickIndicator.SetActive(false);
    }

    void Update()
    {
        if (cam == null) return;

        // -------------------
        // INPUT
        // -------------------

        // mouse down: maybe a click
        if (Input.GetMouseButtonDown(0))
        {
            holdTime = 0f;
            wasHolding = false;
            mouseDownPoint = GetMouseWorldPosition();
        }

        // mouse held: steering
        if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;

            if (holdTime > holdThreshold)
                wasHolding = true;

            if (wasHolding)
            {
                Vector3? hitPoint = GetMouseWorldPosition();
                if (hitPoint.HasValue)
                    currentTarget = hitPoint.Value;
            }
        }

        // mouse released: decide
        if (Input.GetMouseButtonUp(0))
        {
            if (wasHolding)
            {
                // release after hold -> stop
                currentTarget = null;

                if (clickIndicator != null)
                    clickIndicator.SetActive(false);
            }
            else
            {
                // real click -> command
                if (mouseDownPoint.HasValue)
                {
                    currentTarget = mouseDownPoint.Value;

                    if (clickIndicator != null)
                    {
                        Vector3 p = currentTarget.Value;
                        p.y += 0.05f; // small lift above terrain
                        clickIndicator.transform.position = p;
                        clickIndicator.SetActive(true);
                    }
                }
            }

            wasHolding = false;
            holdTime = 0f;
            mouseDownPoint = null;
        }

        // -------------------
        // MOVEMENT
        // -------------------

        if (!currentTarget.HasValue) return;

        Vector3 target = new Vector3(
            currentTarget.Value.x,
            transform.position.y,
            currentTarget.Value.z
        );

        Vector3 nextPos = Vector3.MoveTowards(transform.position, target, maxSpeed * Time.deltaTime);
        Vector3 delta = nextPos - transform.position;

        if (controller != null)
            controller.Move(delta);
        else
            transform.position = nextPos;

        // -------------------
        // ARRIVAL CHECK (XZ only)
        // -------------------

        if (clickIndicator != null && currentTarget.HasValue)
        {
            Vector2 a = new Vector2(transform.position.x, transform.position.z);
            Vector2 b = new Vector2(currentTarget.Value.x, currentTarget.Value.z);

            if (Vector2.Distance(a, b) < 0.1f)
                clickIndicator.SetActive(false);
        }
    }

    // -------------------
    // RAYCAST
    // -------------------

    private Vector3? GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (usePhysicsRaycast)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                if (drawGizmos) Debug.DrawLine(ray.origin, hit.point, Color.green, 0.1f);
                return hit.point;
            }
            else
            {
                if (debugLogs) Debug.Log("[FollowMouseOnGround] Raycast hit nothing on groundLayer.");
                return null;
            }
        }
        else
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 p = ray.GetPoint(enter);
                if (drawGizmos) Debug.DrawLine(ray.origin, p, Color.cyan, 0.1f);
                return p;
            }
            return null;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
