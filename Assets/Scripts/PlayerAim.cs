using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControlls playerControlls;

    [Header("Aim")]
    [SerializeField] private Transform Aim;



    [Header("CameraInfo")]
    [SerializeField] private float MinCameraDistance = 1.0f;
    [SerializeField] private float MaxCameraDistance = 1.5f;
    [SerializeField] private float CameraSensetivity =5.0f;

    [SerializeField] private Transform CameraTarget;
    [SerializeField] private LayerMask AimLayerMask;
    private Vector2 AimInput;
    private RaycastHit LastknownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        playerControlls = player.Controls;

        AssignInputEvent();

    }

    private void Update()
    {
        Aim.position = GetMouseHitInfo().point;
        Aim.position = new Vector3(Aim.position.x, transform.position.y + 1, Aim.position.z); 
        CameraTarget.position = Vector3.Lerp(CameraTarget.position, DesieredCameraPos(), CameraSensetivity*Time.deltaTime);
    }

    private Vector3 DesieredCameraPos()
    {
        float actualMaxCameraDistance = player.PlayerMovement.MoveInput.y < -0.5f ? MinCameraDistance : MaxCameraDistance;

        Vector3 desieredCameraPos = GetMouseHitInfo().point;
        Vector3 AimDir =(desieredCameraPos - transform.position).normalized;

        float DistanceToDesieredPos = Vector3.Distance(transform.position, desieredCameraPos);

        float clampedDistance = Mathf.Clamp(DistanceToDesieredPos, MinCameraDistance, actualMaxCameraDistance);

        desieredCameraPos = transform.position + AimDir * clampedDistance;
        desieredCameraPos.y = transform.position.y + 1;

        return desieredCameraPos;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(AimInput);

        if(Physics.Raycast(ray, out RaycastHit hitInfo,Mathf.Infinity,AimLayerMask))
        {
            LastknownMouseHit = hitInfo;
            return hitInfo;
        }
        return LastknownMouseHit;
    }

    private void AssignInputEvent()
    {
        playerControlls.Character.Aim.performed += context => AimInput = context.ReadValue<Vector2>();
        playerControlls.Character.Aim.canceled += context => AimInput = Vector2.zero;
    }
}
