using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HITFx : MonoBehaviour
{
    [SerializeField] private GameObject FX;
    private void OnEnable()
    {
        BossAi.OnShotHit += OnBossHit;
    }

    private void OnDisable()
    {
        BossAi.OnShotHit -= OnBossHit;
    }

    private void OnBossHit(Transform hitobj)
    {
        FX.transform.position = hitobj.position;
        FX.SetActive(false);
        FX.SetActive(true);
    }
}