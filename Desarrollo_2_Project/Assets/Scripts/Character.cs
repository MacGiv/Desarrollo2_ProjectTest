using UnityEngine;

/// <summary>
/// Controla el movimiento y el salto del personaje.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float deceleration = 25f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpHoldForce = 4f;
    [SerializeField] private float jumpHoldTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private int maxJumps = 2;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector3 currentVelocity = Vector3.zero;

    [SerializeField] private bool isGrounded;
    private bool jumpRequested;
    private bool jumpHeld;
    private int jumpCount;
    private float coyoteTimer;
    private float jumpHoldTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetMovementInput(Vector2 input)
    {
        movementInput = input;
    }

    public void RequestJump()
    {
        jumpRequested = true;
    }

    public void HoldJump(bool isHeld)
    {
        jumpHeld = isHeld;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        JumpLogic();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            jumpCount = 0;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
    }

    private void Move()
    {
        // Direction from camera
        Vector3 inputDir = new Vector3(movementInput.x, 0, movementInput.y);
        if (inputDir.magnitude > 1f)
            inputDir.Normalize();

        // Movement direction depending on camera direction
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Get movement + camera
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;

        // Movement
        if (moveDir.magnitude > 0.1f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, moveDir * moveSpeed, acceleration * Time.fixedDeltaTime);

            // Rotate to object's direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Deacelerate¿?
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        Vector3 velocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
        rb.linearVelocity = velocity;
    }

    private void JumpLogic()
    {
        // Jump
        if (jumpRequested && (coyoteTimer > 0f || jumpCount < maxJumps))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
            jumpRequested = false;
            jumpHoldTimer = 0f;
        }

        // Hold Jump like Mario
        if (jumpHeld && jumpHoldTimer < jumpHoldTime)
        {
            rb.AddForce(Vector3.up * jumpHoldForce, ForceMode.Force);
            jumpHoldTimer += Time.fixedDeltaTime;
        }

        if (!jumpHeld)
        {
            jumpHoldTimer = jumpHoldTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
