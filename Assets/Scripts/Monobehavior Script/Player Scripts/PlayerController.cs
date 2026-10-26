using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private PlayerStats playerStats;

    [Header("Player Scripts")]
    [SerializeField]
    private MonoBehaviour[] playerScripts;

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

        // Disable scripts at start
        DisableActions();
    }

    // Add events to be observed when active
    private void OnEnable()
    {

        EventController.SwitchPlayerMode += EnableActions;
        EventController.SwitchTowerMode += DisableActions;
        EventController.DamagePlayer += DamagePlayer;

    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.SwitchPlayerMode -= EnableActions;
        EventController.SwitchTowerMode -= DisableActions;
        EventController.DamagePlayer += DamagePlayer;
    }

    // Method enables player script
    public void EnableActions()
    {
        foreach(MonoBehaviour script in playerScripts)
        {
            script.enabled = true;
        }
    }

    // Method disables player script
    public void DisableActions()
    {
        foreach (MonoBehaviour script in playerScripts)
        {
            script.enabled = false;
        }
    }

    // Method reduces health of player depending on damage passed
    public void DamagePlayer(float damage)
    {
        runtimeStats.health = Mathf.Max(0f, runtimeStats.health - damage); // Avoid health going below 0
        EventController.RaiseUpdateHealth(runtimeStats.health, playerStats.health);

        if (runtimeStats.health <= 0f)
        {
            EventController.RaiseLostAllHealth("Player lost all health!");
            EventController.RaiseSwitchTowerMode();
            Destroy(gameObject);
        }
    }

}
