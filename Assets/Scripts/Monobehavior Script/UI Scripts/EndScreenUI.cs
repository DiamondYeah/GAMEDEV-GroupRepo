using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] 
    private GameObject EndScreenPanel;
    [SerializeField] 
    private TMP_Text EndScreenMessage;
    [SerializeField]
    private TMP_Text wavesReachedMessage;

    [SerializeField] string menuSceneName = "MainMenu";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EndScreenPanel.SetActive(false);
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.AllFliesEaten += ShowEndScreen;
        EventController.LostAllHealth += ShowEndScreen;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.AllFliesEaten -= ShowEndScreen;
        EventController.LostAllHealth -= ShowEndScreen;
    }

    public void ShowEndScreen(string message)
    {
        EndScreenMessage.text = message;
        wavesReachedMessage.text = "Reached Wave " + FrogSpawner.currentWave.ToString();
        EndScreenPanel.SetActive(true);

        // Pause the game
        Time.timeScale = 0f; 

        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        // Resume the game
        Time.timeScale = 1f;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        // Resume the game
        Time.timeScale = 1f;

        // Load the main menu scene
        SceneManager.LoadScene(menuSceneName);
    }

    public bool isEndScreenActive()
    {
        return EndScreenPanel.activeSelf;
    }
}
