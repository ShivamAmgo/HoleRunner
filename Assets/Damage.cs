using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    
    [SerializeField] private float MaxHealth = 100;
    [SerializeField] private Image FillBar;
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
        
        if (transform.tag=="Boss")
        {
            float health;
            health = (SceneManager.GetActiveScene().buildIndex)*20;
            MaxHealth += health;
        }
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
            currenthealth = 0;
        }

        if (transform.tag=="Player")
        {
            CameraFollow.Instance.CameraShake();
        }

        if (FillBar!=null)
        {
            FillBar.fillAmount = 1-(1 - (currenthealth / MaxHealth));
        }
//        Debug.Log(transform+" Health "+currenthealth);
    }

    void Dead()
    {
        OnDead?.Invoke(transform.root);
    }
}
