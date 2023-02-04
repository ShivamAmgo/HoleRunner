using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class HoleManager : MonoBehaviour
{
    
    

    public delegate void IncreasePlayerSize(float Size);

    public delegate void WinStatus(bool WinStatus);

    public static event WinStatus OnWin;
    public static event IncreasePlayerSize OnPlayerScaling;
    public static HoleManager Instance { get; private set; }
    private Transform Player;
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
        Damage.OnDead += CheckDead;
        PlayerMovement.DeliverPlayerInfo += Receiveplayer;
    }

   

    private void OnDisable()
    {
        
        Damage.OnDead -= CheckDead;
        PlayerMovement.DeliverPlayerInfo -= Receiveplayer;
    }

    private void CheckDead(Transform deadguy)
    {
        Debug.Log("dead Guy "+deadguy);
        if (deadguy==Player)
        {
            OnWin?.Invoke(false);
        }
        else if (deadguy.transform.CompareTag("Boss"))
        {
            Debug.Log("dead boss "+deadguy);
            OnWin?.Invoke(true);
        }
    }
    private void Receiveplayer(PlayerMovement player)
    {
        Player = player.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
