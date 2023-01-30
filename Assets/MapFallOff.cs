using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFallOff : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Collider Collu;
    private void Start()
    {
        Collu.enabled = false;
        StartCoroutine(EnableCollider());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root!=null)
        {
            Destroy(other.transform.gameObject);
        }
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(2);
        Collu.enabled = true;
    }
}
