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

    [SerializeField] private bool isGrounded = false;
    private Vector3 _moveVector = new Vector3();
    private bool _isJumpRequested;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        moveAction.action.started += HandleMoveInput;
        moveAction.action.performed += HandleMoveInput;
        moveAction.action.canceled += HandleMoveInput;

        jumpAction.action.started += HandleJumpInput;
    }


    private void HandleMoveInput(InputAction.CallbackContext ctx)
    {
        string state = "";
        
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

    private void HandleJumpInput(InputAction.CallbackContext ctx)
    {
        _isJumpRequested = true;
    }

    private void Update()
    {
        //gameObject.GetComponent<Transform>().position += _moveVector * _forceValue * Time.deltaTime; 
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(ray, groundCheckDistance, groundLayer);
        //rb.AddForce(_moveVector * _forceValue * Time.fixedDeltaTime, ForceMode.Impulse);
        rb.AddForce(_moveVector * _forceValue, ForceMode.Force);

        if (_isJumpRequested)
        {
            _isJumpRequested = false;
            rb.AddForce(Vector3.up * _forceJump, ForceMode.Impulse);

            /*
            _moveVector.y += _forceJump;
            rb.AddForce(_moveVector, ForceMode.Impulse);
            _moveVector.y = 0;
            */
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
