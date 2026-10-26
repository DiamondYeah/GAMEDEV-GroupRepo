using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TruckActivate : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] TruckMove truck;
    MeshRenderer meshRenderer;
    bool isActivated = false;

    Rigidbody rb;
    
    void Start()
    {
      
       meshRenderer = GetComponent<MeshRenderer>();
       meshRenderer.enabled = false;
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && isActivated == true)
        {
            truck.activateTruck();
            
            Destroy(gameObject);
        }
    }
    public void canActivateTruck()
    {
       if(!isActivated)
       {
            isActivated = true;

            meshRenderer.enabled = true;
         
            }
       }
    public void Restore()
    {
        meshRenderer.enabled = false;
        isActivated = false;

    }

}



