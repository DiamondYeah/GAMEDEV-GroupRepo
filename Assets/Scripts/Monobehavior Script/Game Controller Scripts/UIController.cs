using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Space(8)]
    [Header("UI Wave Settings")]
    [SerializeField]
    private TextMeshProUGUI currentWaveText;
    [SerializeField]
    private TextMeshProUGUI timeText;

    [Header("UI Player Settings")]
    [SerializeField]
    private Slider playerHealthBar;
    [SerializeField]
    private Slider playerStaminaBar;
    [SerializeField]
    private GameObject playerStaminaContainer;

    [Header("UI Player Settings")]
    [SerializeField]
    private TextMeshProUGUI amphicoinsHeldText;

    [Header("UI Crosshair Settings")]
    [SerializeField]
    private Image crosshairImage;
     
    // Variables store information on timer
    private bool timerisRunning;
    private float timePassed = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Disable UI elements only visible in player view
        crosshairImage.gameObject.SetActive(false);
        playerStaminaBar.gameObject.SetActive(false);
        playerStaminaContainer.SetActive(false);
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.UpdatedAmphicoinsHeld += UpdateAmphicoinsUI;
        EventController.UpdateHealth += UpdateHealthBar;
        EventController.UpdateStamina += UpdateStaminaBar;
        EventController.StartWaves += () => timerisRunning = true;
        EventController.NextWave += IncrementWaveText;
        EventController.SwitchPlayerMode += ChangeActiveUIInPlayerView;
        EventController.SwitchTowerMode += ChangeActiveUIInTowerView;

    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.UpdatedAmphicoinsHeld -= UpdateAmphicoinsUI;
        EventController.UpdateHealth -= UpdateHealthBar;
        EventController.UpdateStamina -= UpdateStaminaBar;
        EventController.StartWaves -= () => timerisRunning = false;
        EventController.NextWave -= IncrementWaveText;
        EventController.SwitchPlayerMode -= ChangeActiveUIInPlayerView;
        EventController.SwitchTowerMode -= ChangeActiveUIInTowerView;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerisRunning)
        {
            UpdateTimer();
        }
        
    }

    // Method updates player Health UI by calculating percentage with health and maxHealth
    private void UpdateHealthBar(float health, float maxHealth)
    {
        playerHealthBar.value = Mathf.Max(0f, health / maxHealth);
    }

    // Method updates player Health UI by calculating percentage with stamina and maxStamina
    private void UpdateStaminaBar(float stamina, float maxStamina)
    {
        playerStaminaBar.value = Mathf.Max(0f, stamina / maxStamina);
    }

    // Method continously updates the timer every second
    private void UpdateTimer()
    {
        timePassed += Time.deltaTime; // Store elapsed time

        // Obtain the current minutes and seconds
        int minutes = (int)(timePassed / 60f);
        int seconds = (int)(timePassed % 60f);

        // Format the string to show minutes and seconds together and use in timer text
        string timerText = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = timerText;
    }

    // Metphod updates Amphicoins of player when called
    private void UpdateAmphicoinsUI()
    {
        amphicoinsHeldText.text = (CurrencyController.ControllerInstance.amphicoinsAmount).ToString();
    }

    // Method continously updates the timer every second
    private void IncrementWaveText(int wave)
    {
        currentWaveText.text = string.Format("Wave {0}", wave);
    }

    // Method enables and disabled UI inside method if in player view
    private void ChangeActiveUIInPlayerView()
    {
        // Make UI elements inactive

        // Make UI elements active
        crosshairImage.gameObject.SetActive(true);
        playerStaminaBar.gameObject.SetActive(true);
        playerStaminaContainer.SetActive(true);
    }

    // Method enables and disabled UI inside method if in tower view
    private void ChangeActiveUIInTowerView()
    {
        // Make UI elements inactive
        crosshairImage.gameObject.SetActive(false);
        playerStaminaBar.gameObject.SetActive(false);
        playerStaminaContainer.SetActive(false);
        // Make UI elements active
    }

}
