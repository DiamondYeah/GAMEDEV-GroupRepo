using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Truck : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject end;
    [SerializeField] float speed;
    [SerializeField] Material material;
    MeshRenderer meshRenderer;
    Material[] currentMaterials;
    bool isActivated = false;
    bool playerActivated = false;
    
    void Start()
    {
       meshRenderer = GetComponent<MeshRenderer>();
       currentMaterials = meshRenderer.materials;
    }

    // Update is called once per frame
    void Update()
    {
       if(playerActivated)
        {
          transform.position = Vector3.MoveTowards(transform.position, new Vector3(end.transform.position.x, transform.position.y, transform.position.z), speed * Time.deltaTime);

        }
       if(transform.position.x == end.transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    public void canActivateTruck()
    {
       if(!isActivated)
       {
        isActivated = true;
        Material[] truckMaterials = new Material[meshRenderer.materials.Length + 1];
        truckMaterials[0] = currentMaterials[0];
        truckMaterials[1] = material;
        meshRenderer.materials = truckMaterials;
       }
       

    }

    public void Restore()
    {
        meshRenderer.materials = currentMaterials;
        isActivated = false;

    }
    public void activateTruck()
    {
       
        playerActivated = true;
    }
}
