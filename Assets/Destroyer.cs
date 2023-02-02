using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] bool Enemy = false;
    [SerializeField] private float Points = 10;
    [SerializeField] private EnemyAI enemeyAI;
    private bool Used = false;
    

   

    public void DestroyIt()
    {
        if (Used)
        {
            return;
        }

        Used = true;
        PlayerScaler.Instance.AddScore(Points);
        if (enemeyAI!=null)
        {
            enemeyAI.KillTWeens();
        }
        gameObject.SetActive(false);
    }
}