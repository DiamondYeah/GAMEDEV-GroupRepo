using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    [Header("Stats Settings")]
    public TowerStats towerStatsHolder;

    [Header("UI Settings")]
    public Slider healthBar;

    [HideInInspector]
    public TowerStats statsHolder;

    // Awake is called when loading an instance of a script component. Mainly used for initializing and instantiating objects
    void Awake()
    {
        // Create a copy of tower stats that will be edited on runtime and be referenced against
        statsHolder = Instantiate(towerStatsHolder);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Adjust max and current health of slider based on health in scriptable object
        healthBar.maxValue = statsHolder.health;
        healthBar.value = statsHolder.health;
    }

    // Method makes tower take damage when called based on parameter passed and adjust health bar aswell
    public void TakeDamage(int damage)
    {
        statsHolder.health = Mathf.Max(0f, statsHolder.health - damage); // Avoid health going below 0
        healthBar.value = statsHolder.health;

        // If health is 0, reset navmesh path before destroying frog
        if (statsHolder.health <= 0f)
        {
            DestroyTower();
        }
    }

    // Method destroys tower when called
    private void DestroyTower()
    {
        Destroy(gameObject);
    }
}
