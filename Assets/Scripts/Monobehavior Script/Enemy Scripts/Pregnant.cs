using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Pregnant : MonoBehaviour
{
    [SerializeField] 
    private FrogController frogScript;
    [SerializeField]
    private int numOfSpawns;
    [SerializeField]
    private GameObject frogSpawn;

    private FrogMovementController movementScript;
    private bool notExploded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frogScript.stopImmediateKill = true;

        movementScript = GetComponent<FrogMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(frogScript.stats.health <= 0 && notExploded)
        {
            notExploded = false;
            Vector3 nextPos;
            Debug.Log("Death");
            for (int i= 0;  i< numOfSpawns;  i++)
            {
                if (i % 2 == 0)
                {
                    nextPos = new Vector3(0, 1.5f, -0.5f - (1f + i * 1f));
                }
                else
                {
                    nextPos = new Vector3(0, 1.5f, 0.5f + (1f + i * 1f));
                }
                Debug.Log(nextPos);
                GameObject newFrog = Instantiate(frogSpawn, gameObject.transform.position + nextPos, Quaternion.identity);
                
                Rigidbody frogrb = newFrog.GetComponent<Rigidbody>();
                FrogMovementController newFrogMovementScript = newFrog.GetComponent<FrogMovementController>();

                // Set current waypoints and destination count
                newFrogMovementScript.waypoints = movementScript.waypoints;
                newFrogMovementScript.destinationCount = movementScript.destinationCount;
            }

            frogScript.stopImmediateKill = false;
            frogScript.TakeDamage(999);
        }
    }

}
