using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerAnimator_CC : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;

    private Vector3 lastPosition;
    private float verticalVelocity;

    int hashSpeed, hashIsJumping, hashIsFalling;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (controller == null) controller = GetComponent<CharacterController>();

        lastPosition = transform.position;

        hashSpeed = Animator.StringToHash("Speed");
        hashIsJumping = Animator.StringToHash("IsJumping");
        hashIsFalling = Animator.StringToHash("IsFalling");
    }

    void Update()
    {
        // Horizontal speed
        Vector3 delta = transform.position - lastPosition;
        Vector3 horizontal = new Vector3(delta.x, 0, delta.z);
        float speed = horizontal.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        animator.SetFloat(hashSpeed, speed);
        lastPosition = transform.position;

        // Jumping
        if (controller.isGrounded && animator.GetBool(hashIsJumping))
        {
            animator.SetBool(hashIsJumping, false); // landed
        }

        // Falling
        bool isFalling = !controller.isGrounded && verticalVelocity < -0.1f;
        animator.SetBool(hashIsFalling, isFalling);
    }

    // Called from movement script
    public void SetVerticalVelocity(float v)
    {
        verticalVelocity = v;

        if (controller.isGrounded && v > 0.1f) // jumping upwards
        {
            animator.SetBool(hashIsJumping, true);
        }
    }
}