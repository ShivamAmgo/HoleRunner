using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaler : MonoBehaviour
{
    float TotalScore = 0;
    [SerializeField] private float ScoreFactor = 5;
    [SerializeField] private float ScaleIncreaseFactor = 1.25f;
    [SerializeField] private float MaxScaleLimit = 2.5f;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private Transform TargetScaleObject;
    [SerializeField] private Transform m_Magnet;
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
        TotalScore += Points;
        IncreaseSizeByScore(TotalScore);
    }

    void IncreaseSizeByScore(float score)
    {
        Vector3 NewScale = TargetScaleObject.localScale;
        if (NewScale.x>=MaxScaleLimit)
        {
            return;
        }
        if (TotalScore%ScoreFactor==0)
        {

            
            NewScale .x+= ScaleIncreaseFactor;
            NewScale.z += ScaleIncreaseFactor;
            NewScale.y = TargetScaleObject.localScale.y;
            TargetScaleObject.localScale = NewScale;
            NewScale = m_Magnet.localScale;
            NewScale .x+= ScaleIncreaseFactor;
            NewScale. z+= ScaleIncreaseFactor;
            NewScale.y = m_Magnet.localScale.y;
            m_Magnet.localScale = NewScale;
            m_PlayerMovement.OnScaleChange(ScaleIncreaseFactor);
        }
    }
}
