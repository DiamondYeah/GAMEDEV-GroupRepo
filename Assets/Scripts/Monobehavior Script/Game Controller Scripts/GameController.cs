using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Manager Scripts Settings")]
    [SerializeField]
    private FrogSpawner spawner;


    [Header("Game UI Settings")]
    [SerializeField]
    private Button startWavesButton;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startWavesButton.onClick.AddListener(() => StartWaves()); // Add Listener to start frog waves when clicked
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.FlyWasEaten += CheckForFliesLeft;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.FlyWasEaten -= CheckForFliesLeft;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method controls FrogSpawner script to start the frog waves 
    private void StartWaves()
    {
        if(spawner != null) 
        {
            spawner.StartFrogSpawning();
            startWavesButton.gameObject.SetActive(false);
            EventController.RaiseStartWaves();
        }
    }

    // Checks for flies in scene and calls game over if no more flies are left
    private void CheckForFliesLeft()
    {
        GameObject[] flies = GameObject.FindGameObjectsWithTag("Fly");

        if(flies.Length - 1 <= 0)
        {
            EventController.RaiseAllFliesEaten("All flies were eaten!");
        }

    }
}
