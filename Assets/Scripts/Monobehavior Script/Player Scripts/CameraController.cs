using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Player Object")]
    [SerializeField]
    private GameObject player;

    [Header("Camera Follow Settings")]
    [SerializeField]
    private float lookSensitivty;

    private Vector2 mouseInput;
    private float pitch = 0f;
    private float yaw = 0f;

    private void Start()
    {
        // Set camera to parent transform
        transform.parent = player.transform;
      
        yaw = player.transform.eulerAngles.y;

        // Hides cursor and makes it unable to move postiion
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Type of Update that is called after Update(), used for camera movement
    private void LateUpdate()
    {
        mouseInput = Mouse.current.delta.ReadValue(); // Gets mouse's current values

        pitch -= mouseInput.y * lookSensitivty;
        yaw += mouseInput.x * lookSensitivty;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        player.transform.rotation = Quaternion.Euler(0f, yaw, 0f);

    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }
}
