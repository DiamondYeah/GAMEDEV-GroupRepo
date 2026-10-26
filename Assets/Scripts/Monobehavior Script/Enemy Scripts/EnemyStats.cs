using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Enemy Health")]
    public float health;
    public bool isVulnerable;

    [Header("Enemy Agent Settings")]
    public float speed;
    public float jumpDelay;
    public float nextPathCheck;
    public float nextFlyCheck;
    public float arriveDistance;

    [Header("Consume Settings")]
    public string flyConsumeTag;
    public float consumeDistance;

    [Header("Amphicoins Settings")]
    public int coinsWhenKilled;

}

