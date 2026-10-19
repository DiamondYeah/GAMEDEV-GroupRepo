using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    EnemyStats enemyStats;
    Rigidbody rb;
    void Start()
    {
        enemyStats.health = 100;
        Debug.Log(enemyStats.health);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage (int damage)
    {
        enemyStats.health -= damage;
        Debug.Log(enemyStats.health);
       
    }
}
