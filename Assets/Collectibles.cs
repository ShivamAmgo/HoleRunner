using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] private float ValuePoints = 1; 
    float MagneticSpeed=1;
    public delegate void Collected(float Points);

    public static event Collected OnCollectedItem;
    private bool Magnetism = false;
    private Transform Player;

    private void Start()
    {
        Magnetism = false;
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
    
    private void FixedUpdate()
    {
        if (Magnetism)
        {
            Vector3 Dir = Player.position - transform.position;
            transform.position += Dir * MagneticSpeed * Time.deltaTime;
        }
    }
}
