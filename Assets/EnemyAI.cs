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
    [SerializeField] private float GroundOffset = 0.04f;
    [SerializeField] private float RunSpeed = 13;

    [SerializeField] private Vector3 FreeFallTorque;

    [SerializeField] private bool Sitting = false;
    [SerializeField] Color DeathColor = Color.red;
    private Collider GroundCollider;

    [SerializeField] private SkinnedMeshRenderer m_Renderer;

    //[SerializeField] private CapsuleCollider _capsuleCollider;
    float ChaosRotation = 0;
    private bool Flee = false;
    private Tween YOYO;
    private Tween FreeFall;
    private bool IsDead = false;
    private bool IsFlying = false;
    private Vector3 StartPos;
    private Transform Player;
    private Material[] m_materials;
    private Collider[] col;
    private Collider bosscol;
    private bool BossHit = false;

    private void OnEnable()
    {
        KillZone.OnEnemyDetected += OnKillZoneDetected;
        PlayerMovement.DeliverPlayerInfo += ReceivePlayer;
        FinishLine.OnFInishLineCrossed += OnLineCrossed;
        //BossAi.OnShotHit += BossHitted;
    }


    private void OnDisable()
    {
        KillZone.OnEnemyDetected -= OnKillZoneDetected;
        PlayerMovement.DeliverPlayerInfo -= ReceivePlayer;
        FinishLine.OnFInishLineCrossed -= OnLineCrossed;
        //BossAi.OnShotHit -= BossHitted;
    }


    private void Start()
    {
        StartPos = transform.position;
        m_materials = m_Renderer.materials;
        if (Sitting)
        {
            m_animator.Play("Sitting");
        }
        else
        {
            StartPos.y = GroundOffset;
            transform.position = StartPos;
        }

        bosscol = BossAi.Instance.GetCollider();
    }

    private void FixedUpdate()
    {
        if (Flee)
        {
            transform.position += transform.forward * RunSpeed * Time.deltaTime;
        }
    }

    private void OnLineCrossed()
    {
        gameObject.SetActive(false);
    }

    public void BossHitted()
    {
        BossHit = true;
        RagdollActive(true);
        SetCollision(true, GroundCollider);
    }

    private void ReceivePlayer(PlayerMovement player)
    {
        Player = player.transform;
    }

    private void OnKillZoneDetected(Transform t)
    {
        if (t != transform && !Flee)
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
        transform.position = new Vector3(transform.position.x, GroundOffset, transform.position.z);

        if (IsFlying)
        {
            return;
        }

        YOYO = DOTween.To(() => ChaosRotation, value => ChaosRotation = value, randomval, Rotationduration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear)
            .OnStart(() =>
            {
                //Debug.Log("RandomVal " + randomval + " chaosval " + ChaosRotation);
            }).OnUpdate(
                () => { transform.eulerAngles = new Vector3(0, ChaosRotation, 0); });


        m_animator.SetTrigger("Run");
//        Debug.Log("rot va "+ChaosRotation);
    }

    public void RagdollActive(bool activestatus)
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


        // ragdollactivedelayed(activestatus);
    }

    public void Fly()
    {
        IsFlying = true;
        KillTWeens();
        RagdollActive(false);
        SetCollision(true, GroundCollider);
    }

    public void Dead(Collider Col)
    {
        GroundCollider = Col;
        //Debug.Log("Colname "+GroundCollider.transform.name);
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

        ChangeColor();
        FreeFall = transform.DOLocalMove(Vector3.zero, 0.6f).SetEase(Ease.Linear).OnStart(() => { RandomFallforce(); });
        RagdollActive(true);
        SetCollision(false, Col);
        transform.parent = Player;
    }

    void SetBossCollision(Collider BossCol)
    {
        foreach (Collider c in col)
        {
            Physics.IgnoreCollision(BossCol, c, true);
        }
    }

    void ChangeColor()
    {
        foreach (Material m in m_materials)
        {
            //m.color = DeathColor;
            m.DOColor(DeathColor, 0.35f).SetEase(Ease.Linear);
        }
    }

    void RandomFallforce()
    {
        foreach (Rigidbody rb in RigRigidbodies)
        {
            rb.AddTorque(new Vector3(Random.Range(-FreeFallTorque.x, FreeFallTorque.x),
                Random.Range(-FreeFallTorque.y, FreeFallTorque.y),
                Random.Range(-FreeFallTorque.z, FreeFallTorque.z)), ForceMode.Impulse);
        }
    }

    public void Floored(Collider GroundCol)
    {
        //RagdollActive(true);
        return;
        //IsDead = true;
        if (YOYO.IsActive())
        {
            YOYO.Kill();
        }

        m_animator.enabled = false;

        SetCollision(true, GroundCol);
    }

    public void SetCollision(bool status, Collider GroundCol)
    {
//        Debug.Log(transform.name+" Collision "+ status);
        col = GetComponentsInChildren<Collider>();
        foreach (Collider c in col)
        {
            //transform.position
            c.enabled = true;
            if (!IsFlying)
            {
                transform.position = new Vector3(transform.position.x, StartPos.y, transform.position.z);
            }

            if (GroundCollider != null)
            {
                Physics.IgnoreCollision(GroundCollider, c, !status);
                //Debug.Log("Col Setted"+Physics.GetIgnoreCollision(GroundCollider,c));
            }

            if (BossHit)
            {
                Physics.IgnoreCollision(bosscol, c, true);
            }

            //c.enabled = status;
        }
        //_capsuleCollider.enabled = true;
    }

    public void KillTWeens()
    {
        FreeFall.Kill();
        YOYO.Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish") && !IsFlying)
        {
            gameObject.SetActive(false);
        }
    }
}