using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class Gun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public float FireCooldown;
    public UnityEvent OnGunShoot;

    private float CurrentCooldown;
    void Start()
    {
        CurrentCooldown = FireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (CurrentCooldown <= 0f)
            {
                OnGunShoot?.Invoke();
                CurrentCooldown = FireCooldown;
            }
        }
    }
}
