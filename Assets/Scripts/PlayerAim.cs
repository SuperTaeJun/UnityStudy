using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControlls playerControlls;



    [Header("AimInfo")]
    [SerializeField] private float MinCameraDistance = 1.0f;
    [SerializeField] private float MaxCameraDistance = 1.5f;
    [SerializeField] private float AimSensetivity =5.0f;

    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask AimLayerMask;
    private Vector2 AimInput;

    private void Start()
    {
        player = GetComponent<Player>();
        playerControlls = player.Controls;

        AssignInputEvent();

    }

    private void Update()
    {
        aim.position = Vector3.Lerp(aim.position, DesieredAimPos(), AimSensetivity*Time.deltaTime);
    }

    private Vector3 DesieredAimPos()
    {
        float actualMaxCameraDistance = player.PlayerMovement.MoveInput.y < -0.5f ? MinCameraDistance : MaxCameraDistance;

        Vector3 desieredAimPos = GetMousePos();
        Vector3 AimDir =(desieredAimPos - transform.position).normalized;

        float DistanceToDesieredPos = Vector3.Distance(transform.position, desieredAimPos);

        float clampedDistance = Mathf.Clamp(DistanceToDesieredPos, MinCameraDistance, actualMaxCameraDistance);

        desieredAimPos = transform.position + AimDir * clampedDistance;
        desieredAimPos.y = transform.position.y + 1;

        return desieredAimPos;
    }

    public Vector3 GetMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(AimInput);

        if(Physics.Raycast(ray, out RaycastHit hitInfo,Mathf.Infinity,AimLayerMask))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }

    private void AssignInputEvent()
    {
        playerControlls.Character.Aim.performed += context => AimInput = context.ReadValue<Vector2>();
        playerControlls.Character.Aim.canceled += context => AimInput = Vector2.zero;
    }
}
