using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;



public class HoleManager : MonoBehaviour
{


    [SerializeField] private GameObject[] Win_Lose_Panel;
    [SerializeField] private int StartSceneIndex = 0;
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
        Application.targetFrameRate=60;
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
            DOVirtual.DelayedCall(4, ()=>
            {
                Win_Lose_Panel[1].SetActive(true);
            });
        }
        else if (deadguy.transform.CompareTag("Boss"))
        {
            Debug.Log("dead boss "+deadguy);
            OnWin?.Invoke(true);
            DOVirtual.DelayedCall(4, ()=>
            {
                Win_Lose_Panel[0].SetActive(true);
            });
        }
    }
    private void Receiveplayer(PlayerMovement player)
    {
        Player = player.transform;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        /*
        if(GAScript.Instance)
            GAScript.Instance.LevelCompleted(SceneManager.GetActiveScene().name);
            */
        
        int index = SceneManager.GetActiveScene().buildIndex;
        index++;
        
        if (index<=SceneManager.sceneCountInBuildSettings-1)
        {

            SceneManager.LoadScene(index);
        }
        else
        {
            SceneManager.LoadScene(StartSceneIndex);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
