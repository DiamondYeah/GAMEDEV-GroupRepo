using UnityEngine;
using UnityEngine.InputSystem;

public class PerspectiveController : MonoBehaviour
{
    [Header("Keybind Settings")]
    [SerializeField]
    private InputActionReference switchPerspectiveAction;

    [Header("Tower Placement Perspective Settings")]
    [SerializeField]
    private Camera towerCamera;

    [Header("Tower Player Perspective Settings")]
    [SerializeField]
    private Camera playerCamera;

    public static Camera ActiveCamera { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Show tower camera at start
        towerCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);
        ActiveCamera = towerCamera;
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        switchPerspectiveAction.action.Enable();
        EventController.LostAllHealth += (string message) => { towerCamera.gameObject.SetActive(true); playerCamera.gameObject.SetActive(false); };
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        switchPerspectiveAction.action.Disable();
        EventController.LostAllHealth -= (string message) => { towerCamera.gameObject.SetActive(true); playerCamera.gameObject.SetActive(false); };
    }

    // Update is called once per frame
    private void Update()
    {
        // If key was pressed switch perspectives
        if(switchPerspectiveAction.action.WasPressedThisFrame()){
            Debug.Log("SWITCH");
            SwitchPerspectives();
        }

    }

    // Method switches perspective from tower to player view and vice versa, including enabling mechanics of said view
    private void SwitchPerspectives()
    {
        bool isPlayerView = towerCamera.gameObject.activeInHierarchy;
        // Switch perspectives by doing inverse whether what was active in the hierarchy
        towerCamera.gameObject.SetActive(!isPlayerView);
        playerCamera.gameObject.SetActive(isPlayerView);

        // Switch active Camera
        ActiveCamera = isPlayerView ? playerCamera : towerCamera;

        Debug.Log(ActiveCamera);

        if (isPlayerView)
        {
            EventController.RaiseSwitchPlayerMode();

            // Hides cursor and makes it unable to move postiion
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            EventController.RaiseSwitchTowerMode();

            // Shows cursor and allows it to move within screen
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
