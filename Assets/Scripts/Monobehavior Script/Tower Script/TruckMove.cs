using System.Collections;
using UnityEngine;
using static FrogWave;
using static Unity.VisualScripting.Member;

public class TruckMove : MonoBehaviour
{
    [SerializeField] GameObject end;
    [SerializeField] float speed;
    [SerializeField] GameObject parent;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;
    [SerializeField] int damage;
    Rigidbody rb;
    bool playerActivated = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerActivated)
        {

            Vector3 movement = new Vector3(20f, 0.0f, 0.0f);
            rb.linearVelocity = movement * speed;
            StartCoroutine(DestroyTruck());
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        FrogController frogControllerScript = collision.transform.GetComponent<FrogController>();
        if (frogControllerScript != null)
        {
            frogControllerScript.StunFrog();
            frogControllerScript.TakeDamage(damage);
            Debug.Log("Hit enemy");
        }
    }
    public void activateTruck()
    {

        playerActivated = true;
        source.PlayOneShot(clip);
        StartCoroutine(DestroyTruck());
    }

    IEnumerator DestroyTruck()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        Destroy(end);
        Destroy(parent);
    }
    
    
}
