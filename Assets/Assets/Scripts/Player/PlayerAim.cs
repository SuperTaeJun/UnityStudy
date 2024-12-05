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

    [Header("AimLaser")]
    [SerializeField] private LineRenderer AimLaser;

    [Header("Aim")]
    [SerializeField] private Transform Aim;

    [SerializeField] private bool IsAimingPrecisly;

    [Header("CameraInfo")]
    [SerializeField] private float MinCameraDistance = 1.0f;
    [SerializeField] private float MaxCameraDistance = 1.5f;
    [SerializeField] private float CameraSensetivity = 5.0f;

    [SerializeField] private Transform CameraTarget;
    [SerializeField] private LayerMask AimLayerMask;
    private Vector2 MouseInput;
    private RaycastHit LastknownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        playerControlls = player.Controls;

        AssignInputEvent();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) { IsAimingPrecisly = !IsAimingPrecisly; }


        UpdateAimLaser();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    private void UpdateAimLaser()
    {
        AimLaser.enabled = player.WeaponController.GetWeaponReady();

        if (AimLaser.enabled == false) return;

    
        WeaponModel weaponModel = player.WeaponVisual.CurWeaponModel();

        weaponModel.transform.LookAt(Aim);
        weaponModel.MuzzlePoint.LookAt(Aim); 

        Vector3 LaserDir = player.WeaponController.BulletDir();
        Transform MuzzlePoint = player.WeaponController.GetMuzzlePoint();

        float TipLenght = 0.5f;
        float WeaponRange = player.WeaponController.GetCurWeapon().WeaponRange;

        Vector3 EndPoint = MuzzlePoint.position + LaserDir * WeaponRange;

        if (Physics.Raycast(MuzzlePoint.position, LaserDir, out RaycastHit hit, WeaponRange))
        {
            EndPoint = hit.point;
            TipLenght = 0f;
        }

        AimLaser.SetPosition(0, MuzzlePoint.position);
        AimLaser.SetPosition(1, EndPoint);
        AimLaser.SetPosition(2, EndPoint + LaserDir * TipLenght);
    }
    private void UpdateAimPosition()
    {
        Aim.position = GetMouseHitInfo().point;

        if (!IsAimingPrecisly)
            Aim.position = new Vector3(Aim.position.x, transform.position.y + 1, Aim.position.z);
    }

    public Transform GetAim() => Aim;
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(MouseInput);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, AimLayerMask))
        {
            LastknownMouseHit = hitInfo;
            return hitInfo;
        }
        return LastknownMouseHit;
    }
    public bool CanAimPrecisly() => IsAimingPrecisly;

    #region Camera Region
    private Vector3 DesieredCameraPos()
    {
        float actualMaxCameraDistance = player.PlayerMovement.MoveInput.y < -0.5f ? MinCameraDistance : MaxCameraDistance;

        Vector3 desieredCameraPos = GetMouseHitInfo().point;
        Vector3 AimDir = (desieredCameraPos - transform.position).normalized;

        float DistanceToDesieredPos = Vector3.Distance(transform.position, desieredCameraPos);

        float clampedDistance = Mathf.Clamp(DistanceToDesieredPos, MinCameraDistance, actualMaxCameraDistance);

        desieredCameraPos = transform.position + AimDir * clampedDistance;
        desieredCameraPos.y = transform.position.y + 1;

        return desieredCameraPos;
    }
    private void UpdateCameraPosition()
    {
        CameraTarget.position = Vector3.Lerp(CameraTarget.position, DesieredCameraPos(), CameraSensetivity * Time.deltaTime);
    }

    #endregion

    private void AssignInputEvent()
    {
        playerControlls.Character.Aim.performed += context => MouseInput = context.ReadValue<Vector2>();
        playerControlls.Character.Aim.canceled += context => MouseInput = Vector2.zero;
    }

}
