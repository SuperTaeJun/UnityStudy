public enum EWeaponType
{
    Pistol,
    Revolver,
    Rifle,
    Shotgun,
    Sniper
}



[System.Serializable]
public class Weapon
{
    public EWeaponType WeaponType;
    public int CurAmmo;
    public int ReloadAmount;
    public int totalCapacity;


    public bool CanFire()
    {
        return HasAmmo();
    }

    private bool HasAmmo()
    {
        if (CurAmmo > 0)
        {
            CurAmmo--;
            return true;
        }

        return false;
    }
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
        totalCapacity += CurAmmo; //���ݰ������ִ� �Ѿ� �ٽ� �߰����ֱ�

        int BulletToReload = ReloadAmount;
        if(BulletToReload> totalCapacity)
        {
            BulletToReload = totalCapacity;
        }

        totalCapacity -= BulletToReload;
        CurAmmo = BulletToReload;
    }
}
