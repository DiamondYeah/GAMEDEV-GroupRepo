using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Space(10)]
    [Header("Player Health")]
    public float health;
    public bool isVulnerable;

    [Space(10)]
    [Header("Player Horizontal Movement Settings")]
    public float baseSpeedMultipler;
    public float maxVelocity;

    [Space(10)]
    [Header("Player Sprint Settings")]
    public float sprintSpeedMultiplier;
    public float sprintSpeedStamina;
    public float sprintStaminaDrain;
    public float sprintStaminaGain;

    [Space(10)]
    [Header("Player Vertical Movement")]
    public float jumpHeight;
    public float groundDetectionDistance = 0.2f;


    [HideInInspector]
    public float movementSpeedMultiplier = 0f;
    [HideInInspector]
    public float currentSpeed = 0f;
}
