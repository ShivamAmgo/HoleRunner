using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
   public delegate void OnEnemyDetection(Transform T);

   public static event OnEnemyDetection OnEnemyDetected;
   private void OnTriggerEnter(Collider other)
   {
      EnemyAI Enemey = other.transform.GetComponentInParent<EnemyAI>();
      if (Enemey!=null)
      {
         //Debug.Log("Entered "+other.name);
         OnEnemyDetected?.Invoke(Enemey.transform);
      }
   }
}
