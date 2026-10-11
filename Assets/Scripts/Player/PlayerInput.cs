using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // Made because this is called for inputs only when player makes it.
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool IsJumping { get; private set; }
    public bool LeftClickPressed { get; private set; }
    public bool RightClickPressed { get; private set; }
    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions(); // Handles all input here.

// If moving, get value. if not, zero.
        inputActions.Player.Move.performed += ctx =>
            MoveInput = ctx.ReadValue<Vector2>();

        inputActions.Player.Move.canceled += ctx =>
            MoveInput = Vector2.zero;

// Mouse Value for Looking (Up down only)
        inputActions.Player.Look.performed += ctx =>
            LookInput = ctx.ReadValue<Vector2>();

        inputActions.Player.Look.canceled += ctx =>
            LookInput = Vector2.zero;

// Jump
inputActions.Player.Jump.performed += OnJumpPerformed;


// Sprint. Toggle.
        inputActions.Player.Sprint.performed += ctx =>
            IsSprinting = true;

        inputActions.Player.Sprint.canceled += ctx =>
            IsSprinting = false;

    inputActions.Player.InteractLeft.performed += OnLeftClick;
    inputActions.Player.InteractRight.performed += OnRightClick;

    }

    // Not to multiple jumps.
private void OnJumpPerformed(InputAction.CallbackContext context)
{
    IsJumping = true;
}
public void ResetJump()
{
    IsJumping = false;
}
private void OnLeftClick(InputAction.CallbackContext context)
{
    LeftClickPressed = true;
}

private void OnRightClick(InputAction.CallbackContext context)
{
    RightClickPressed = true;
}
public void ResetLeftClick()
{
    LeftClickPressed = false;
}

public void ResetRightClick()
{
    RightClickPressed = false;
}

// turning on and off input system when player is enabled/disabled.
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}