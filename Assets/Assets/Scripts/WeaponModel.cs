using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESwapType
{
    SideSwap,
    BackSwap
};

public enum EHoldType
{
    Common = 1,
    Low,
    High
}

public class WeaponModel : MonoBehaviour
{
    public EWeaponType WeaponType;
    public ESwapType SwapType;
    public EHoldType HoldType;

    public Transform MuzzlePoint;
    public Transform HoldPoint;
}
