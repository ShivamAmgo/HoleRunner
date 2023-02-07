using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
   [SerializeField] private bool Enemy = false;
   //[SerializeField] private Transform ShootPoint;
   [SerializeField] private Rigidbody RB;
   [SerializeField] private float HitDamage = 10;
   [SerializeField] private Vector3 FreeFallTorque;
   [SerializeField] private Vector3 Force;
   [SerializeField] private EnemyAI _enemyAI;
   [SerializeField] private float MidAIrRotationDuration = 1.5f;
   private Animator m_animator;
   private bool FLy = false;
   [SerializeField] private float FlySpeed = 5;
   [SerializeField] private Vector3 ProjectileRange;
   [SerializeField]Vector3 FlyRot=new Vector3(-60,0,0);
   private bool FinalHit = false;
   private Tween FlyRotTweeen;
   private Tween ragdollTween;
   
   private void OnEnable()
   {
      BossAi.OnShotHit += BossHit;
   }

 

   private void OnDisable()
   {
      BossAi.OnShotHit -= BossHit;
   }

   private void Start()
   {
      if (Enemy)
      {
         m_animator = GetComponent<Animator>();
      }
      
   }

   private void FixedUpdate()
   {
      if (!FLy)
      {
         return;
      }

      transform.position += FlySpeed * Time.deltaTime*transform.forward;
   }
   private void BossHit(Transform hitobj)
   {
      if (hitobj!=transform || FinalHit)
      {
         return;
      }

      FinalHit = true;
      BossAi.Instance.transform.GetComponent<Damage>().DamageIt(HitDamage);
      if (Enemy)
      {
         Debug.Log("Boss HItted");
         FLy = false;
        _enemyAI.BossHitted();
         if (FlyRotTweeen.IsActive())
         {
            FlyRotTweeen.Kill();
         }
         ragdollTween.Kill();
         
      }
      //PlayHitFX(BossAi.Instance.transform);
   }

   
   public void Launch(Transform ShootPoint)
   {
      transform.SetParent(null);
      transform.position = ShootPoint.position;
      transform.gameObject.SetActive(true);
      if (Enemy)
      {
         LaunchEnemy();
         return;
      }
      RB.AddForce(new Vector3(Force.x,Force.y,Force.z),ForceMode.Impulse);
      RB.AddTorque(new Vector3(Random.Range(-FreeFallTorque.x,FreeFallTorque.x),Random.Range(-FreeFallTorque.y,FreeFallTorque.y),
         Random.Range(-FreeFallTorque.z,FreeFallTorque.z)),ForceMode.Impulse);
   }
   
   void LaunchEnemy()
   {
      _enemyAI.Fly();
     // m_animator.applyRootMotion = true;
       FlyRot = new Vector3(-60, 0, 0);
      transform.eulerAngles = FlyRot;
      m_animator.SetTrigger("Fly");
      FLy = true;
      FlyRotTweeen= DOTween.To(() => FlyRot.x, value =>FlyRot.x  = value, -FlyRot.x, MidAIrRotationDuration).SetEase(Ease.Linear)
         .OnUpdate(() =>
         {
            
            transform.eulerAngles = FlyRot;
           
            
         }).OnStart(() =>
         {
            ragdollTween = DOVirtual.DelayedCall(MidAIrRotationDuration - 0.15f, () =>
            {
               _enemyAI.RagdollActive(true);
               _enemyAI.SetCollision(true,null);
            });
         });
      //transform.DOMoveZ(transform.position.z + ProjectileRange.z,MidAIrRotationDuration).SetEase(Ease.Linear);

   }
}
