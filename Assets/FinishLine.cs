using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject[] WinFX;
    [SerializeField] private GameObject[] DisableObjectsOnWIn;
    [SerializeField] private Transform BossPlatform;

    public delegate void OnFInishLine();

    public static event OnFInishLine OnFInishLineCrossed;
    private bool Used = false;

    private void OnEnable()
    {
        HoleManager.OnWin += OnWin;
        FinishLine.OnFInishLineCrossed += OnfinishLineCrossed;
    }

    private void OnDisable()
    {
        HoleManager.OnWin -= OnWin;
        FinishLine.OnFInishLineCrossed -= OnfinishLineCrossed;
    }

    private void Start()
    {
        Vector3 tempPos = BossPlatform.position;
        tempPos.y = 0;
        BossPlatform.position = tempPos;
    }

    private void OnfinishLineCrossed()
    {
        DisableObjects();
    }

    private void OnWin(bool winstatus)
    {
        if (winstatus)
        {
            PlayWinFX();
            
        }
    }

    void PlayWinFX()
    {
        foreach (GameObject obj in WinFX)
        {
            obj.SetActive(false);
            obj.SetActive(true);
        }
    }

    void DisableObjects()
    {
        foreach (GameObject obj in DisableObjectsOnWIn)
        {
            obj.SetActive(false);
        }
    }
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