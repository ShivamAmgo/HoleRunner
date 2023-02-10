using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCloser : MonoBehaviour
{
    [SerializeField] private GameObject[] ObjectsToDisable;

    [SerializeField] string PowerName = "Hole";
    [SerializeField]float PowerValue=1.5f;

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
        
        OnPowerSelect?.Invoke(PowerName,PowerValue);
        DisableObjects();
        
    }
}