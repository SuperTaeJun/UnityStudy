using System;
using Unity.Mathematics;
using UnityEngine;

public enum EWeaponType
{
    Pistol,
    Revolver,
    Rifle,
    Shotgun,
    Sniper
}
public enum EWeaponFireType
{
    Single,
    Auto
}


[System.Serializable]
public class Weapon
{
    public EWeaponType WeaponType;
    public EWeaponFireType FireType;

    public int BulletsPerFire { get; private set; }

    private float DefaultFireRate;
    private float FireRate = 1f; //초당 발사수
    private float LastFireTime;

    [Header("BurstFire")]
    private bool BurstFireAvaliable;
    public bool BurstActive;
    private int BurstBulletsPerFire;
    private float BurstFireRate;
    public float BurstFireDelay { get; private set; }

    [Header("Magazine Details")]
    public int CurAmmo;
    public int ReloadAmount;
    public int totalCapacity;

    public float ReloadSpeed {  get; private set; }
    public float SwapSpeed { get; private set; }
    public float WeaponRange { get; private set; }
    public float CameraRange { get; private set; }
    [Header("Spread")]
    private float BaseSpreadValue;
    private float CurSpreadValue = 2;
    private float MaxSpreadValue = 3;

    private float IncreaseSpreadRate = 0.15f;

    private float LastSpreadUpdateTime;
    private float SpreadCooldown = 1;

    public Weapon(WeaponData weaponData)
    {
        WeaponType = weaponData.WeaponType;
        FireType = weaponData.WeaponFireType;

        CurAmmo = weaponData.CurAmmo;
        ReloadAmount = weaponData.ReloadAmount;
        totalCapacity = weaponData.totalCapacity;

        BulletsPerFire = weaponData.BulletsPerFire;
        FireRate = weaponData.FireRate;
        DefaultFireRate = FireRate;

        BaseSpreadValue = weaponData.BaseSpread;
        MaxSpreadValue = weaponData.MaxSpread;
        IncreaseSpreadRate = weaponData.SpreadIncreaseRate;

        ReloadSpeed = weaponData.ReloadSpeed;
        SwapSpeed = weaponData.SwapSpeed;
        WeaponRange = weaponData.WeaponRange;
        CameraRange = weaponData.CameraRange;

        BurstFireAvaliable = weaponData.BurstAvailable;
        BurstActive = weaponData.BurstActive;
        BurstBulletsPerFire = weaponData.BurstBulletPerFire;
        BurstFireDelay = weaponData.BurstFireDelay;
        BurstFireRate = weaponData.BurstFireRate;

    }

    #region Burst Methods
    public bool BurstActivated()
    {
        if(WeaponType == EWeaponType.Shotgun)
        {
            BurstFireDelay = 0f;
            return true;
        }

        return BurstActive;
    }

    public void ToggleBurst()
    {
        if (BurstFireAvaliable == false) return;

        BurstActive = !BurstActive;

        if(BurstActive)
        {
            BulletsPerFire = BurstBulletsPerFire;
            FireRate = BurstFireRate;
        }
        else
        {
            BulletsPerFire = 1;
            FireRate = DefaultFireRate;
        }
    }
    #endregion

    #region Spread
    public Vector3 ApplySpread(Vector3 OriginDir)
    {
        UpdateSpread();
        float RandomizedValue = UnityEngine.Random.Range(-CurSpreadValue, CurSpreadValue);

        Quaternion spreadRot = Quaternion.Euler(RandomizedValue, RandomizedValue, RandomizedValue);


        return spreadRot * OriginDir;
    }

    private void UpdateSpread()
    {
        if(Time.time > LastSpreadUpdateTime+SpreadCooldown) 
        {
            CurSpreadValue = BaseSpreadValue;
        }
        else
        {
            IncreaseSpread();
        }
        LastSpreadUpdateTime = Time.time;
    }

    public void IncreaseSpread()
    {
        CurSpreadValue = Mathf.Clamp(CurSpreadValue+IncreaseSpreadRate, BaseSpreadValue, MaxSpreadValue);
    }
    #endregion

    private bool HasAmmo() => CurAmmo > 0;

    public bool CanReload()
    {
        if (CurAmmo == ReloadAmount) return false;
        if(totalCapacity>0)
        {
            return true;
        }
        return false;
    }
    public void Reload()
    {
        totalCapacity += CurAmmo; //지금가지고있는 총알 다시 추가해주기

        int BulletToReload = ReloadAmount;
        if(BulletToReload> totalCapacity)
        {
            BulletToReload = totalCapacity;
        }

        totalCapacity -= BulletToReload;
        CurAmmo = BulletToReload;
    }
    public bool CanFire() => HasAmmo() && ReadyToFire();
    private bool ReadyToFire()
    {
        if(Time.time> LastFireTime + 1 / FireRate)
        {
            LastFireTime = Time.time;
            return true;
        }
        return false;
    }
}
