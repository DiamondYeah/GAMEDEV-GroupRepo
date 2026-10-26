using System;
using UnityEngine;

public static class EventController
{
    // Game Events
    public static event Action ResetGame;
    public static event Action StartWaves;
    public static event Action<int> NextWave;
    public static event Action<string> AllFliesEaten;
    public static event Action FlyWasEaten;

    // Curreny Events
    public static event Action UpdatedAmphicoinsHeld;

    // Perspective Events
    public static event Action SwitchPlayerMode;
    public static event Action SwitchTowerMode;

    // Player Events
    public static event Action<float, float> UpdateHealth;
    public static event Action<float, float> UpdateStamina;
    public static event Action<float> DamagePlayer;
    public static event Action<string> LostAllHealth;



    // Game Events Functions
    public static void RaiseResetGame() => ResetGame?.Invoke();
    public static void RaiseStartWaves() => StartWaves?.Invoke();
    public static void RaiseNextWave(int wave) => NextWave?.Invoke(wave);
    public static void RaiseAllFliesEaten(string message) => AllFliesEaten?.Invoke(message);
    public static void RaiseFlyWasEaten() => FlyWasEaten?.Invoke();

    // Curreny Events Functions
    public static void RaiseUpdatedAmphicoinsHeld() => UpdatedAmphicoinsHeld?.Invoke();

    // Perspective Events Functions
    public static void RaiseSwitchPlayerMode() => SwitchPlayerMode.Invoke();
    public static void RaiseSwitchTowerMode() => SwitchTowerMode?.Invoke();

    // Player Events Functions
    public static void RaiseUpdateHealth(float health, float maxHealth) => UpdateHealth.Invoke(health, maxHealth);
    public static void RaiseUpdateStamina(float stamina, float maxStamina) => UpdateStamina?.Invoke(stamina, maxStamina);
    public static void RaiseLostAllHealth(string message) => LostAllHealth?.Invoke(message);
    public static void RaiseDamagePlayer(float damage) => DamagePlayer?.Invoke(damage);
}