using System;

public static class EventController
{
    public static event Action<float> DamagePlayer;
    public static event Action ResetGame;

    public static void RaiseDamagePlayer(float damage) => DamagePlayer?.Invoke(damage);
    public static void RaiseResetGame() => ResetGame?.Invoke();
}