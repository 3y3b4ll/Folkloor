using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerAnimator_CC : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;

    private Vector3 lastPosition;
    private float verticalVelocity;

    int hashSpeed, hashIsJumping;

    void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (controller == null) controller = GetComponent<CharacterController>();

        lastPosition = transform.position;

        hashSpeed = Animator.StringToHash("Speed");
        hashIsJumping = Animator.StringToHash("IsJumping");
    }

    void Update()
    {
        // Horizontal speed
        Vector3 delta = transform.position - lastPosition;
        Vector3 horizontal = new Vector3(delta.x, 0, delta.z);
        float speed = horizontal.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        animator.SetFloat(hashSpeed, speed);
        lastPosition = transform.position;

        // Jump/Fall detection
        bool inAir = !controller.isGrounded;
        animator.SetBool(hashIsJumping, inAir);
    }

    public void SetVerticalVelocity(float v)
    {
        verticalVelocity = v; // optional, can still use if you need upward movement logic
    }
}