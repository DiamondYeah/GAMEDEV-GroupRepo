using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    public Transform flyEatArea;

    [HideInInspector]
    public bool isSelected { set; get; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSelected = false;
    }

    // Method destroys fly when consumed
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
