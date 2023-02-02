using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public delegate void OnFInishLine();

    public static event OnFInishLine OnFInishLineCrossed;
    private bool Used = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Used)
        {
            Used = true;
            OnFInishLineCrossed?.Invoke();
            Debug.Log("crossed");
        }
    }
}
