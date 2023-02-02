using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float DamageValue = 10;
    [SerializeField] private Collider _collider;

    private void OnTriggerEnter(Collider other)
    {
        Damage d = other.GetComponent<Damage>();
        if (d!=null)
        {
            _collider.enabled=false;
            d.DamageIt(DamageValue);
        }
    }

    public void SetCanDamage()
    {
        _collider.enabled = true;
    }
}
