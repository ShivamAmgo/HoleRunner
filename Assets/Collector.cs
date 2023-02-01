using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Destroyer d = other.transform.GetComponentInParent<Destroyer>();
        if (d!=null)
        {
            d.DestroyIt();
        }
    }
}
