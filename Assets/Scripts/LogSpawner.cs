using UnityEngine;

public class LogSpawner : MonoBehaviour
{
    [Header("Object to spawn")]
    [SerializeField] GameObject[] logPrefab;

    [Header("Lane")]
    [SerializeField] Transform endPoint;    // The end point of the lane where logs will be destroyed
    [SerializeField] float laneSpeed = 3f;  // Every log in this lane moves at this speed

    [Header("Spacing / Spawn Interval Settings")]
    [SerializeField] float minSpawnInterval = 2f;
    [SerializeField] float maxSpawnInterval = 5f;

    [Header("Start of Level")]
    [SerializeField] bool prewarm = true; // Fills the lane with logs at the start of the level

    Vector3 direction;
    float timer, nextSpawnTime, laneLength;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Check if the end point is assigned
        if (endPoint == null)
        {
            UnityEngine.Debug.LogWarning("LogSpawner: No end point assigned. Logs will not move.", this);
            enabled = false;
            return;
        }

        // Check if the log prefabs are assigned
        if (logPrefab == null || logPrefab.Length == 0)
        {
            UnityEngine.Debug.LogWarning("LogSpawner: No log prefabs assigned.", this);
            enabled = false;
            return;
        }

        // Calculate the direction from the spawner to the end point
        Vector3 toEnd = endPoint.position - transform.position;
        toEnd.y = 0f;       // Ignore vertical differences
        direction = toEnd.normalized;

        // Calculate the length of the lane
        laneLength = toEnd.magnitude;

        if (prewarm) PreWarmLane();
        ScheduleNextSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            SpawnLog(0f);
            timer = 0f;
            ScheduleNextSpawn();
        }
    }

    private void ScheduleNextSpawn()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void PreWarmLane()
    {
        float offset = laneSpeed * Random.Range(minSpawnInterval, maxSpawnInterval);

        while (offset < laneLength)
        {
            SpawnLog(offset);
            offset += laneSpeed * Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnLog(float offsetAlongLane)
    {
        GameObject prefab = logPrefab[Random.Range(0, logPrefab.Length)];
        Vector3 position = transform.position + direction * offsetAlongLane;

        // Use the Prefab's rotation so each log keeps the orientation it was designed with.
        GameObject log = Instantiate(prefab, position, prefab.transform.rotation);

        LogMovement mover = log.GetComponent<LogMovement>();
        if (mover != null)
        {
            // A log placed partway along the lane only has the remaining distance left to travel.
            mover.Initialize(laneSpeed, direction, laneLength - offsetAlongLane);
        }
        else
        {
            UnityEngine.Debug.LogWarning("LogSpawner: log prefab has no LogMovement component.", this);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (endPoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, endPoint.position);
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawWireSphere(endPoint.position, 0.3f);
    }
}
