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
    private bool damagable = true;

    private void OnEnable()
    {
        HoleManager.OnWin += CheckOnWin;
    }

    private void OnDisable()
    {
        HoleManager.OnWin-= CheckOnWin;
    }

    private void CheckOnWin(bool winstatus)
    {
        damagable = false;
    }

    private void Start()
    {
        currenthealth = MaxHealth;
    }

    public void DamageIt(float damage)
    {
        if (IsDead || !damagable)
        {
            return;
        }
        currenthealth -= damage;
        if (currenthealth<=0)
        {
            IsDead = true;
            Dead();
        }

        if (transform.tag=="Player")
        {
            CameraFollow.Instance.CameraShake();
        }
//        Debug.Log(transform+" Health "+currenthealth);
    }

    void Dead()
    {
        OnDead?.Invoke(transform.root);
    }
}
