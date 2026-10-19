using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public class PlaceableTower
{
    public Button towerButton;
    public GameObject towerObject;
}

public class TowerPlacingController : MonoBehaviour
{

    [Header("General Settings")]
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
    [Header("Validity Placing Settings")]
    [SerializeField]
    private Color acceptPlacementColor;
    [SerializeField]
    private Color denyPlacementColor;
    [SerializeField]
    private LayerMask validPlacementLayers;

    [Space(10)]
    [Header("Tower List")]
    [SerializeField]
    private PlaceableTower[] towerList;


    private Ray cameraRay;

    private GameObject selectedTower;
    private GameObject ghostTower;
    private MeshRenderer ghostTowerRender;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(PlaceableTower tower in towerList)
        {
            tower.towerButton.onClick.AddListener(() => SelectTower(tower.towerObject));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Read the mouse 2D Position, and add use it to create a Camera ray that points to that position
        Vector2 mouse2DPOS = Mouse.current.position.ReadValue();
        cameraRay = Camera.main.ScreenPointToRay(new Vector3(mouse2DPOS.x, mouse2DPOS.y, 0f));

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

            bool wasLeftMouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
            bool isMouserNotOverUI = !EventSystem.current.IsPointerOverGameObject();
            if (selectedTower != null && IsPlacementValid(hitPos, hit) && wasLeftMouseClicked && isMouserNotOverUI)
            {
                Instantiate(selectedTower, hitPos, transform.rotation);
            }
        }

        

    }

    // Method changes the selected tower to the parameter passed to function
    private void SelectTower(GameObject tower)
    {
        selectedTower = tower;

        if (ghostTower != null)
        {
            Destroy(ghostTower);
        }
        ghostTower = Instantiate(tower);
        ghostTower.layer = LayerMask.NameToLayer("Ghost");

        ghostTowerRender = ghostTower.GetComponent<MeshRenderer>();
        ghostTowerRender.material.color = new Color(ghostTowerRender.material.color.r, ghostTowerRender.material.color.g, 
                                                    ghostTowerRender.material.color.b, selectTransparencyValue);

    }


    // Checks whether placement is valid by checking conditions such as overlapping and valid floor. Also changes ghost's color to signal if valid or not
    private bool IsPlacementValid(Vector3 hitPos, RaycastHit hit)
    {
        // Check using a sphere if there are any nearby towers that block placement
        bool isOverlapping = Physics.CheckSphere(hitPos, overlappingDistance, towerLayerMask);

        // Obtain the layer hit by the Raycast and tower stats to check if it's a valid layer to place in based on possible ray and tower layers
        TowerStatsHolder stats = selectedTower.GetComponent<TowerStatsHolder>();
        string LayerHitName = LayerMask.LayerToName(hit.collider.gameObject.layer);
        bool isAreaValidLayerToRay = validPlacementLayers.ToString().Contains(LayerHitName);
        bool isAreaValidLayerToTower = stats.towerStatsHolder.validPlacementType.ToString().Contains(LayerHitName);

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
}
