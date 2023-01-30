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
    float ChaosRotation=0;
    private bool Flee = false;
    private Tween YOYO;
    private bool IsDead = false;
    private Vector3 StartPos;

    private void OnEnable()
    {
        KillZone.OnEnemyDetected += OnKillZoneDetected;
    }

    private void OnDisable()
    {
        KillZone.OnEnemyDetected -= OnKillZoneDetected;
    }

    private void Start()
    {
        StartPos = transform.position;
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
        transform.DORotate(new Vector3(0,ChaosRotation,0), Rotationduration).SetEase(Ease.Linear).OnComplete(() =>
        {
           YOYO= DOTween.To(() => ChaosRotation, value => ChaosRotation = value, randomval, Rotationduration)
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    Debug.Log("RandomVal " + randomval + " chaosval " + ChaosRotation);
                }).OnUpdate(
                    () =>
                    {
                        transform.eulerAngles = new Vector3(0, ChaosRotation, 0);
                    });
        });
        
        m_animator.SetTrigger("Run");
        Debug.Log("rot va "+ChaosRotation);
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
        YOYO.Kill();
        RagdollActive(true);
        SetCollision(false,Col);
        
    }

    public void Floored(Collider GroundCol)
    {
        

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
        Collider[] col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col)
        {
            //transform.position
            c.enabled = true;
            transform.position = new Vector3(transform.position.x, StartPos.y, transform.position.z);
            Physics.IgnoreCollision(GroundCol,c,status);
            //c.enabled = status;
        }
    }
}
