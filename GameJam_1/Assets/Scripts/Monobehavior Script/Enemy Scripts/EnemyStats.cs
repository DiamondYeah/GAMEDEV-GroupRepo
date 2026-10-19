using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{

    [Header("Enemy Health")]
    public float health;
    public bool isVulnerable;


}

