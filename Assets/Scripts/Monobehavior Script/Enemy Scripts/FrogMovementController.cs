using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FrogMovementController : MonoBehaviour
{
    private NavMeshAgent agent;
    private FrogController controller;
    private EnemyStats stats;

    private NavMeshPath path;
    private GameObject selectedFlyArea = null;
    private float repathCheck = 0f;
    private bool isJumping;

    private Transform[] waypoints;
    private int destinationCount = 0;

    private bool isNewWaypoint = true;

    [HideInInspector]
    public bool isStunned { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get components for needed variables
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<FrogController>();
        stats = controller.stats;

        // Adjust navmesh agent settings
        agent.speed = stats.speed;
        agent.stoppingDistance = 0f;
        agent.autoBraking = false; // Prevent slowing down when almost reaching destination

        path = new NavMeshPath();
        agent.autoTraverseOffMeshLink = false;

        // Set waypoints
        waypoints = controller.waypointsUntilFly;

        // Set stun condition to false
        isStunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping || isStunned)
        {
            return;
        }
        if (!agent.enabled)
        {
            return;
        }
          
        if (agent.isOnOffMeshLink)
        {
            StartCoroutine(JumpAcrossLog());
            return;
        }

        Vector3 target = FindNextTarget();

        if ((Time.time >= repathCheck && !agent.pathPending) || isNewWaypoint)
        {
            repathCheck = Time.time + stats.nextPathCheck;
            agent.SetDestination(target);

            if(isNewWaypoint)
            {
                isNewWaypoint = false;
            }
        }

    }

    // Method determines next target to set destination to by looking at current destination count and waypoints left
    private Vector3 FindNextTarget()
    {
        if(waypoints != null && destinationCount < waypoints.Length)
        {
            Vector3 destination = waypoints[destinationCount].position; 

            // If frog has reached destination in z position (forward), increment count and find next target
            if (Mathf.Abs(destination.z - transform.position.z) < stats.arriveDistance)
            {
                destinationCount++;
                isNewWaypoint = true;
            }

            if(destinationCount < waypoints.Length)
            {
                return new Vector3(transform.position.x, destination.y, destination.z); // Go to new destination while making sure frog goes straight
            }
            
        }


        // Find fly location once all waypoints have been reached, and check if frog has a selected fly area
        FindFly();
        if(selectedFlyArea == null)
        {
            return transform.position; // Stay in place if no more flies left to pick

        }
        return selectedFlyArea.transform.position; // If no waypoints or all waypoints have been reached, travel to selected fly location

    }

    private void FindFly()
    {
        GameObject[] flyAreas = GameObject.FindGameObjectsWithTag(stats.flyConsumeTag);
        GameObject closestFlyArea = null;
        FlyController closestFlyController = null;

        float shortestPath = float.MaxValue;

        foreach (GameObject area in flyAreas)
        {
            FlyController flyControl = area.GetComponentInParent<FlyController>();

            Debug.Log(area.GetComponentInParent<FlyController>());

            // Ignore fly if already selected
            if (flyControl.isSelected)
            {
                continue;
            }

            bool ok = agent.CalculatePath(flyControl.flyEatArea.position, path);
            Debug.Log($"{flyControl.name} at {flyControl.flyEatArea.position}: ok={ok}, status={path.status}");

            // Calculate if path is possible and complete, if not skip to next flies
            if (!agent.CalculatePath(flyControl.flyEatArea.transform.position, path) || path.status != NavMeshPathStatus.PathComplete)
            {
                continue;
            }

            // Find path length and check if calculated distance is shorter than the current one stored, if so update shortestPath
            float flyDistance = FindPathLength();
            if (shortestPath > flyDistance)
            {
                shortestPath = flyDistance;
                closestFlyArea = area;
                closestFlyController = flyControl;
            }
        }

        if (!selectedFlyArea && closestFlyController)
        {
            closestFlyController.isSelected = true;
            selectedFlyArea = closestFlyArea;
        }

    }

    // Method gets all corners of path to calculate total distance of NavMeshPath via Vector3.Distance
    private float FindPathLength()
    {
        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return distance;
    }

    // Method resets nav mesh agent paths
    public void ResetPath()
    {
        // Reset path only if on NavMesh
        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }


        // If fly was selected before resetting, deselect fly
        if(selectedFlyArea != null)
        {
            selectedFlyArea.GetComponentInParent<FlyController>().isSelected = false;
            selectedFlyArea = null;
        }
    }

    // Coroutine that performs jump traversal against a navmesh link and waits based on jumpDelay before jumping again
    private IEnumerator JumpAcrossLog()
    {
        OffMeshLinkData link = agent.currentOffMeshLinkData;

        isJumping = true;
        agent.autoTraverseOffMeshLink = true;
        agent.SetDestination(link.endPos);
        yield return new WaitForSeconds(stats.jumpDelay);          
        agent.autoTraverseOffMeshLink = false;
        isJumping = false;
    }
}