using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloser : MonoBehaviour
{
    [SerializeField] private GameObject[] ObjectsToDisable;

    [SerializeField] string PowerName = "Hole";
    [SerializeField]float PowerValue=1.5f;
    [SerializeField] private float Cost = 150;
    [SerializeField] private bool Testing = true;
    public delegate void PowerUps(string PowerName, float PowerValue);

    public static event PowerUps OnPowerSelect;

    private void OnEnable()
    {
        HoleManager.OnROundStart += RoundStarted;
    }

    private void OnDisable()
    {
        HoleManager.OnROundStart -= RoundStarted;
    }

    private void RoundStarted()
    {
        DisableObjects();
    }

    void DisableObjects()
    {
        foreach (GameObject obj in ObjectsToDisable)
        {
            obj.SetActive(false);
        }
    }

    public void SetPowerUp()
    {
        
        float TempCoins = 0;
        if (PlayerPrefs.HasKey("Coins"))
        {
          TempCoins   = PlayerPrefs.GetFloat("Coins");
        }
        Debug.Log("setting "+TempCoins);
        if (TempCoins>=Cost)
        {
            Debug.Log( Cost+ " Deducted from "+ TempCoins+" Remains : "+(TempCoins-Cost));
            if (!Testing)
            {
                PlayerPrefs.SetFloat("Coins",(TempCoins-Cost));
            }
            
            OnPowerSelect?.Invoke(PowerName,PowerValue);//Targeting PlayerScaler Script
        }

        
        DisableObjects();
        
    }
}