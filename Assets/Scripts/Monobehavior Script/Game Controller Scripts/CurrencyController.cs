using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    [SerializeField]
    private int startingAmount = 0;


    public int amphicoinsAmount { get; private set; }

    public static CurrencyController ControllerInstance { get; private set; }

    // Awake is called when loading an instance of a script component. Mainly used for initializing and instantiating objects
    void Awake()
    {
        // Create a singleton manager by making sure only one instance exists
        if (ControllerInstance != null && ControllerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        ControllerInstance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Add starting amount to amount held and raise event to update UI
        amphicoinsAmount = startingAmount;
        EventController.RaiseUpdatedAmphicoinsHeld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.ResetGame += ResetHeldCoins;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.ResetGame -= ResetHeldCoins;
    }

    public bool canPurchase(int amountRequired)
    {
        return amphicoinsAmount >= amountRequired;
    }

    public bool SpendCurrency(int amount)
    {
        if (canPurchase(amount))
        {
            amphicoinsAmount -= amount;
            EventController.RaiseUpdatedAmphicoinsHeld();
            return true;
        }

        return false;
    }

    public void AddHeldAmount(int amount)
    {
        if(amount > 0)
        {
            amphicoinsAmount += amount;
            EventController.RaiseUpdatedAmphicoinsHeld();
        }
    }

    // Method resets coins held to starting amount when called
    private void ResetHeldCoins()
    {
        amphicoinsAmount = startingAmount;
    }
}
