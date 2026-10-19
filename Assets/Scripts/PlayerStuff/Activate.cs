using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class Activate : MonoBehaviour
{
    [SerializeField] int raySize;
    [SerializeField] Material material;
    Truck lastTruck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitinfo, raySize))
        {
            GameObject hit = hitinfo.transform.gameObject; 
            if (hit.CompareTag("Truck"))
            {
                lastTruck =hit.GetComponent<Truck>();
                lastTruck.canActivateTruck();
                if(Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Debug.Log("That is the truck");
                    lastTruck.activateTruck();
                }
                
            }
            else if (lastTruck != null)
            {
                Debug.Log("Restore attempt");
                lastTruck.Restore();
                lastTruck = null;
            }
        }
    }
}
