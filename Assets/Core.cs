using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    
    [SerializeField] private Collider CoreCollider;
    private Collider GroundCollider;
    private void OnEnable()
    {
        FinishLine.OnFInishLineCrossed += OnFinishLine;
        Ground.OnGroundInfo += RecieveGround;
    }

   
    private void OnDisable()
    {
        FinishLine.OnFInishLineCrossed -= OnFinishLine;
        Ground.OnGroundInfo -= RecieveGround;
    }

    private void RecieveGround(Collider ground)
    {
        GroundCollider = ground;
    }

    private void Start()
    {
        
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
            //Physics.IgnoreCollision(GroundCollider,other,true);
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
            //Physics.IgnoreCollision(GroundCollider,other,false);
        }
        if (other.transform.root.CompareTag("Enemy"))
        {
            Debug.Log("Exited");
            other.transform.root.GetComponent<EnemyAI>().Floored(GroundCollider);
        }
    }
}
