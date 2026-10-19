using UnityEngine;

public class TowerStatsHolder : MonoBehaviour
{
    public TowerStats towerStatsHolder;
    [HideInInspector]
    public TowerStats statsHolder;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create a copy of Scriptable Object to be able to edit stats freely without editing the make Scriptable object
        statsHolder = Instantiate(towerStatsHolder); 
    }
}
