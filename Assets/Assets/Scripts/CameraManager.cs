using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private CinemachineVirtualCamera CinemachinCam;
    private CinemachineFramingTransposer Transposer;

    [Header("CameraDistance")]
    [SerializeField] private bool CanChangeCameraDistance;
    [SerializeField] private float DistanceChangeRate;
    private float TargetCameraDistance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);

        CinemachinCam = GetComponentInChildren<CinemachineVirtualCamera>();
        Transposer = CinemachinCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        UpdateCameraDistance();
    }

    private void UpdateCameraDistance()
    {
        if (CanChangeCameraDistance == false) return;

        float CurCameraDistanc = Transposer.m_CameraDistance;

        if (Mathf.Abs(TargetCameraDistance - CurCameraDistanc) > 0.01f)
        {
            Transposer.m_CameraDistance = Mathf.Lerp(CurCameraDistanc, TargetCameraDistance, Time.deltaTime * DistanceChangeRate);
        }
    }

    public void ChangeCameraRange(float range) => TargetCameraDistance = range;
}
