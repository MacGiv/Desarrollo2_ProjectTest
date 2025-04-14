using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private float _speed;
    [SerializeField] private float _force;

    private void OnEnable()
    {
        if (_moveAction == null)
        {
            return;
        }
        _moveAction.action.performed += OnMove;
    }

    private void OnDisable()
    {
        _moveAction.action.performed -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        //var request = new ForceRequest();
        //request.mode = ForceMode.Force;
        //var horizontalInput = obj.ReadValue<Vector2>();
        //request.direction = obj.ReadValue<Vector2>(); new Vector3(horizontalInput.x, 0, horizontalInput.y);
        //request.speed = speed;
        //request.force = force;
        //character.RequestForce(request);
    }

}
