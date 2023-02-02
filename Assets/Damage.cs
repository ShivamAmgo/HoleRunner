using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    
    [SerializeField] private float MaxHealth = 100;
    private float currenthealth = 0;

    public delegate void Die(Transform Deadguy);

    public static event Die OnDead;
    private bool IsDead = false;
    
    private void Start()
    {
        currenthealth = MaxHealth;
    }

    public void DamageIt(float damage)
    {
        if (IsDead)
        {
            return;
        }
        currenthealth -= damage;
        if (currenthealth<=0)
        {
            IsDead = true;
            Dead();
        }
        Debug.Log(transform+" Health "+currenthealth);
    }

    void Dead()
    {
        OnDead?.Invoke(transform.root);
    }
}
