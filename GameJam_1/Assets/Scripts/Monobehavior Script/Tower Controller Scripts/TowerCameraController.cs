using UnityEngine;
using UnityEngine.InputSystem;

public class TowerCameraController : MonoBehaviour
{
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

    private Vector3 startPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }


    // Method adds force to add horizontal directions for player movement.
    void MoveCamera()
    {
        float rightDirectionToMove = (Vector3.right * xMovement).normalized.x; // Find the normalized direction of player to move for left and right
        float forwardDirectionToMove = (Vector3.forward * zMovement).normalized.z; // Find the normalized direction of player to move for forward and backwards
        transform.position += new Vector3(rightDirectionToMove * sidewaysCameraSpeed * Time.deltaTime, 0f, forwardDirectionToMove * forwardCameraSpeed * Time.deltaTime);


        Vector3 camPos = transform.position;
        camPos.x = Mathf.Clamp(camPos.x, -(startPos.x + sidewaysDistanceClamp), (startPos.x + sidewaysDistanceClamp));
        camPos.z = Mathf.Clamp(camPos.z, -(startPos.z + forwardDistanceClamp), (startPos.z + forwardDistanceClamp));
        transform.position = camPos;

    }

    // Called by InputAction when input is performed. Gets the values of the input for horizontal movement
    public void OnMove(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>();

        xMovement = movementVector.x;
        zMovement = movementVector.y;
    }
}
