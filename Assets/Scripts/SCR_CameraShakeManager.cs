using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SCR_CameraShakeManager : MonoBehaviour
{
    public static SCR_CameraShakeManager Instance;

    [SerializeField] private float globalShakeForce = 1f;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}
