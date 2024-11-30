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
    public int CurAmmo;
    public int ReloadAmount;
    public int totalCapacity;

    public float ReloadSpeed = 1f;
    public float SwapSpeed = 1f;

    [Space]
    public float FireRate = 1f; //초당 발사수
    private float LastFireTime;

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
    public bool CanFire()
    {
        if(HasAmmo() && ReadyToFire())
        {
            CurAmmo--;
            return true;
        }
        return false;
    }
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
