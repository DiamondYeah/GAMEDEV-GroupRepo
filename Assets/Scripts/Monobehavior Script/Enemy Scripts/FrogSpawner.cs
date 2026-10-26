using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class FrogWave
{
    [Space(8)]
    [Header("Wave Settings")]
    public int waveDuration;
    public int maxFrogSpawns;

    [Space(8)]
    [Header("Spawn Interval Settings")]
    public float minSpawnInterval;
    public float maxSpawnInterval;

    [Serializable]
    public class Frog
    {
        public GameObject frog;
        [Range(0f, 1f)]
        public float spawnChance = 0f;

    }

    [Header("Frog Spawn Settings")]
    public Frog[] frogs;
    
}

public class FrogSpawner : MonoBehaviour
{
    [Header("Frog Location Settings")]
    [SerializeField]
    private GameObject frogSpawnZone;
    [SerializeField]
    private Transform[] waypointLocations;

    [Space(16)]
    [Header("Frog Spawner Settings")]
    [SerializeField]
    private FrogWave[] waves;


    private float currentTime = 0f;

    [HideInInspector]
    public static int currentWave = 1;

    // Stores the frog spawn collider to get bounds for randomized spawning
    private Collider frogSpawnCollider;

    // Stores whether script is spawning frogs or not
    private bool isSpawningFrogs = false;

    // Stores Coroutine for frog spawning
    private Coroutine spawnRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWave = 1;

        frogSpawnCollider = frogSpawnZone.GetComponent<Collider>();
    }

    private void Update()
    {
        // Only consider time when frogs are being spawned
        if (isSpawningFrogs)
        {
            // Get current wave and number of frogs in scene
            FrogWave selectedWave = waves[currentWave - 1];

            currentTime += Time.deltaTime;

            if (currentTime >= selectedWave.waveDuration)
            {
                currentWave++;
                EventController.RaiseNextWave(currentWave);

                currentTime = 0f;
            }
        }
    }
    // Add events to be observed when active
    private void OnEnable()
    {

    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {

    }

    // Method starts frog spawning 
    public void StartFrogSpawning()
    {
        if(spawnRoutine == null)
        {
            spawnRoutine = StartCoroutine(SpawnFrogs());
            isSpawningFrogs = true;
        }

    }

    // Method stops frog spawning 
    public void StopFrogSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
            isSpawningFrogs = false;
        }


    }

    // Method returns a random Vector3 Location of a frog spawn within the frog collider's bounds via Random.Range on all 3 axis
    private Vector3 RandomizeFrogSpawnLocation()
    {
        return new Vector3(UnityEngine.Random.Range(frogSpawnCollider.bounds.min.x, frogSpawnCollider.bounds.max.x),
                           UnityEngine.Random.Range(frogSpawnCollider.bounds.min.y, frogSpawnCollider.bounds.max.y),
                           frogSpawnZone.transform.position.z);
    }

    // Method returns a random float determining when the next frog spawns based on the min and max spawn interval passed
    private float RandomizeSpawnInterval(float min, float max)
    {
        return (UnityEngine.Random.Range(min, max));
    }

    // Method returns a random Frog gameobject via a random number based on the possible number of frogs listed in the wave
    private GameObject RandomizeFrogPick(FrogWave selectedWave)
    {
        GameObject selectedFrog = null;

        // While loop to keep getting frog until one is selected
        while(selectedFrog == null)
        {
            int randomNum = UnityEngine.Random.Range(0, selectedWave.frogs.Length); // Get number of possible frogs to spawn in the wave
            
            // Get a random number within 1-100, and check if the num is equal or less than frog's spawn chance. Select frog if true
            int chanceNum = UnityEngine.Random.Range(1, 101); // 0 spawnChance will never spawn
            Debug.Log(chanceNum);
            if (chanceNum <= (selectedWave.frogs[randomNum].spawnChance * 100))
            {
                Debug.Log("Frog Selected");
                selectedFrog = selectedWave.frogs[randomNum].frog;
                break;
            }
        }

        return selectedFrog;
    }

    // Coroutine continously spawns frogs based on a spawntimer and will 
    private IEnumerator SpawnFrogs()
    {
        while (true)
        {
            // Get current wave and number of frogs in scene
            FrogWave selectedWave = waves[currentWave - 1];

            int numOfCurrentFrogs = GameObject.FindGameObjectsWithTag("Frog").Length;

            // Wait for a randomized spawn interval based on returned value
            yield return new WaitForSeconds(RandomizeSpawnInterval(selectedWave.minSpawnInterval, selectedWave.maxSpawnInterval));

            if (selectedWave.maxFrogSpawns > numOfCurrentFrogs)
            {   
                GameObject selectedFrog = RandomizeFrogPick(selectedWave);
                GameObject spawnedFrog = Instantiate(selectedFrog, RandomizeFrogSpawnLocation(), selectedFrog.transform.rotation);
                spawnedFrog.GetComponent<FrogController>().InitializeWaypoints(waypointLocations);
            }

        }
    }
}
