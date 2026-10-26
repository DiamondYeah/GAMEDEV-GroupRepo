using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonUI : MonoBehaviour
{
    [Header("UI Elements Settings")]
    [SerializeField]
    private Image buttonImage;
    [SerializeField]
    private TextMeshProUGUI towerNameText;
    [SerializeField]
    private TextMeshProUGUI towerCostText;

    // Method sets up the elements of the tower UI button such as name and cost through the passed Tower Statst scriptable object
    public void SetupTowerButton(TowerStats stats)
    {
        buttonImage.sprite = stats.towerImage;
        towerNameText.text = stats.towerName;
        towerCostText.text = stats.towerCost.ToString();

    }

}
