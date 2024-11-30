using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    private PlayerWeaponVisual WeaponVisualController;
    private WeaponController WeaponController;
    private void Start()
    {
        WeaponVisualController = GetComponentInParent<PlayerWeaponVisual>();
        WeaponController= GetComponentInParent<WeaponController>();
    }

    public void ReloadOver()
    {
        WeaponVisualController.MaximizeRigWeight();
        WeaponController.GetCurWeapon().Reload();

        WeaponController.SetWeaponReady(true);
    }

    public void RetrunRig()
    {
        WeaponVisualController.MaximizeRigWeight();
        WeaponVisualController.MaximizeLeftHandIkWeight();
    }
    public void WeaponSwapOver()
    {

        WeaponController.SetWeaponReady(true);
    }

    public void SwitchOnWeaponModel() => WeaponVisualController.SwitchOnCurWeaponModel();
}
