using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputActionAsset runtimeActions;
    private InputActionMap playerMap;
    private InputAction move, look, jump, sprint, leftClick, rightClick;

    public Vector2 MoveInput => move != null && move.enabled ? move.ReadValue<Vector2>() : Vector2.zero;
    public Vector2 LookInput => look != null && look.enabled ? look.ReadValue<Vector2>() : Vector2.zero;
    public bool IsSprinting => sprint != null && sprint.enabled && sprint.IsPressed();
    public bool IsJumping => jump != null && jump.enabled && jump.WasPressedThisFrame();
    public bool LeftClickPressed => leftClick != null && leftClick.enabled && leftClick.WasPressedThisFrame();
    public bool RightClickPressed => rightClick != null && rightClick.enabled && rightClick.WasPressedThisFrame();

    private void Awake()
    {
        if (inputActions == null)
        {
            Debug.LogError("Assign FriendSlopControls to PlayerInput's Input Actions field.", this);
            enabled = false;
            return;
        }

        runtimeActions = Instantiate(inputActions);
        playerMap = runtimeActions.FindActionMap("Player", true);
        move = playerMap.FindAction("Move", true);
        look = playerMap.FindAction("Look", true);
        jump = playerMap.FindAction("Jump", true);
        sprint = playerMap.FindAction("Sprint", true);
        leftClick = playerMap.FindAction("InteractLeft", true);
        rightClick = playerMap.FindAction("InteractRight", true);
    }

    private void OnEnable() => playerMap?.Enable();
    private void OnDisable() => playerMap?.Disable();
    private void OnDestroy()
    {
        if (runtimeActions != null) Destroy(runtimeActions);
    }
}
