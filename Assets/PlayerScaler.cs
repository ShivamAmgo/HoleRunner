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
    [SerializeField] private GameObject DeathFX;
    
    [SerializeField] private AudioClip ScaleUpSFX;
    private float ScalePoints=0;
    private Vector3 MagnetScaleAtStart;
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
        HoleManager.OnWin += OnWinCheck;
        ButtonCloser.OnPowerSelect += PowerUp;
    }

    private void OnDisable()
    {
        Collectibles.OnCollectedItem -= AddScore;
        HoleManager.OnWin -= OnWinCheck;
        ButtonCloser.OnPowerSelect -= PowerUp;
    }

    

    private void Start()
    {
        MagnetScaleAtStart = m_Magnet.localScale;
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
                m_Magnet.localScale = MagnetScaleAtStart;
                return;

            }

            TargetScaleObject.DOScale(NewScale, 0.25f);
            NewScale = m_Magnet.localScale;
            NewScale.x += ScaleIncreaseFactor * SFactor;
            NewScale.z += ScaleIncreaseFactor * SFactor;
            NewScale.y = 0.1f;
            m_Magnet.localScale = NewScale;
            ScalePoints = 0;
           
            m_PlayerMovement.OnScaleChange(ScaleIncreaseFactor*SFactor);
            AudioManager.Instance.PlaySound("Prop",ScaleUpSFX);
        }
        
    }
    private void OnWinCheck(bool winstatus)
    {
        if (winstatus)
        {
            ScoreManager.Instance.CalculateCoins(TotalScore);
            return;
        }
        Vector3 NewScale = TargetScaleObject.localScale;
        NewScale.x = 0;
        NewScale.z = 0;
        NewScale.y = TargetScaleObject.localScale.y;
        TargetScaleObject.DOScale(NewScale, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            DeathFX.SetActive(false);
            DeathFX.SetActive(true);
        });
    }

    public void PowerUp(string Powername,float PowerValue)
    {
        if (Powername=="Hole")
        {
            IncreaseSizeByScore(ScoreFactor*PowerValue);
        }
        else if (Powername=="Magnet")
        {
            m_Magnet.localScale *= PowerValue;
        }
        AudioManager.Instance.PlaySound("Prop",ScaleUpSFX);
    }
}