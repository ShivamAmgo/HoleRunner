using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    private bool Used = false;

    [SerializeField] private Collider[] CollidersToDisable;

    [SerializeField] float Points = 100;
    //[SerializeField]private AlliesMovement AllieToSpawn;


    private void OnTriggerEnter(Collider other)
    {
        if (Used)
        {
            return;
        }

        if (other.transform.CompareTag("Player"))
        {
            Used = true;
//            Debug.Log("Door");
            //GameManagerTelekenisis.Instance.ActivateAllies(AllieToSpawn,transform);
            foreach (Collider c in CollidersToDisable)
            {
                c.enabled = false;
            }
            PlayerScaler.Instance.AddScore(Points);
        }


        //Debug.Log(other.name);
    }
}