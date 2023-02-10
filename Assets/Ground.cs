using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public delegate void GroundInfo(Collider Ground);

    public static event GroundInfo OnGroundInfo;
    [SerializeField] private Collider GroundCOllider;

    private void Start()
    {
        OnGroundInfo?.Invoke(GroundCOllider);
    }
}
