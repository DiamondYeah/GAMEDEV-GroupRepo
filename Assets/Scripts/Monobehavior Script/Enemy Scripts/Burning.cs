using Unity.VisualScripting;
using UnityEngine;

public class Burning : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float BurnTimer;
    private float TickTimer;
    [SerializeField] FrogController frogControllerScript;
    [SerializeField] float BurnDamage;
    [SerializeField] float tickInterval;
    [SerializeField] GameObject fire;

    void Start()
    {
      fire.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (BurnTimer > 0)
            
        {
            Debug.Log(BurnTimer);
            fire.SetActive(true);
            BurnTimer -= Time.deltaTime;
            TickTimer -= Time.deltaTime;
            
            if (TickTimer <= 0)
            {
                TickTimer += tickInterval;
                frogControllerScript.TakeDamage(5);
            }

            

        }
        if (BurnTimer <= 0)
        {
            fire.SetActive(false);
        }
    }
    public void SetBurnTimer(float timer)
    {
        BurnTimer = timer;
    }
}
