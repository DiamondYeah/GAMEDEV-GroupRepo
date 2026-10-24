using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    public EnemyStats enemyStats;
    public float enemyHealth;

    Rigidbody rb;
    void Start()
    {
        
        enemyHealth =  enemyStats.health;
        Debug.Log(enemyStats.health);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        
        if (enemyHealth <= 0)
        {
            rb.detectCollisions = false;
            StartCoroutine(DestroyFrog());


        }
    }

    IEnumerator DestroyFrog()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
