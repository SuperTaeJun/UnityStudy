using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    private PlayerWeaponVisual WeaponVisualController;

    private void Start()
    {
        WeaponVisualController = GetComponentInParent<PlayerWeaponVisual>();
    }

    public void ReloadOver()
    {
        WeaponVisualController.MaximizeRigWeight();

        //fill ammo ...

    }

    public void RetrunRig()
    {
        WeaponVisualController.MaximizeRigWeight();
        WeaponVisualController.MaximizeLeftHandIkWeight();
    }
    public void WeaponSwapOver()
    {
        WeaponVisualController.SetbIsSwappingWeapon(false);
    }
}
