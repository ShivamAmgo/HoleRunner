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