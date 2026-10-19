using System;
using System.Collections;
using UnityEngine;

using UnityEngine.InputSystem;
public class Kick : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float raySize;
    [SerializeField] float kickForce;
    [SerializeField] int kickDmg;
    [SerializeField] int kickCooldown;

    bool canKick = true;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && canKick)
        {
            StartCoroutine(StartKickCoolDown());

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitinfo, raySize))
            {
                Enemy enemy = hitinfo.transform.gameObject.GetComponent<Enemy>();
                Rigidbody enemyrb = hitinfo.transform.gameObject.GetComponent<Rigidbody>();
                if (enemy != null)
                {
                    enemyrb.AddForce(transform.forward*kickForce, ForceMode.Impulse);
                    enemy.TakeDamage(kickDmg);
                   
                    

                }
            }

        }

    }
    IEnumerator StartKickCoolDown()
    {
        canKick = false;
        Debug.Log("On CD!");
        yield return new WaitForSeconds(kickCooldown);
        canKick = true;
        yield return null;

    }
}
