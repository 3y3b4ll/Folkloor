using UnityEngine;

[RequireComponent(typeof(Transform))]
public class FollowMouseOnGround : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 8f;         // units/sec

    [Header("Ground projection")]
    public bool usePhysicsRaycast = true;    // use raycast to hit actual colliders (terrain/mesh)
    public LayerMask groundLayer = ~0;       // choose ground layer(s); default = everything
    public float groundY = 0f;               // used only if usePhysicsRaycast == false

    [Header("CharacterController support")]
    public CharacterController controller;   // optional - if present script uses controller.Move(delta)

    [Header("Debug")]
    public bool debugLogs = false;
    public bool drawGizmos = true;
    public Camera worldCamera;               // optional; if null Camera.main will be used

    Camera cam;

    void Start()
    {
        cam = worldCamera != null ? worldCamera : Camera.main;
        if (cam == null)
            Debug.LogError("[FollowMouseOnGround] No camera assigned and Camera.main is null. Tag a camera \"MainCamera\" or assign one here.");

        if (controller == null)
            controller = GetComponent<CharacterController>();

        // If an Animator is on the root, ensure it isn't applying root motion unless intentional:
        Animator anim = GetComponent<Animator>();
        if (anim != null && anim.applyRootMotion)
        {
            if (debugLogs) Debug.Log("[FollowMouseOnGround] Animator has Apply Root Motion ON — this may override transform movement. Consider turning it off.");
        }
    }

    void Update()
    {
        if (cam == null) return;

        // Only move when left mouse button is held down
        if (!Input.GetMouseButton(0)) return;

        Vector3? hitPoint = GetMouseWorldPosition();
        if (!hitPoint.HasValue) return;

        // We only want to change XZ; preserve current Y
        Vector3 target = new Vector3(hitPoint.Value.x, transform.position.y, hitPoint.Value.z);

        // Smooth move
        Vector3 nextPos = Vector3.MoveTowards(transform.position, target, maxSpeed * Time.deltaTime);
        Vector3 delta = nextPos - transform.position;

        if (controller != null)
            controller.Move(delta);
        else
            transform.position = nextPos;
    }


    // Returns world point on ground (y = groundY) or collider hit point (if using raycast)
    private Vector3? GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (usePhysicsRaycast)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
            {
                if (drawGizmos) Debug.DrawLine(ray.origin, hit.point, Color.green, 0.1f);
                return hit.point;
            }
            else
            {
                if (debugLogs) Debug.Log("[FollowMouseOnGround] Raycast hit nothing on groundLayer. Check layer mask and that terrain has a collider.");
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
