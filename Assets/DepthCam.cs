using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCam : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    private void Start()
    {
        m_camera.clearFlags = CameraClearFlags.Depth;
        m_camera.depth = 1;
    }
}
