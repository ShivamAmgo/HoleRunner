using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScaler : MonoBehaviour
{
    float TotalScore = 0;
    [SerializeField] private float ScoreFactor = 5;
    [SerializeField] private float ScaleIncreaseFactor = 1.25f;
    [SerializeField] private float MaxScaleLimit = 2.5f;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private Transform TargetScaleObject;
    [SerializeField] private Transform m_Magnet;
    [SerializeField] private TextMeshPro ScoreText;
    private float ScalePoints=0;
    public static PlayerScaler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Collectibles.OnCollectedItem += AddScore;
    }

    private void OnDisable()
    {
        Collectibles.OnCollectedItem -= AddScore;
    }

    public void AddScore(float Points)
    { 
        ScalePoints += Points;
        TotalScore += Points;
        
        ScoreText.text = TotalScore + "";
        
        IncreaseSizeByScore(ScalePoints);
        //Debug.Log("total Score "+TotalScore);
    }

    void IncreaseSizeByScore(float score)
    {
        Vector3 NewScale = TargetScaleObject.localScale;
        if (NewScale.x >= MaxScaleLimit && score>0)
        {
           // Debug.Log("returned Scale");
            ScalePoints = 0;
            return;
        }
//        Debug.Log("scalePoints "+ScalePoints);
        
        if (Mathf.Abs(score / ScoreFactor) >=1)
        {
            int SFactor = (int)(score / ScoreFactor);
           
            NewScale.x += ScaleIncreaseFactor * SFactor;
            NewScale.z += ScaleIncreaseFactor * SFactor;
            NewScale.y = TargetScaleObject.localScale.y;
            if (NewScale.x<1)
            {
                ScalePoints = 0;
                TargetScaleObject.localScale = new Vector3(1, TargetScaleObject.localScale.y, 1);
                m_Magnet.localScale = new Vector3(1, m_Magnet.localScale.y, 1);
                return;

            }

            TargetScaleObject.DOScale(NewScale, 0.25f);
            NewScale = m_Magnet.localScale;
            NewScale.x += ScaleIncreaseFactor * SFactor;
            NewScale.z += ScaleIncreaseFactor * SFactor;
            NewScale.y = m_Magnet.localScale.y;
            m_Magnet.localScale = NewScale;
            ScalePoints = 0;
           
            m_PlayerMovement.OnScaleChange(ScaleIncreaseFactor*SFactor);
            
        }
        
    }
}