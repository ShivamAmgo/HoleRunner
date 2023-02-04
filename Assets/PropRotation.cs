using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PropRotation : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDisable;
    [SerializeField] private GameObject CollectFX;
    private float Timer = 0;
    private bool Used = false;

    public delegate void MagnetCollect();

    public static event MagnetCollect OnMagnetCollect;
    private void FixedUpdate()
    {
        
        Timer += Time.deltaTime * 100;
        transform.eulerAngles = new Vector3(0, Timer, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Used)
        {
            Used = true;
            Collect();
            OnMagnetCollect?.Invoke();
        }
    }

    void Collect()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
        CollectFX.SetActive(false);
        CollectFX.SetActive(true);
        DOVirtual.DelayedCall(1, () =>
        {
            Destroy(gameObject);
        });
    }
}
