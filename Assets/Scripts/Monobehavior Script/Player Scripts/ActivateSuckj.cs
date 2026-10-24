using System.Collections;
using UnityEngine;

public class ActivateSuckj : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider myCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCollider = GetComponent<CapsuleCollider>();
        myCollider.enabled = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        myCollider.enabled = true;
        myCollider.isTrigger = true;
        rb.isKinematic = true;
        StartCoroutine(DestroyBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.CompareTag("Frog"))
        {
           Rigidbody frogRb = other.gameObject.GetComponent<Rigidbody>();
           Vector3 suctionPoint = (gameObject.transform.position - other.transform.position).normalized; 
           Debug.Log("Test");
           
           frogRb.AddForce(suctionPoint * 20, ForceMode.Impulse);
        }
        
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
