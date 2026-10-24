using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Pregnant : MonoBehaviour
{
    [SerializeField] Enemy frog;
    [SerializeField] GameObject enemy;
    bool notExploded = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
     
    }

    // Update is called once per frame
    void Update()
    {
        if(frog.enemyHealth <= 0 && notExploded)
        {
            notExploded = false;
            Vector3 nextPos;
            Debug.Log("Death");
            for (int i= 0;  i<5;  i++)
            {
                if (i % 2 == 0)
                {
                    nextPos = new Vector3(0, 0, -0.5f - (1f + i * 1f));
                }
                else
                {
                    nextPos = new Vector3(0, 0, 0.5f + (1f + i * 1f));
                }
                Debug.Log(nextPos);
                GameObject newFrog = Instantiate(enemy, gameObject.transform.position + nextPos, Quaternion.identity);
                
                Rigidbody frogrb = newFrog.GetComponent<Rigidbody>();
  

            }
        }
    }

}
