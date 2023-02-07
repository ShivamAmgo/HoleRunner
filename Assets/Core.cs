using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] private Collider GroundCollider;
    [SerializeField] private Collider CoreCollider;
    
    private void OnEnable()
    {
        FinishLine.OnFInishLineCrossed += OnFinishLine;
    }

   
    private void OnDisable()
    {
        FinishLine.OnFInishLineCrossed -= OnFinishLine;
    }
    private void OnFinishLine()
    {
        CoreCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible") )
        {
            other.transform.GetComponentInParent<Collectibles>().CollectedItem(false,GroundCollider);
         
        }

        if (other.transform.root.CompareTag("Enemy"))
        {
            other.transform.root.GetComponent<EnemyAI>().Dead(GroundCollider);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectible") )
        {
            other.transform.GetComponentInParent<Collectibles>().CollectedItem(true,GroundCollider);
        }
        if (other.transform.root.CompareTag("Enemy"))
        {
            Debug.Log("Exited");
            other.transform.root.GetComponent<EnemyAI>().Floored(GroundCollider);
        }
    }
}
