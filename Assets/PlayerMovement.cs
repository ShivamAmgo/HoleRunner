using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> RigRigidbodies;
    [SerializeField] public float Speed=3;
    [SerializeField] private float StrafeSpeed = 3;

    [SerializeField] private float RotateSpeed = 10;
    
    [SerializeField]private Animator PlayerAnim;
    public bool isMoving = false;
    private Touch Xmove;
    public float posX = 0;
    public float YAxisMove = 0;
   
    private Vector3 Offset = new Vector3(0, 0.6f, 0);

    [SerializeField] private float PlayerClampXPos = 5.5f;
    [SerializeField] private Joystick JoystickController;
    Vector3 ClampPlayerPosX=Vector3.zero;
    private bool IsDead = false;
    public bool UseJoystick = true;
    public delegate void PlayerInfoRaise(PlayerMovement Player);

    public static event PlayerInfoRaise DeliverPlayerInfo;


    private void Awake()
    {
        DOTween.KillAll();
    }

    private void Start()
    {
        //PlayerAnim = GetComponent<Animator>();
        //RagdollActive(false);
        
        Physics.gravity = new Vector3(0, -9.8f*5f, 0);
        //PlayerAnim.SetTrigger("Run");
        
        DeliverPlayerInfo?.Invoke(this);
       
    }
    
/*
    private void OnEnable()
    {
        PlayerDamage.OnPlayerDead += OnPlayerDead;
        LevelComplete.OnLevelComplete += OnLevelFinish;
    }

    private void OnDisable()
    {
        PlayerDamage.OnPlayerDead -= OnPlayerDead;
        LevelComplete.OnLevelComplete -= OnLevelFinish;
    }
*/
    
    private void FixedUpdate()
    {
        DOMovement();
    }

    public void OnScaleChange(float ScaleVal)
    {
        Debug.Log("scale val "+ScaleVal);
        if (ScaleVal<0)
        {
            PlayerClampXPos -= (ScaleVal*2);
        }
        if(ScaleVal>0)
        {
            PlayerClampXPos -= (ScaleVal*2);
        }
        
    }
    private void OnPlayerDead(Transform player)
    {
        IsDead = true;
        //AlliesMovement.Instance.SetState(AllieState.Idle);
        
    }

    private void OnLevelFinish(Transform t)
    {
        //AlliesMovement.Instance.SetState(AllieState.Idle);
        PlayerAnim.SetTrigger("Win");
        //GameManagerTelekenisis.Instance.Win(true);
    }
    public void DOMovement()
    {
        if (IsDead)
        {
            return;
        }
        if (Application.isEditor)
        {
            posX = JoystickController.Horizontal;
            YAxisMove = JoystickController.Vertical;
            
            
            ClampPlayerPos();
        }
        else
        {
            Xmove = Input.GetTouch(0);
            ClampPlayerPos();
            if (Xmove.phase==TouchPhase.Moved)
            {
                posX = JoystickController.Horizontal;
                YAxisMove = JoystickController.Vertical;

            }
            else if(Xmove.phase==TouchPhase.Ended)
            {
                posX = 0;
                YAxisMove = 0;

            }
           
        }
        //PlayerAnim.SetFloat("Xmove",posX);
        //PlayerAnim.SetFloat("Ymove",YAxisMove);
        Move(posX);
    }
  
    void RotatePlayer()
    {
        posX = Mathf.Clamp(posX, -1, 1);
        //transform.rotation=Quaternion.Euler(0,posX*RotateSpeed*Time.deltaTime,0);
        transform.rotation=Quaternion.AngleAxis(posX*RotateSpeed*Time.deltaTime,Vector3.up);
       
    }
  
    void ClampPlayerPos()
    {
        ClampPlayerPosX = transform.position;
        ClampPlayerPosX.x = Mathf.Clamp(ClampPlayerPosX.x, -PlayerClampXPos, PlayerClampXPos);
        transform.position = ClampPlayerPosX;
    }
    void Move(float XAxismove)
    {
        //Debug.Log("Move");
        Vector3 playerpos = transform.position;
        playerpos.x += XAxismove;
        //transform.position = playerpos;
        if (UseJoystick)
        {
            transform.position += (Vector3.right*XAxismove*Time.deltaTime*StrafeSpeed+Vector3.forward*Time.deltaTime*Speed*YAxisMove);
        }
        else
        {
            transform.position += (Vector3.right*XAxismove*Time.deltaTime*StrafeSpeed+Vector3.forward*Time.deltaTime*Speed);
        }
        
       
    }

    
    

    /*
    public void RagdollActive(bool activestatus)
    {
        if (activestatus)
        {
            PlayerAnim.SetTrigger("Dead");
            return;
            //StartCoroutine(ragdollactivedelayed(activestatus));
            
        }
        ragdollactivedelayed(activestatus);

    }

    public  void ragdollactivedelayed(bool activestatus)
    {
        //yield return new WaitForSeconds(waitSeconds);
        PlayerAnim.enabled = !activestatus;
        foreach (Rigidbody rb in RigRigidbodies)
        {
            rb.isKinematic = !activestatus;
            //rb.AddForce(Vector3.forward*5,ForceMode.Impulse);
        }
    }
    */
   

    
}
