using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private int MinFleeRotation = 10;
    [SerializeField] private int MaxFleeRotation = 30;
    [SerializeField] private float Rotationduration = 0.6f;
    [SerializeField] private List<Rigidbody> RigRigidbodies;

    [SerializeField] private float Speed = 3;

    [SerializeField] private Vector3 FreeFallTorque;
    //[SerializeField] private CapsuleCollider _capsuleCollider;
    float ChaosRotation=0;
    private bool Flee = false;
    private Tween YOYO;
    private Tween FreeFall;
    private bool IsDead = false;
    private Vector3 StartPos;
    private Transform Player;
    private void OnEnable()
    {
        KillZone.OnEnemyDetected += OnKillZoneDetected;
        PlayerMovement.DeliverPlayerInfo += ReceivePlayer;
    }

    private void OnDisable()
    {
        KillZone.OnEnemyDetected -= OnKillZoneDetected;
        PlayerMovement.DeliverPlayerInfo -= ReceivePlayer;
    }

   

    private void Start()
    {
        StartPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (Flee)
        {
            transform.position += transform.forward * Speed*Time.deltaTime;
        }
    }
    private void ReceivePlayer(PlayerMovement player)
    {
        Player = player.transform;
    }

    private void OnKillZoneDetected(Transform t)
    {
        if (t!=transform && !Flee)
        {
            return;
        }

        if (Flee)
        {
            return;
        }
        
        SetChaos();
    }

   
    void SetChaos()
    {
        if (Flee)
        {
            return;
        }
        Flee = true;
        
        float randomval = Random.Range(MinFleeRotation, MaxFleeRotation);
        ChaosRotation = -randomval;
        transform.eulerAngles = Vector3.zero;
        YOYO= DOTween.To(() => ChaosRotation, value => ChaosRotation = value, randomval, Rotationduration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
            .OnStart(() =>
            {
                //Debug.Log("RandomVal " + randomval + " chaosval " + ChaosRotation);
            }).OnUpdate(
                () =>
                {
                    transform.eulerAngles = new Vector3(0, ChaosRotation, 0);
                    
                });
        
        
        m_animator.SetTrigger("Run");
//        Debug.Log("rot va "+ChaosRotation);
    }
    public void RagdollActive(bool activestatus)
    {
        if (activestatus)
        {
            //m_animator.SetTrigger("Dead");
            m_animator.enabled = !activestatus;
            foreach (Rigidbody rb in RigRigidbodies)
            {
                rb.isKinematic = !activestatus;
                //rb.AddForce(Vector3.forward*5,ForceMode.Impulse);
            }
            return;
            //StartCoroutine(ragdollactivedelayed(activestatus));
            
        }
       // ragdollactivedelayed(activestatus);

    }

    public void Dead(Collider Col)
    {
        if (IsDead)
        {
            return;
        }
        
        IsDead = true;
        Flee = false;
        if (YOYO.IsActive())
        {
            YOYO.Kill();
        }
        
        FreeFall=transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(Ease.Linear).OnStart(() =>
        {
            RandomFallforce();
        });
        //RagdollActive(true);
        SetCollision(false,Col);
        transform.parent = Player;
        

    }

    void RandomFallforce()
    {
        foreach (Rigidbody rb in RigRigidbodies)
        {
            rb.AddTorque(new Vector3(Random.Range(-FreeFallTorque.x,FreeFallTorque.x),Random.Range(-FreeFallTorque.y,FreeFallTorque.y),
                Random.Range(-FreeFallTorque.z,FreeFallTorque.z)),ForceMode.Impulse);
        }
    }
    public void Floored(Collider GroundCol)
    {
        
        return;
        //IsDead = true;
        if (YOYO.IsActive())
        {
            YOYO.Kill();
        }
        
        m_animator.enabled = false;
        
        SetCollision(true,GroundCol);
        
    }

    void SetCollision(bool status,Collider GroundCol)
    {
        RagdollActive(true);
        
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col)
        {
            //transform.position
            c.enabled = true;
            transform.position = new Vector3(transform.position.x, StartPos.y, transform.position.z);
            Physics.IgnoreCollision(GroundCol,c,!status);
            //c.enabled = status;
        }
        //_capsuleCollider.enabled = true;
    }

    private void OnDestroy()
    {
        if (FreeFall.IsActive())
        {
            FreeFall.Kill();
        }
    }
}
