using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private Transform target; // PlayerCameraRoot
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float verticalClamp = 70f;

    private float yaw;
    private float pitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Mouse.current.delta.ReadValue().x * sensitivity;
        float mouseY = Mouse.current.delta.ReadValue().y * sensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -verticalClamp, verticalClamp);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        if (target != null)
            target.rotation = Quaternion.Euler(0, yaw, 0);
    }
}
