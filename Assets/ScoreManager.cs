using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float CoinsScoreFactor = 3;
    [SerializeField] private TextMeshProUGUI CoinsText;
    [SerializeField] private TextMeshProUGUI CurrentCollectedCoinsText;
    [SerializeField] private GameObject ScoringPanel;
    private float StoredCoins;
    private float Coins = 0;
    public static ScoreManager Instance { get; private set; }

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

        //Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            Coins = PlayerPrefs.GetFloat("Coins");
        }
        else
        {
            PlayerPrefs.SetFloat("Coins",0);
        }
        ScoringPanel.SetActive(false);
    }

    public void CalculateCoins(float ScorePoints)
    {
        float CoinsCollected = (ScorePoints / CoinsScoreFactor);
        Coins += CoinsCollected;
        CoinsText.text = (int)Coins + "";
        CurrentCollectedCoinsText.text = "+"+(int)CoinsCollected + "";
        
            PlayerPrefs.SetFloat("Coins",Coins);
        
        ScoringPanel.SetActive(true);
        
    }
    
}