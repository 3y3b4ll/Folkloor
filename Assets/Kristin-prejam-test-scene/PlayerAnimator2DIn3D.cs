using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator2DIn3D : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;  // assuming you're moving with 3D physics
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;

    // Caches for parameter hashes (optional but slightly more efficient)
    int hashSpeed;
    int hashIsJumping;
    int hashIsFalling;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();

        // Precompute hashes
        hashSpeed = Animator.StringToHash("Speed");
        hashIsJumping = Animator.StringToHash("IsJumping");
        hashIsFalling = Animator.StringToHash("IsFalling");
    }

    void Update()
    {
        // 1. Ground check via raycast
        CheckGrounded();

        // 2. Set Speed parameter based on horizontal movement (X, Z)
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = horizontalVel.magnitude;
        animator.SetFloat(hashSpeed, speed);

        // 3. Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Trigger the jump
            animator.SetBool(hashIsJumping, true);
            isGrounded = false;  // temporarily mark not grounded
        }

        // 4. Falling detection
        // If velocity going down and not grounded, mark falling
        if (rb.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool(hashIsFalling, true);
        }
        else
        {
            animator.SetBool(hashIsFalling, false);
        }
    }

    void CheckGrounded()
    {
        // Cast a small ray downward from the player�s feet
        Vector3 origin = transform.position;
        Ray ray = new Ray(origin, Vector3.down);
        float rayDist = groundCheckDistance + 0.1f;

        if (Physics.Raycast(ray, out RaycastHit hit, rayDist, groundLayer))
        {
            // If we're very near the ground, mark grounded
            isGrounded = true;
            animator.SetBool(hashIsJumping, false);
            animator.SetBool(hashIsFalling, false);
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the ground check ray
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (groundCheckDistance + 0.1f));
    }
}
