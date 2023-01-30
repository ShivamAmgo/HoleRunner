using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] private Collider GroundCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Physics.IgnoreCollision(GroundCollider,other,true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Physics.IgnoreCollision(GroundCollider,other,false);
        }
    }
}
