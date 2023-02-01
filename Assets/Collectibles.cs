using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    //[SerializeField] private float ValuePoints = 1; 
    [SerializeField]float MagneticSpeed=5;
    public delegate void Collected(float Points);

    public static event Collected OnCollectedItem;
    private bool Magnetism = false;
    private Transform Player;
    private Rigidbody Rb;
    private Collider[] Colliders;
    [SerializeField] private bool Collectable = true;
    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        Magnetism = false;
        Colliders = GetComponentsInChildren<Collider>();
        Rb.isKinematic = true;
    }

    private void OnEnable()
    {
        PlayerMovement.DeliverPlayerInfo += ReceivePlayer;
        Magnet.OnMagnetActive += OnMagnetActive;
    }

    
    private void OnDisable()
    {
        PlayerMovement.DeliverPlayerInfo -= ReceivePlayer;
        Magnet.OnMagnetActive -= OnMagnetActive;
    }

    private void ReceivePlayer(PlayerMovement player)
    {
        Player = player.transform;
    }
    private void OnMagnetActive(bool activestatus)
    {
        Magnetism = activestatus;
        Rb.isKinematic = !activestatus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collector"))
        {
            //OnCollectedItem?.Invoke(ValuePoints);
        }

        if (other.CompareTag("Magnet") && !Magnetism)
        {
            Magnetism = true;
        }
    }

    public void CollectedItem(bool CollideStatus,Collider groundCol)
    {
        if (Rb.isKinematic)
        {
            Rb.isKinematic = !CollideStatus;
        }

        if (!Collectable)
        {
            return;
        }
        foreach (Collider c in Colliders)
        {
            Physics.IgnoreCollision(groundCol,c,CollideStatus);
        }
        
    }
    private void FixedUpdate()
    {
        if (Magnetism)
        {
            Vector3 Dir = Player.position - transform.position;
            Dir.y=0;
            Debug.Log("direction "+Dir);
            transform.position += Dir * MagneticSpeed * Time.deltaTime;
        }
    }
}
