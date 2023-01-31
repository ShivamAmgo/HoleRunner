using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] bool Enemy = false;
    private bool Used = false;
    public void DestroyIt()
    {
        if (Used)
        {
            return;
        }

        Used = true;
        PlayerScaler.Instance.AddScore(10);
        Destroy(gameObject);
    }
}