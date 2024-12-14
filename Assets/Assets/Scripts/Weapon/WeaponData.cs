using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string           WeaponName;
    public EWeaponFireType  WeaponFireType;

    [Header("Magazine Details")]
    public int CurAmmo;
    public int ReloadAmount;
    public int totalCapacity;

    [Header("Nomal Fire")]
    public int              BulletsPerFire = 1;
    public float            FireRate;

    [Header("Burst Fire")]
    public bool BurstAvailable;
    public bool BurstActive;
    public int  BurstBulletPerFire;
    public float BurstFireDelay = 0.1f;
    public float BurstFireRate;

    [Header("Spread")]
    public float BaseSpread;
    public float MaxSpread;
    public float SpreadIncreaseRate = 0.15f;

    [Header("Specsifics")]
    public EWeaponType WeaponType;
    [Range(1, 3)]
    public float ReloadSpeed = 1;
    [Range(1, 3)]
    public float SwapSpeed = 1;
    [Range(4, 8)]
    public float WeaponRange = 4;
    [Range(4, 8)]
    public float CameraRange = 6;
}
