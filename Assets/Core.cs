using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] private Collider GroundCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible") )
        {
            Physics.IgnoreCollision(GroundCollider,other,true);
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
            Physics.IgnoreCollision(GroundCollider,other,false);
        }
        if (other.transform.root.CompareTag("Enemy"))
        {
            Debug.Log("Exited");
            other.transform.root.GetComponent<EnemyAI>().Floored(GroundCollider);
        }
    }
}
