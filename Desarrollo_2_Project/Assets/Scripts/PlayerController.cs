using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        character.SetMovementInput(input);

        if (jumpAction.action.triggered)
        {
            character.RequestJump();
        }

        if (jumpAction.action.IsPressed())
        {
            character.HoldJump(true);
        }
        else
        {
            character.HoldJump(false);
        }
    }
}
