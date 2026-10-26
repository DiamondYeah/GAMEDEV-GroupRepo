using UnityEngine;
using UnityEngine.InputSystem;

public class TowerCameraController : MonoBehaviour
{
    [Header("Camera Input Settings")]
    [SerializeField]
    private InputActionReference moveAction;   // drag Tower/Move here

    [Space(10)]
    [Header("Camera Movement Settings")]
    [SerializeField]
    private float forwardCameraSpeed = 0f;
    [SerializeField]
    private float sidewaysCameraSpeed = 0f;
    [SerializeField]
    private float forwardDistanceClamp = 10f;
    [SerializeField]
    private float sidewaysDistanceClamp = -10f;

    private Camera cam;
    // x and z movement values in Vector2 for horizontal movement
    private float xMovement;
    private float zMovement;

    // Distance clamps based on clamp values and Cam's position
    private float totalForwardDistance;
    private float totalSidewaysDistance;

    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        startPos = transform.position;

        // Get total distance for forward and sideways
        totalForwardDistance = forwardDistanceClamp + Mathf.Abs(transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = moveAction.action.ReadValue<Vector2>();

        xMovement = movementVector.x;
        zMovement = movementVector.y;

        MoveCamera();
    }


    // Method adds force to add horizontal directions for player movement.
    void MoveCamera()
    {
        float rightDirectionToMove = (Vector3.right * xMovement).normalized.x; // Find the normalized direction of player to move for left and right
        float forwardDirectionToMove = (Vector3.forward * zMovement).normalized.z; // Find the normalized direction of player to move for forward and backwards
        transform.position += new Vector3(rightDirectionToMove * sidewaysCameraSpeed * Time.deltaTime, 0f, forwardDirectionToMove * forwardCameraSpeed * Time.deltaTime);

        Vector3 camPos = transform.position;
        camPos.x = Mathf.Clamp(camPos.x, startPos.x - sidewaysDistanceClamp, startPos.x + sidewaysDistanceClamp);
        camPos.z = Mathf.Clamp(camPos.z, startPos.z - forwardDistanceClamp, startPos.z + forwardDistanceClamp);
        transform.position = camPos;
    }
}
