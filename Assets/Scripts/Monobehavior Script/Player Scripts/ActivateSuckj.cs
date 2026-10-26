using System.Collections;
using UnityEngine;
using static FrogWave;

public class ActivateSuckj : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField] int stunDuration;
    CapsuleCollider myCollider;
    bool SoundPlayed = false;
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
        if(!SoundPlayed)
        {
            source.PlayOneShot(clip);
        }
        SoundPlayed = true;
        myCollider.isTrigger = true;
        rb.isKinematic = true;
        StartCoroutine(DestroyBullet());

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.gameObject.CompareTag("Frog"))
        {
            SuckEnemy(other);
        }
        
    }

    IEnumerator DestroyBullet()
    {
        
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    void SuckEnemy(Collider other)
    {
        Rigidbody frogRb = other.GetComponent<Rigidbody>();
        FrogController frogControllerScript = other.GetComponent<FrogController>();
        Vector3 suctionPoint = (transform.position - other.transform.position).normalized;

        frogControllerScript.StunFrog();
        frogRb.AddForce(suctionPoint * 20, ForceMode.Impulse);

        // Timer runs on the frog, so it survives the bullet being destroyed
        frogControllerScript.StartCoroutine(UnstunAfterDelay(frogControllerScript, stunDuration));
    }

    static IEnumerator UnstunAfterDelay(FrogController frog, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (frog != null)
            frog.UnstunFrog();
    }
}
