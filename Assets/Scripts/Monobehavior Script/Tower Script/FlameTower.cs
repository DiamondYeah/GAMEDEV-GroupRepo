using System;
using UnityEngine;

public class FlameTower : MonoBehaviour
{
    [SerializeField] float burnTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.gameObject.CompareTag("Frog"))
        {
            Burning EnemyBurn = other.transform.GetComponent<Burning>();
            EnemyBurn.SetBurnTimer(burnTime);

        }
    }
}
