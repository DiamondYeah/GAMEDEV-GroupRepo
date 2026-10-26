using UnityEngine;

[CreateAssetMenu(fileName = "TowerStats", menuName = "Scriptable Objects/TowerStats")]
public class TowerStats : ScriptableObject
{
    [Header("Tower General Info Settings")]
    public string towerName;
    public Sprite towerImage;
    public LayerMask validPlacementType;

    [Space(10)]
    [Header("Tower Cost Settings")]
    public int towerCost;

    [Space(10)]
    [Header("Tower Health Settings")]
    public bool canBeDamaged;
    public float health;

    [Space(10)]
    [Header("Tower Damage Settings")]
    public bool canDealDamage;
    public LayerMask validAttackMasks;
    public int damage;
    public float range;
    public float attackDelay;

}
