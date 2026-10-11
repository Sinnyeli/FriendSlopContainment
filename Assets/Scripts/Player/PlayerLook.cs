using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float sensitivity = 0.15f;
    private PlayerInput inputHandler;
    private float xRotation;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInput>();
        if (cameraHolder == null)
        {
            Camera camera = GetComponentInChildren<Camera>();
            if (camera != null) cameraHolder = camera.transform;
        }
        if (cameraHolder == null || cameraHolder == transform)
        {
            Debug.LogError("Assign a child camera or camera pivot to PlayerLook.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        Vector2 look = inputHandler.LookInput * sensitivity;
        xRotation = Mathf.Clamp(xRotation - look.y, -80f, 80f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * look.x);
    }
}
