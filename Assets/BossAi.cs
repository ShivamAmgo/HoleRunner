using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAi : MonoBehaviour
{
   [SerializeField] private Animator BossAnim;
   [SerializeField] private float FallDuration=2;
   [SerializeField] private float GroundOffset = 0.05f;
   [SerializeField] private float WalkDuration = 8;
   [SerializeField] private Transform Weapon;
   [SerializeField] private Vector3 Weaponpos;
   [SerializeField] private Vector3 WeaponRot;
   [SerializeField] private Transform WeaponHand;
   [SerializeField] private float AttackRate = 3;
   [SerializeField] private Collider[] Weapons;
  
   
   private Tween JumpTween;
   private Tween Walktween;
   private bool IsGrounded = false;

   public delegate void OnHit(Transform HitObj);

   public static event OnHit OnShotHit;
   public static BossAi Instance { get; private set; }
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
      FinishLine.OnFInishLineCrossed += ActivateBoss;
      Damage.OnDead +=CheckDeadStatus;
   }

   

   private void OnDisable()
   {
      FinishLine.OnFInishLineCrossed -= ActivateBoss;
      Damage.OnDead -=CheckDeadStatus;
   }
   
   void ActivateBoss()
   {
      JumpDown();
   }

   void JumpDown()
   {
      
      JumpTween = transform.DOMoveY(0+GroundOffset, FallDuration).SetEase(Ease.Linear);
   }
   void Land()
   {
      BossAnim.SetTrigger("Land");
   }
   private void CheckDeadStatus(Transform deadguy)
   {
      if (deadguy!=transform)
      {
         /*
         if (deadguy.GetComponent<PlayerMovement>()!=null)
         {
            
         }
         */
         
         return;
      }
      BossAnim.SetTrigger("Dead");
      DOTween.Kill(gameObject);
      this.enabled = false;

   }
   void CombatMode()
   {
      BossAnim.SetTrigger("Idle");
      DOVirtual.DelayedCall(0.25f, () =>
      {
         
      }).OnStart(() =>
      {
         BossAnim.SetTrigger("Lowercut");
      }).OnComplete(() =>
      {
         DOVirtual.DelayedCall(AttackRate, () =>
         {
            int random = Random.Range(1, 9);
            if (random % 2 == 0)
            {
               BossAnim.SetTrigger("Lowercut");
            }
            else
            {
               BossAnim.SetTrigger("Uppercut");
            }
         }).SetLoops(-1);
      });
      
   }

   public void Chase()
   {
      Debug.Log("Chasing");
      Walktween = transform.DOMoveZ(transform.position.z-50, WalkDuration).SetEase(Ease.Linear);
   }

   public void EquipWeapon()
   {
      Weapon.SetParent(WeaponHand);
      Weapon.localPosition = Weaponpos;
      Weapon.localEulerAngles = WeaponRot;
   }

   public void ActivateWeapon()
   {
      foreach (Collider c in Weapons)
      {
         c.enabled = true;
      }
   }
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Ground") && !IsGrounded)
      {
         IsGrounded = true;
         
         Land();
      }

      else if (other.CompareTag("Finish"))
      {
         CombatMode();
         Walktween.Kill();
      }
      else if (other.CompareTag("Enemy") || other.CompareTag("Collectible"))
      {
         Debug.Log("enetered "+other.name);
          OnShotHit?.Invoke(other.transform.root);
      }
      
   }
   
}
