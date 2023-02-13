using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private GameObject[] MagnetFx;
    [SerializeField] private Collider MagnetCollider;
    [SerializeField] private float MagnetDuration = 3;
    [SerializeField] private AudioClip MagnetSFX;
    public delegate void Magnetism(bool Activestatus);

    public static event Magnetism OnMagnetActive;
    private bool MAgnetActive = false;

    private void OnEnable()
    {
        PropRotation.OnMagnetCollect += MagnetActivation;
    }

    private void OnDisable()
    {
        PropRotation.OnMagnetCollect -= MagnetActivation;
    }

    private void Start()
    {
        MagnetCollider.enabled = false;
        
    }

    private void FixedUpdate()
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
        foreach (GameObject obj in MagnetFx)
        {
            obj.SetActive(false);
            obj.SetActive(true);
        }
        
    }

    public void MagnetActivation()
    {
        ActivateMagnet();
        AudioManager.Instance.PlaySound("Enemy",MagnetSFX);
    }
}
