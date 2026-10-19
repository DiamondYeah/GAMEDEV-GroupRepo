using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private PlayerStats playerStats;

    private Rigidbody rb;

    // Copy of player stats for adjusting values at runtime 
    public PlayerStats runtimeStats { get; set; }

    // x and z movement values in Vector2 for horizontal movement
    private float xMovement;
    private float zMovement;

    // y movement values for vertical movement
    private Boolean pressedJump = false;
    private Boolean isGrounded;

    // Awake is called when loading an instance of a script component. Mainly used for initializing and instantiating objects
    void Awake()
    {
        runtimeStats = Instantiate(playerStats); // Create a copy of playerStats that will be edited on runtime
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Prevents rigidbody from rotating when moving
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.DamagePlayer += DamagePlayer;
        EventController.ResetGame += ResetPlayer;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.DamagePlayer -= DamagePlayer;
        EventController.ResetGame -= ResetPlayer;
    }


    // Type of Update that is called at regular, fixed intervals for physics updates
    void FixedUpdate()
    {
        MovePlayer();

        RaycastHit hit; // Hit detection for raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, runtimeStats.groundDetectionDistance);

        // If player is in the ground and jumped, create force that moves the player up
        if (isGrounded && pressedJump)
        {
            JumpPlayer();
        }
    }

    // Method adds force to add horizontal directions for player movement.
    void MovePlayer()
    {
        Vector3 directionToMove = ((transform.right * xMovement) + (transform.forward * zMovement)).normalized; // Find the normalized direction of player to move
        rb.AddForce(directionToMove * runtimeStats.movementSpeedMultiplier, ForceMode.VelocityChange); // Add force to the direction the player is moving 


        // Check linear velocity magnitude if it has exceeded the maxVelocity, if so, clamp it so that player doesn't continously get faster
        Vector3 rbVeloctiy = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (rbVeloctiy.magnitude >= runtimeStats.maxVelocity)
        {
            Vector3 clampedVelocty = rbVeloctiy.normalized * runtimeStats.maxVelocity;
            rb.linearVelocity = new Vector3(clampedVelocty.x, rb.linearVelocity.y, clampedVelocty.z);
        }
    }

    // Method adds force that goes up to make the player jump. Also works when player is grounded
    private void JumpPlayer()
    {
        rb.AddForce(Vector3.up * runtimeStats.jumpHeight, ForceMode.VelocityChange);
        pressedJump = false;
    }

    // Method reduces health of player depending on damage passed
    private void DamagePlayer(float damage)
    {
        runtimeStats.health -= runtimeStats.health > 0f ? Mathf.Clamp(damage, 0f, damage) : 0f;
    }

    // Method resets stats of player
    private void ResetPlayer()
    {
        runtimeStats = Instantiate(playerStats);
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
        pressedJump = true;
    }


}
