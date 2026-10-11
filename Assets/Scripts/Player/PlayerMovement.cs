using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private PlayerInput inputHandler;

    private bool isGrounded;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputHandler = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;

        Move();
        HandleJump();
        ApplyGravity();
    }

    private void Move()
    {
        Vector2 moveInput = inputHandler.MoveInput;

        Vector3 moveDirection =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        float currentSpeed = moveSpeed;

        if (inputHandler.IsSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        controller.Move(
            moveDirection *
            currentSpeed *
            Time.deltaTime);
    }
    private void HandleJump()
    {
        if (inputHandler.IsJumping && isGrounded)
        {
            
            verticalVelocity =
                Mathf.Sqrt(jumpHeight * -2f * gravity);

            inputHandler.ResetJump();
        }
    }
    private void ApplyGravity()
    {
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 gravityMove =
            Vector3.up *
            verticalVelocity *
            Time.deltaTime;

        controller.Move(gravityMove);
    }
}