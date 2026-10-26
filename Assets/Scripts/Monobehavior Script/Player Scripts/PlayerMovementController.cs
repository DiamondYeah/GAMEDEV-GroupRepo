using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody rb;

    private PlayerStats defaultStats;
    private PlayerStats stats;

    // x and z movement values in Vector2 for horizontal movement and current speed
    private float xMovement;
    private float zMovement;

    // Values for sprinting
    private bool isSprinting = false;

    // y movement values for vertical movement
    private bool pressedJump = false;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Prevents rigidbody from rotating when moving
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();

        // Create copies of stats
        stats = controller.runtimeStats;
        defaultStats = Instantiate(stats);

        rb.freezeRotation = true;
    }

    // Add events to be observed when active
    private void OnEnable()
    {

    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Call functions that determine player speed, and whether they crouched or not
        AdjustPlayerSpeed();
        UpdateSprintStamina();
    }

    // Type of Update that is called at regular, fixed intervals for physics updates
    void FixedUpdate()
    {

        // Call function that determine horizontal movement
        MovePlayer();

        RaycastHit hit; // Hit detection for raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, stats.groundDetectionDistance);

        // If player is in the ground and jumped, create force that moves the player up
        if (isGrounded && pressedJump)
        {
            JumpPlayer();
        }
    }

    // Method adds force to add horizontal directions for player movement.
    private void MovePlayer()
    {
        Vector3 directionToMove = ((transform.right * xMovement) + (transform.forward * zMovement)).normalized; // Find the normalized direction of player to move
        rb.AddForce(directionToMove * (stats.currentSpeed + stats.movementSpeedMultiplier), ForceMode.VelocityChange); // Add force to the direction the player is moving 

        // Check linear velocity magnitude if it has exceeded the maxVelocity, if so, clamp it so that player doesn't continously get faster
        Vector3 rbVeloctiy = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (rbVeloctiy.magnitude >= stats.maxVelocity)
        {
            Vector3 clampedVelocty = rbVeloctiy.normalized * stats.maxVelocity;
            rb.linearVelocity = new Vector3(clampedVelocty.x, rb.linearVelocity.y, clampedVelocty.z);
        }
    }

    // Method adds force to add horizontal directions for player movement.
    private void AdjustPlayerSpeed()
    {
        // Return sprint/crouch speed multipler if player is sprinting or crouching, else reuturn base speed
        if(isSprinting && stats.sprintSpeedStamina > 0f) // Also check current stamina
        {
            stats.currentSpeed = stats.sprintSpeedMultiplier;
        }
        else
        {
            stats.currentSpeed = stats.baseSpeedMultipler;
        }
    }

    // Method updates stamina based on whether player is sprinting or not. Also updates UI
    private void UpdateSprintStamina()
    {
        if (isSprinting)
        {
            stats.sprintSpeedStamina = Mathf.Max(0f, stats.sprintSpeedStamina - stats.sprintStaminaDrain * Time.deltaTime);
        }
        else
        {
            stats.sprintSpeedStamina = Mathf.Min(defaultStats.sprintSpeedStamina, stats.sprintSpeedStamina + stats.sprintStaminaGain * Time.deltaTime);
        }

        EventController.RaiseUpdateStamina(stats.sprintSpeedStamina, defaultStats.sprintSpeedStamina);
    }

    // Method adds force that goes up to make the player jump. Also works when player is grounded
    private void JumpPlayer()
    {
        rb.AddForce(Vector3.up * stats.jumpHeight, ForceMode.VelocityChange);
        pressedJump = false;
    }

    // Called by InputAction when input is performed. Gets the values of the input for horizontal movement
    public void OnMove(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>();

        xMovement = movementVector.x;
        zMovement = movementVector.y;
    }

    // Called by InputAction when input is performed. Gets the values of the input for vertical movement
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            pressedJump = true;
        }
    }

    // Called by InputAction when input is performed. Gets the values of the input for sprinting if player is grounded
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed && isGrounded && stats.sprintSpeedStamina > 0f;
    }
}
