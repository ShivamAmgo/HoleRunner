using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ShootPoint;
    private List<Transform> CollectedItems = new List<Transform>();
    private bool Shooting = false;

    private void OnEnable()
    {
        FinishLine.OnFInishLineCrossed += Shoot;
    }

    private void OnDisable()
    {
        FinishLine.OnFInishLineCrossed -= Shoot;
    }

    private void FixedUpdate()
    {
        if (Shooting)
        {
            
        }
    }

    private void Shoot()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroyer d = other.transform.GetComponentInParent<Destroyer>();
        if (d!=null)
        {
            d.DestroyIt();
            CollectedItems.Add(d.transform);
        }
    }
    
}
