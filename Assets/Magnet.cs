using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private GameObject MagnetFx;
    [SerializeField] private Collider MagnetCollider;
    [SerializeField] private float MagnetDuration = 3;

    public delegate void Magnetism(bool Activestatus);

    public static event Magnetism OnMagnetActive;
    private bool MAgnetActive = false;
    private void Start()
    {
        MagnetCollider.enabled = false;
        MagnetFx.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ActivateMagnet();
        }
    }
    public void ActivateMagnet()
    {
        if (MAgnetActive)
        {
            return;
        }

        MAgnetActive = true;
        MagnetCollider.enabled = true;
        DOVirtual.DelayedCall(MagnetDuration, () =>
        {
            MagnetCollider.enabled = false;
            OnMagnetActive?.Invoke(false);
            MAgnetActive = false;
        });
        PlayMagnetFX();
    }
    
    void PlayMagnetFX()
    {
        MagnetFx.SetActive(false);
        MagnetFx.SetActive(true);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
