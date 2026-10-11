using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    private CharacterController controller;
    private PlayerInput inputHandler;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        bool grounded = controller.isGrounded;
        if (grounded && verticalVelocity < 0f) verticalVelocity = -2f;
        if (grounded && inputHandler.IsJumping)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        Vector2 input = Vector2.ClampMagnitude(inputHandler.MoveInput, 1f);
        Vector3 movement = transform.right * input.x + transform.forward * input.y;
        movement *= inputHandler.IsSprinting ? sprintSpeed : moveSpeed;
        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;
        controller.Move(movement * Time.deltaTime);
    }
}
