
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] 
    private GameObject pausePanel;
    [SerializeField] 
    private EndScreenUI endScreen;
    [SerializeField] 
    private PlayerController script;

    [Header("Scene Management")]
    [SerializeField] 
    private string menuSceneName = "MainMenu";

    [SerializeField] 
    private InputAction pauseAction = new InputAction("Pause", InputActionType.Button, "<Keyboard>/escape");

    private bool isPaused = false;
    private bool isOnTowerView = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pausePanel.SetActive(false);
    }

    private void OnEnable()
    {
        pauseAction.performed += OnPausePressed;
        EventController.SwitchPlayerMode += () => isOnTowerView = false;
        EventController.SwitchTowerMode += () => isOnTowerView = true;
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        EventController.SwitchPlayerMode -= () => isOnTowerView = false;
        EventController.SwitchTowerMode -= () => isOnTowerView = true;
        pauseAction.Disable();
    }


    // This method is called when the pause action is performed
    void OnPausePressed(InputAction.CallbackContext ctx)
    {
        // If the end screen is active, do not allow pausing
        if (endScreen != null && endScreen.isEndScreenActive()) return;

        if (isPaused) Resume();
        else Pause();


    }

    // Pauses the game
    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        script.DisableActions();
    }

    // Resumes the game
    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);

        Time.timeScale = 1f;

        if (isOnTowerView)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        script.EnableActions();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
