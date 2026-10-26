using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Purchasing;
using UnityEngine.UI;

[Serializable]
public class TowerTypeColor
{
    public string typeName;
    public Color color;
}

[Serializable]
public class PlaceableTower
{
    public Button towerButton;
    public GameObject towerObject;
    public GameObject towerImage;

    [HideInInspector]
    public Color towerColorType; // Will be set after DisplayTowerStatsToButton call for reference of previous Color background
}

public class TowerPlacingController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    private Camera cameraTower;
    [SerializeField]
    private float maxPlaceDistance;
    [SerializeField]
    private float distanceAboveMouse;
    [SerializeField]
    private LayerMask validHitMask;

    [Space(10)]
    [Header("Placement Settings")]
    [SerializeField]
    [Range(0f, 1f)]
    private float selectTransparencyValue = 0.5f;

    [Space(10)]
    [Header("Overlapping Settings")]
    [SerializeField]
    private float overlappingDistance;
    [SerializeField]
    private LayerMask towerLayerMask;

    [Space(10)]
    [Header("Insufficient Currency Settings")]
    [SerializeField]
    private Color insufficientAmountColor;

    [Space(10)]
    [Header("Validity Placing Settings")]
    [SerializeField]
    private Color acceptPlacementColor;
    [SerializeField]
    private Color denyPlacementColor;
    [SerializeField]
    private LayerMask validPlacementLayers;

    [Space(10)]
    [Header("Truck Settings")]
    [Tooltip("Spawn points ordered LEFT to RIGHT. Truck spawns at the first free one.")]
    [SerializeField]
    private Transform[] truckSpawnPoints;
    [Tooltip("A spawn point is free once the truck spawned there is farther than this (or destroyed).")]
    [SerializeField]
    private float truckSpawnClearDistance = 5f;

    [Space(10)]
    [Header("Tower UI Settings")]
    [SerializeField] 
    private TowerTypeColor[] typeColors;

    [Space(10)]
    [Header("Tower List")]
    [SerializeField]
    private PlaceableTower[] towerList;

    private Ray cameraRay;

    private GameObject selectedTower;
    private int selectedTowerPrice;

    private GameObject ghostTower;
    private MeshRenderer ghostTowerRender;
    private GameObject[] spawnedTrucks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnedTrucks = new GameObject[truckSpawnPoints.Length];
        foreach (PlaceableTower tower in towerList)
        {
            tower.towerButton.onClick.AddListener(() => SelectTower(tower.towerObject));
            DisplayTowerStatsToButton(tower);
        }

        CheckIfPurchasable(); // Check for any items that can be purchasable or not at start
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.UpdatedAmphicoinsHeld += CheckIfPurchasable;
        EventController.SwitchPlayerMode += DisablePlacing;
        EventController.SwitchTowerMode += EnablePlacing;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.UpdatedAmphicoinsHeld -= CheckIfPurchasable;
        EventController.SwitchPlayerMode -= DisablePlacing;
        EventController.SwitchTowerMode -= EnablePlacing;
    }

    // Update is called once per frame
    private void Update()
    {
        PlaceTowerInScene();
    }

    // Method adjusts tower button to dispaly stats of the tower
    private void DisplayTowerStatsToButton(PlaceableTower tower)
    {
        // Get Button and tower object
        GameObject towerObject = tower.towerObject;
        Button towerButton = tower.towerButton;

        // Get tower stats of tower parameter
        TowerStats stats = towerObject.GetComponent<TowerController>().towerStatsHolder;

        // Go through typeColors array and find the tower type. If found, fill out the details of the button
        foreach (TowerTypeColor typeColor in typeColors)
        {
            if (typeColor.typeName.Equals(stats.validPlacementType.ToString()))
            {
                towerButton.GetComponent<Image>().color = typeColor.color;
                towerButton.GetComponent<TowerButtonUI>().SetupTowerButton(stats);
                tower.towerColorType = typeColor.color;
            }
        }
    }

    // Method checks towerList and checks if current currency is enough to purchase tower or not. Disables button if not enough
    private void CheckIfPurchasable()
    {
        foreach (PlaceableTower tower in towerList)
        {
            // Get tower stats of tower parameter and check if purchasable
            TowerStats stats = tower.towerObject.GetComponent<TowerController>().towerStatsHolder;
            Button towerButton = tower.towerButton;

            if (!CurrencyController.ControllerInstance.canPurchase(stats.towerCost))
            {
                towerButton.enabled = false;
                towerButton.GetComponent<Image>().color = insufficientAmountColor;
            }
            else
            {
                towerButton.enabled = true;
                towerButton.GetComponent<Image>().color = tower.towerColorType;
            }
        }
    }

    // Method disables tower placing by setting tower buttons to be non actuve
    private void DisablePlacing()
    {
        selectedTower = null;
        foreach (PlaceableTower button in towerList)
        {
            button.towerButton.gameObject.SetActive(false);
        }

        Destroy(ghostTower); // Destroy ghostTower null to avoid it existing in player view
    }

    // Method disables tower placing by setting tower buttons to be actuve
    private void EnablePlacing()
    {
        foreach (PlaceableTower button in towerList)
        {
            button.towerButton.gameObject.SetActive(true);
        }

    }

    // Method performs checks such as valid area or if tower was selected before placing tower in scene via ScreenPointToRay
    private void PlaceTowerInScene()
    {
        // Read the mouse 2D Position, and add use it to create a Camera ray that points to that position

        Vector2 mouse2DPOS = Mouse.current.position.ReadValue();
        cameraRay = cameraTower.ScreenPointToRay(new Vector3(mouse2DPOS.x, mouse2DPOS.y, 0f));

        RaycastHit hit;
        // Check if the raycast has hit an object to allow spawning of objects
        if (Physics.Raycast(cameraRay, out hit, maxPlaceDistance, validHitMask))
        {
            // Find the hit position via point and add point's position with distanceAboveMouse
            Vector3 hitPos = new Vector3(hit.point.x, hit.point.y + distanceAboveMouse, hit.point.z);

            if (ghostTower != null)
            {
               
                ghostTower.SetActive(true);
                ghostTower.transform.position = hitPos;
            }

            // Make sure that player can purchase tower before proceeding. Stop if they can't
            if (!CurrencyController.ControllerInstance.canPurchase(selectedTowerPrice))
            {
                selectedTower = null;

                // Display deny color if ghost tower is visible
                if(ghostTower != null)
                {
                    ghostTowerRender.material.color = new Color(denyPlacementColor.r, denyPlacementColor.g,
                                denyPlacementColor.b, selectTransparencyValue);
                }

                return;
            }

            // Check for left mouse click and determine if placement is valid before instantiating
            bool wasLeftMouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            bool isMouserNotOverUI = !EventSystem.current.IsPointerOverGameObject();
            if (selectedTower != null && IsPlacementValid(hitPos, hit) && wasLeftMouseClicked && isMouserNotOverUI)
            {
           
                
                    Instantiate(selectedTower, hitPos, Quaternion.Euler(0f, 180f, 0f));
                    CurrencyController.ControllerInstance.SpendCurrency(selectedTowerPrice); // Update amount to spend
                

            }
        }
    }

    // Method changes the selected tower to the parameter passed to function
    private void SelectTower(GameObject tower)
    {
        selectedTower = tower;
        selectedTowerPrice = tower.GetComponent<TowerController>().towerStatsHolder.towerCost;

        // Remove previous ghost tower
        if (ghostTower != null)
        {
            Destroy(ghostTower);
        }
        if (tower.CompareTag("Truck"))
        {
            selectedTower = null;
            TrySpawnTruck(tower);
            return;
        }
        else
        {
            ghostTower = Instantiate(tower, transform.position, Quaternion.Euler(0f, 180f, 0f));
            ghostTower.layer = LayerMask.NameToLayer("Ghost");

            ghostTowerRender = ghostTower.GetComponent<MeshRenderer>();
            ghostTowerRender.material.color = new Color(ghostTowerRender.material.color.r, ghostTowerRender.material.color.g,
                                                        ghostTowerRender.material.color.b, selectTransparencyValue);
        }

    }


    // Checks whether placement is valid by checking conditions such as overlapping and valid floor. Also changes ghost's color to signal if valid or not
    private bool IsPlacementValid(Vector3 hitPos, RaycastHit hit)
    {
        // Check using a sphere if there are any nearby towers that block placement
        bool isOverlapping = Physics.CheckSphere(hitPos, overlappingDistance, towerLayerMask);

        // Obtain the layer hit by the Raycast and tower stats to check if it's a valid layer to place in based on possible ray and tower layers
        TowerStats stats = selectedTower.GetComponent<TowerController>().towerStatsHolder;
        string LayerHitName = LayerMask.LayerToName(hit.collider.gameObject.layer);
        bool isAreaValidLayerToRay = validPlacementLayers.ToString().Contains(LayerHitName);
        bool isAreaValidLayerToTower = stats.validPlacementType.ToString().Contains(LayerHitName);

        bool isPlacementValid = !isOverlapping && isAreaValidLayerToRay && isAreaValidLayerToTower;

        // Change ghost's material color depending if placement is valid or not
        if (isPlacementValid)
        {
            ghostTowerRender.material.color = new Color(acceptPlacementColor.r, acceptPlacementColor.g,
                                                        acceptPlacementColor.b, selectTransparencyValue);
        }
        else
        {
            ghostTowerRender.material.color = new Color(denyPlacementColor.r, denyPlacementColor.g,
                                            denyPlacementColor.b, selectTransparencyValue);
        }

        return isPlacementValid;
    }

    private void TrySpawnTruck(GameObject truckPrefab)
    {
        int price = truckPrefab.GetComponent<TowerController>().towerStatsHolder.towerCost;

        for (int i = 0; i < truckSpawnPoints.Length; i++)
        {
            if (!IsSpawnPointFree(i))
            {
                continue;
            }

            Transform spawnPoint = truckSpawnPoints[i];
            spawnedTrucks[i] = Instantiate(truckPrefab, spawnPoint.position, spawnPoint.rotation);
            CurrencyController.ControllerInstance.SpendCurrency(price);
            return;
        }

        Debug.Log("No free truck spawn point available");
    }
    private bool IsSpawnPointFree(int index)
    {
        GameObject lastTruck = spawnedTrucks[index];

        if (lastTruck == null)
        {
            return true;
        }

        float distance = Vector3.Distance(lastTruck.transform.position, truckSpawnPoints[index].position);
        return distance > truckSpawnClearDistance;
    }


}
