using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Scriptable Objects/TowerStats")]
public class TowerStats : ScriptableObject
{
    public LayerMask validPlacementType;
    public bool canBeDamaged;
    public float health;
}
