using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
public class Kick : MonoBehaviour
{
    [Header("Kick Detection Settings")]
    [SerializeField]


    private LayerMask kickHitMask;
    [SerializeField] 
    private float raySize;
    [SerializeField]

    [Space(10)]
    [Header("Kick Force Settings")]
    private float kickForce;
    [SerializeField] 
    private int kickDmg;
    [SerializeField] 
    private float kickCooldown;
    [SerializeField]
    private float stunDuration;
    [SerializeField]
    private float stunnedVelocityBoundary;
    [SerializeField]
    private float durationUntilForcedWake;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;

    private bool canKick = true;
    private bool isOnPlayerView = false;

    private GameObject frog;
    [SerializeField]
    private float recoverDistance = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.SwitchPlayerMode += () => isOnPlayerView = true;
        EventController.SwitchTowerMode += () => isOnPlayerView = false;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.SwitchPlayerMode -= () => isOnPlayerView = true;
        EventController.SwitchTowerMode -= () => isOnPlayerView = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Skip if not on player view
        if (!isOnPlayerView)
        {
            return;
        }

        Camera cam = PerspectiveController.ActiveCamera;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && canKick)
        {
            StartCoroutine(StartKickCoolDown());
            source.PlayOneShot(clip);
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitinfo, raySize, kickHitMask))
            {
                frog = hitinfo.transform.gameObject;
                StartCoroutine(PerformKick());    
            }

        }

    }

    // Coroutine to add kick cooldown to avoid kicking continously
    private IEnumerator StartKickCoolDown()
    {
        canKick = false;
        Debug.Log("On CD!"); 
        yield return new WaitForSeconds(kickCooldown);
        canKick = true;
        yield return null;

    }

    // Coroutine performs kick by disabling agent and adding force to frog. Agent is enabled again once frog stops moving from the force
    private IEnumerator PerformKick() {
        NavMeshAgent agent = frog.GetComponent<NavMeshAgent>();
        FrogController frogControllerScript = frog.GetComponent<FrogController>();
        FrogMovementController frogMoveScript = frog.GetComponent<FrogMovementController>();
        Rigidbody rb = frog.GetComponent<Rigidbody>();

        // Call StunFrog to disable agent and kinematic, and make frog stunned
        frogControllerScript.StunFrog();

        // Add force and make frog take damage from kick
        rb.AddForce(transform.forward * kickForce, ForceMode.Impulse);
        frogControllerScript.TakeDamage(kickDmg);

        yield return new WaitForSeconds(0.2f); // Add small delay to accumelate linear velocity for checking later

        // Keep stunned while velocity is above stunnedVelocityBoundary (Frog is still moving alot) or not until force wakeup
        float duration = durationUntilForcedWake;
        while (rb.linearVelocity.sqrMagnitude > stunnedVelocityBoundary && duration > 0f)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(stunDuration); // Delay agent based on stun duration

        // Call UnstunFrog to enable agent and kinematic, and make frog unstunned
        frogControllerScript.UnstunFrog();

        // Instakill frog if not on navmesh
        if (!agent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(frog.transform.position, out NavMeshHit navHit, recoverDistance, NavMesh.AllAreas))
            {
                agent.Warp(navHit.position);
            }
            else
            {
                Debug.Log("not navmesh");
                frogControllerScript.TakeDamage(9999);
            }
        }

    }
}
