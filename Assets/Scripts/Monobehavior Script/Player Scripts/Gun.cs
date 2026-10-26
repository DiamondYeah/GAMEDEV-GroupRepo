using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class Gun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public float FireCooldown;
    [SerializeField] GameObject gun;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    private MeshRenderer GunMesh;
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
                source.PlayOneShot(clip);
                OnGunShoot?.Invoke();
                StartCoroutine(ShowGun());
                CurrentCooldown = FireCooldown;
            }
        }
    }
    IEnumerator ShowGun()
    {
        GunMesh = gun.GetComponent<MeshRenderer>();
        GunMesh.enabled = true;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = enabled;
        }
        yield return new WaitForSeconds(1.5f);
        GunMesh.enabled = false;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
    }
}
