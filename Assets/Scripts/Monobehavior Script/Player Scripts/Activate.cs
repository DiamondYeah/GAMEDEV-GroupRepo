using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class Activate : MonoBehaviour
{
    [SerializeField] int raySize;
    [SerializeField] Material material;
    TruckActivate lastTruck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TruckActivate current = null;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitinfo, raySize))
        {

            GameObject hit = hitinfo.transform.gameObject;
            
            if (hit.CompareTag("Truck"))
            {
                current = hit.GetComponent<TruckActivate>();
                lastTruck = hit.GetComponent<TruckActivate>();
                lastTruck.canActivateTruck();
            }
        }
        if (current != lastTruck)
        {
            if (lastTruck != null) 
                lastTruck.Restore();
            if (current != null) 
                current.canActivateTruck();
            lastTruck = current;
        }
    }
}
