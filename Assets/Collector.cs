using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ShootPoint;
    [SerializeField]float AttackRate=1;
    private List<Transform> CollectedItems = new List<Transform>();
    private bool Shooting = false;
    private bool Collectable = true;

    private void OnEnable()
    {
        FinishLine.OnFInishLineCrossed += Shootable;
    }

    private void OnDisable()
    {
        FinishLine.OnFInishLineCrossed -= Shootable;
    }

    private void FixedUpdate()
    {
        if (!Shooting)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Shooting = false;
            Shoot();
        }
    }

    void Shootable()
    {
        Shooting = true;
        Collectable = false;
    }
    private void Shoot()
    {
        if (CollectedItems.Count<1)
        {
            return;
        }

        DOVirtual.DelayedCall(AttackRate, () =>
        {
            Shooting = true;
        });
        CollectedItems[0].GetComponent<Projectile>().Launch(ShootPoint);
        
        Debug.Log("Items "+CollectedItems.Count+" "+CollectedItems[0].name);
        CollectedItems.Remove(CollectedItems[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Collectable)
        {
            return;
        }
        Destroyer d = other.transform.GetComponentInParent<Destroyer>();
        if (d!=null)
        {
            d.DestroyIt();
            CollectedItems.Add(d.transform);
        }
    }
    
}
