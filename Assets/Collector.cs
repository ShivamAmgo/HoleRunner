using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ShootPoint;
    [SerializeField]float AttackRate=1;
    [SerializeField] private GameObject ShootFX;
    [SerializeField] private GameObject[] CollectFX;
    [SerializeField] private Collider _collider;
    [SerializeField] private GameObject Fillbar;
    [SerializeField] private GameObject ScoreText;
    [SerializeField] private AudioClip CollectSFX;
    [SerializeField] private AudioClip ShootSFX;
    private List<Transform> CollectedItems = new List<Transform>();
    [SerializeField]GameObject SuckFX;
    private bool Shooting = false;
    private bool Collectable = true;
    private Tween SHootingTween;
    private Touch _touch;
    private bool IsEditor = false;
    private void OnEnable()
    {
        FinishLine.OnFInishLineCrossed += Shootable;
        HoleManager.OnWin += CheckWin;

    }

    private void OnDisable()
    {
        FinishLine.OnFInishLineCrossed -= Shootable;
        HoleManager.OnWin -= CheckWin;
    }

    private void Start()
    {
        if (Application.isEditor)
        {
            IsEditor = true;
        }
        ScoreText.SetActive(true);
        Fillbar.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!Shooting)
        {
            return;
        }
        /*
        if (Input.GetMouseButton(0))
        {
            Shooting = false;
            Shoot();
        }
        */
        if (IsEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shooting = false;
                Shoot();
            }
            return;
        }
        _touch = Input.GetTouch(0);
        if (_touch.tapCount>0)
        {
            Shooting = false;
            Shoot();
        }
    }
    private void CheckWin(bool winstatus)
    {
        if (!winstatus)
        {
            
        }

        SHootingTween.Kill();
        Shooting = false;
    }

    void Shootable()
    {
        Shooting = true;
        Collectable = false;
        _collider.enabled = false;
        ScoreText.SetActive(false);
        Fillbar.SetActive(true);
        SuckFX.SetActive(false);
    }
    private void Shoot()
    {
        if (CollectedItems.Count<1)
        {
            return;
        }
        AudioManager.Instance.PlaySound("Player",ShootSFX);
        ShootFX.SetActive(false);
        ShootFX.SetActive(true);
        SHootingTween= DOVirtual.DelayedCall(AttackRate, () =>
        {
            Shooting = true;
        });
        CollectedItems[0].GetComponent<Projectile>().Launch(ShootPoint);
        
        //Debug.Log("Items "+CollectedItems.Count+" "+CollectedItems[0].name);
        CollectedItems.Remove(CollectedItems[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Collectable)
        {
            return;
        }
        Destroyer d = other.transform.GetComponentInParent<Destroyer>();
        if (d!=null)
        {
            d.DestroyIt();
            CollectedItems.Add(d.transform);
            PlayFx();
        }
        AudioManager.Instance.PlaySound("Player",CollectSFX);
    }

    void PlayFx()
    {
        foreach (GameObject obj in CollectFX)
        {
            obj.SetActive(false);
            obj.SetActive(true);
        }
    }
    
}
