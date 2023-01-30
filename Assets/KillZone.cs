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
      
      if (other.transform.root.CompareTag("Enemy"))
      {
         //Debug.Log("Entered "+other.name);
         OnEnemyDetected?.Invoke(other.transform.root);
      }
   }
}
