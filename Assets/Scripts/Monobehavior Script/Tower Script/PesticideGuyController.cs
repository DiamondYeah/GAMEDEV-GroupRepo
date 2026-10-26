using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] 
    private float gizmoRange;

    private TowerController controller;
    private TowerStats towerStats;

    private float attackCheck = 0f;

    private List<Collider> hitColliders = new List<Collider>();
    private GameObject targetedFrog = null;
    private FrogController targetedFrogScript = null;

    private void Awake()
    {

    }
    // Provides visual of draw sphere in the scene
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, gizmoRange);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<TowerController>();
        towerStats = controller.statsHolder;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= attackCheck && towerStats.canBeDamaged)
        {
            attackCheck = Time.time + towerStats.attackDelay;
            AttackFrog();
        }
    }

    private void AttackFrog()
    {
        if (targetedFrog == null || targetedFrogScript == null)
        {
            targetedFrog = null;
            targetedFrogScript = null;
        }

        // Get all colliders within sphere
        hitColliders = new List<Collider>(Physics.OverlapSphere(transform.position, towerStats.range, towerStats.validAttackMasks));

        if (hitColliders.Count > 0)
        {
            // If targeted frog is still within range, keep damaging them
            if (targetedFrog != null && targetedFrogScript != null && hitColliders.Contains(targetedFrog.GetComponent<Collider>()))
            {
                transform.LookAt(targetedFrog.transform.position);
                targetedFrogScript.TakeDamage(towerStats.damage);
                return;
            }

            // Obtain frog to target and deal tamage to them
            targetedFrog = hitColliders[0].gameObject;
            targetedFrogScript = targetedFrog.GetComponent<FrogController>();
            transform.LookAt(targetedFrog.transform.position);
            targetedFrogScript.TakeDamage(towerStats.damage);
            return;
        }
        else
        {
            // Set target to null since there's no more frogs to target
            targetedFrog = null;
            targetedFrogScript = null;
        }

    }
}
