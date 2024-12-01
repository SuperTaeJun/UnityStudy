using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private const float ReferenceBulletSpeed = 20.0f;

    private Player Player;


    [SerializeField] private Weapon CurWeapon;
    private bool WeaponReady;
    private bool IsFiring;

    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float BulletSpeed;

    [SerializeField] private Transform WeaponHolderTransform;

    [Header("Inventory")]
    int MaxSlotCnt = 2;
    [SerializeField] private List<Weapon> WeaponSlots;

    private void Start()
    {
        Player = GetComponent<Player>();

        AssignInputEvent();
        Invoke("StartWeapon", 0.1f);
    }
    private void Update()
    {
        if(IsFiring)
        {
            Shoot();
        }


        if(Input.GetKeyUp(KeyCode.V)) 
        {
            CurWeapon.ToggleBurst();
        }
    }


    #region Slots-Pickup,Drop,Equip
    private void StartWeapon() => EquipWeapon(0);

    private void EquipWeapon(int Index)
    {
        SetWeaponReady(false);

        CurWeapon = WeaponSlots[Index];

        Player.WeaponVisual.PlayWeaponEquipAnim();
    }

    public void PickupWeapon(Weapon NewWeapon)
    {
        if (WeaponSlots.Count >= MaxSlotCnt) return;

        WeaponSlots.Add(NewWeapon); 
        Player.WeaponVisual.SwitchOnBackupWeaponModels();
    }

    private void DropWeapon() 
    {
        if (HasOneWeapon()) return;

        WeaponSlots.Remove(CurWeapon);
        EquipWeapon(0);
    }

    public void SetWeaponReady(bool IsReady) => WeaponReady = IsReady;
    public bool GetWeaponReady() => WeaponReady;
    #endregion 


    private IEnumerator BurstFire()
    {

        SetWeaponReady(false );
        for(int i = 1; i <= CurWeapon.BulletsPerFire; i++)
        {
            FireBulletOne();

            yield return new WaitForSeconds(CurWeapon.BurstFireDelay);
            if(i >= CurWeapon.BulletsPerFire)
            {
                SetWeaponReady(true);
            }
        }
    }

    private void Shoot()
    {
        if (GetWeaponReady() == false) return;

        if (!CurWeapon.CanFire()) return;

       Player.WeaponVisual.PlayFireAnim();

        if (CurWeapon.FireType == EWeaponFireType.Single)
        {
            IsFiring = false;
        }

        if(CurWeapon.BurstActivated()==true)
        {
            StartCoroutine(BurstFire());
            return;
        }


        FireBulletOne();


    }

    private void FireBulletOne()
    {
        CurWeapon.CurAmmo--;

        GameObject NewBullet = ObjectPool.instance.GetBullet();

        NewBullet.transform.position = GetMuzzlePoint().position;
        NewBullet.transform.rotation = Quaternion.LookRotation(GetMuzzlePoint().forward);

        Rigidbody RbNewBeullet = NewBullet.GetComponent<Rigidbody>();


        Vector3 RnadBulletDir = CurWeapon.ApplySpread(BulletDir());

        RbNewBeullet.mass = ReferenceBulletSpeed / BulletSpeed;
        RbNewBeullet.velocity = RnadBulletDir * BulletSpeed;
    }

    private void Reload()
    {
        SetWeaponReady(false);
        Player.WeaponVisual.PlayReloadAnim();
    }

    public Vector3 BulletDir()
    {
        Transform Aim = Player.PlayerAim.GetAim();

        Vector3 Dir = (Aim.position - GetMuzzlePoint().position).normalized;

        if(Player.PlayerAim.CanAimPrecisly() == false)
            Dir.y = 0;

  

        return Dir;
    }

    public bool HasOneWeapon() => WeaponSlots.Count <= 1;

    public Transform GetMuzzlePoint() => Player.WeaponVisual.CurWeaponModel().MuzzlePoint;
    public Weapon GetCurWeapon() => CurWeapon;

    public Weapon GetBackupWeapon()
    {
        foreach(Weapon weapon in WeaponSlots)
        {
            if(weapon != CurWeapon)
            { 
                return weapon; 
            }
        }
        return null;
    }
    #region InputEvent
    private void AssignInputEvent()
    {
        PlayerControlls Controls = Player.Controls;

        Controls.Character.Fire.performed += context => IsFiring = true;
        Controls.Character.Fire.canceled += context => IsFiring = false;

        Controls.Character.Slot1.performed += context => EquipWeapon(0);
        Controls.Character.Slot2.performed += context => EquipWeapon(1);

        Controls.Character.DropCurWeapon.performed += context => DropWeapon();
        Controls.Character.Reload.performed += context =>
        {
            if (CurWeapon.CanReload() && GetWeaponReady())
            {
                Reload();
            }
        };
    }

    #endregion
}
