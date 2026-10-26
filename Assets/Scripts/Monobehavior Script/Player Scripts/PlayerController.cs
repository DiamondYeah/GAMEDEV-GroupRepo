using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private PlayerStats playerStats;

    // Store components
    private Rigidbody rb;

    // Store boolean for enabled or disabled movement
    private bool isMovementEnabled;

    // Copy of player stats for adjusting values at runtime
    [HideInInspector]
    public PlayerStats runtimeStats { get; private set; }

    // Get Movement Script to only be enabled when in player view
    private PlayerMovementController movementScript;

    // Awake is called when loading an instance of a script component. Mainly used for initializing and instantiating objects
    void Awake()
    {
        // Create a copy of player stats that will be edited on runtime and be referenced against
        runtimeStats = Instantiate(playerStats); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<PlayerMovementController>();

        // Disable movement script at start
        isMovementEnabled = false;
        movementScript.enabled = isMovementEnabled;
    }

    // Add events to be observed when active
    private void OnEnable()
    {

        EventController.SwitchPlayerMode += ToggleMovement;
        EventController.SwitchTowerMode += ToggleMovement;
        EventController.DamagePlayer += DamagePlayer;

    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.SwitchPlayerMode -= ToggleMovement;
        EventController.SwitchTowerMode -= ToggleMovement;
        EventController.DamagePlayer += DamagePlayer;
    }

    // Method enables and disables movement script when called
    private void ToggleMovement()
    {
        isMovementEnabled = !isMovementEnabled;
        movementScript.enabled = isMovementEnabled;
    }

    // Method reduces health of player depending on damage passed
    private void DamagePlayer(float damage)
    {
        runtimeStats.health = Mathf.Max(0f, runtimeStats.health - damage); // Avoid health going below 0
        EventController.RaiseUpdateHealth(runtimeStats.health, playerStats.health);

        if (runtimeStats.health <= 0f)
        {
            EventController.RaiseLostAllHealth();
        }
    }

}
