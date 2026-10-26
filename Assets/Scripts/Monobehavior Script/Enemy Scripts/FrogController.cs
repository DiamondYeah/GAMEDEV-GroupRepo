using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FrogController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Frog Settings")]
    [SerializeField]
    private EnemyStats enemyStats;
    [SerializeField]
    private Slider healthBar;

    [HideInInspector]
    public EnemyStats stats { get; private set; }
    [HideInInspector]
    public Transform[] waypointsUntilFly { get; set; }

    // Store components
    private Rigidbody rb;
    private NavMeshAgent agent;
    private FrogMovementController movementController;

    // Store boolean for enabled or disabled movement
    private bool isMovementEnabled ;

    // Awake is called when loading an instance of a script component. Mainly used for initializing and instantiating objects
    void Awake()
    {
        stats = Instantiate(enemyStats); // Create a copy of the scriptable object to allow to be adjusted

        isMovementEnabled = true; // Set movement enabled to true
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        movementController = GetComponent<FrogMovementController>();
        agent = GetComponent<NavMeshAgent>();

        // Adjust max and current health of slider based on health in scriptable object
        healthBar.maxValue = stats.health;
        healthBar.value = stats.health;
    }

    // Method called for initializing various transform positions for moving
    public void InitializeWaypoints(Transform[] waypoints)
    {
        waypointsUntilFly = (Transform[])waypoints.Clone(); // Clone waypoint
    }

    // OnTriggerExit is called when a collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(stats.flyConsumeTag))
        {
            GetComponent<FrogMovementController>().ResetPath();
            other.GetComponentInParent<FlyController>().DestroySelf();
            Destroy(gameObject);
        }
    }

    // Method makes frog take damage when called based on parameter passed and adjust health bar aswell
    public void TakeDamage(int damage)
    {
        stats.health = Mathf.Max(0f, stats.health - damage); // Avoid health going below 0
        healthBar.value = stats.health;

        // If health is 0, reset navmesh path before destroying frog
        if (stats.health <= 0f)
        {
            DestroyFrog();
        }
    }

    // Method destroys frog after performing giving out currency and doing agent checks
    private void DestroyFrog()
    {
        // Enable agent if disabled beforehand to reset paths
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (!agent.enabled)
        {
            agent.enabled = true;
        }

        // Call currency controller and event controller to give currency gained from kill and update UI
        CurrencyController.ControllerInstance.AddHeldAmount(stats.coinsWhenKilled);
        EventController.RaiseUpdatedAmphicoinsHeld();

        // Reset path and destroy game object
        movementController.ResetPath();
        Destroy(gameObject);
    }

    // Method disables frog movement script and nav mesh agent when called
    public void StunFrog()
    {
        isMovementEnabled = !isMovementEnabled;

        agent.enabled = isMovementEnabled;
        rb.isKinematic = isMovementEnabled;
        movementController.isStunned = true;
    }

    // Method enables frog movement script and nav mesh agent when called
    public void UnstunFrog()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        isMovementEnabled = !isMovementEnabled;

        agent.enabled = isMovementEnabled;
        rb.isKinematic = isMovementEnabled;
        movementController.isStunned = false;
    }
}
