using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string levelSceneName = "FroggerGameJam";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();                              // Quit the application
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
    }
}
