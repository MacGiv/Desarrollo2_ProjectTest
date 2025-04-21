using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int _forceValue = 10;
    [SerializeField] private int _forceJump = 10;
    [SerializeField] private float groundCheckDistance = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    //[SerializeField] private float jumpHoldForce = 2f;
    [SerializeField] private float jumpHoldDuration = 0.2f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private Vector3 _moveVector = new Vector3();
    [SerializeField] private bool _isJumpRequested;
    [SerializeField] private int jumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private bool isJumpHeld = false;
    [SerializeField] private float jumpHoldTimer = 0f;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        moveAction.action.started += HandleMoveInput;
        moveAction.action.performed += HandleMoveInput;
        moveAction.action.canceled += HandleMoveInput;

        jumpAction.action.started += StartJump;
        jumpAction.action.canceled += StopJump;
    }

    private void Update()
    {
        //gameObject.GetComponent<Transform>().position += _moveVector * _forceValue * Time.deltaTime; 
    }

    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {        
        _moveVector.x = ctx.ReadValue<Vector2>().x;
        _moveVector.z = ctx.ReadValue<Vector2>().y;

        // DEBUG
        // Debug.Log(state + ctx.ReadValue<Vector2>());
        // if (ctx.started)
        // {
        //     state = "started: ";
        // }
        // else if (ctx.performed)
        // {
        //     state = "performed: ";
        // 
        // }
        // else if (ctx.canceled)
        // {
        //     state = "canceled: ";
        // }
        // 
        //END DEBUG

    }


    private void FixedUpdate()
    {
        // Ground check
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(ray, groundCheckDistance, groundLayer);
        if (isGrounded && rb.linearVelocity.y <= -0.1f)
        {
            jumpCount = 0;
        }
        // Movement
        rb.AddForce(_moveVector * _forceValue, ForceMode.Force);
        // Normal Jump
        if (_isJumpRequested)
        {
            _isJumpRequested = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * _forceJump, ForceMode.Impulse);
        }
        // Holded jump
        if (isJumpHeld && (jumpHoldTimer < jumpHoldDuration))
        {
            rb.AddForce(Vector3.up * _forceJump, ForceMode.Force);
            jumpHoldTimer += Time.fixedDeltaTime;
        }
    }

    private void StartJump(InputAction.CallbackContext ctx)
    {
        if (jumpCount < maxJumpCount)
        {
            _isJumpRequested = true;
            isJumpHeld = true;
            jumpHoldTimer = 0f;
            jumpCount++;
        }
    }

    private void StopJump(InputAction.CallbackContext ctx)
    {
        isJumpHeld = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
