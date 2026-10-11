using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float sensitivity = 0.15f;

    private PlayerInput inputHandler;

    private float xRotation;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 lookInput =
            inputHandler.LookInput;

        float mouseX =
            lookInput.x * sensitivity;

        float mouseY =
            lookInput.y * sensitivity;

        xRotation -= mouseY;

        xRotation =
            Mathf.Clamp(
                xRotation,
                -80f,
                80f);

        cameraHolder.localRotation =
            Quaternion.Euler(
                xRotation,
                0f,
                0f);

        transform.Rotate(
            Vector3.up * mouseX);
    }
}