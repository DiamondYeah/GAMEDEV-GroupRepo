using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Health")]
    public float health;
    public bool isVulnerable;

    [Header("Player Horizontal Movement")]
    public float movementSpeedMultiplier;
    public float maxVelocity;

    [Header("Player Vertical Movement")]
    public float jumpHeight;
    public float groundDetectionDistance = 0.2f;
}
