using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private const float ReferenceBulletSpeed = 20.0f;

    private Player Player;


    [SerializeField] private Weapon CurWeapon;


    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private Transform MuzzlePoint;

    [SerializeField] private Transform WeaponHolderTransform;

    [Header("Inventory")]
    int MaxSlotCnt = 2;
    [SerializeField] private List<Weapon> WeaponSlots;

    private void Start()
    {
        Player = GetComponent<Player>();

        AssignInputEvent();
        //EquipWeapon(0);
    }

    #region Slots-Pickup,Drop,Equip
    private void EquipWeapon(int Index)
    {
        CurWeapon = WeaponSlots[Index];

        Player.WeaponVisual.PlayWeaponEquipAnim();
    }

    public void PickupWeapon(Weapon NewWeapon)
    {
        if (WeaponSlots.Count >= MaxSlotCnt) return;

        WeaponSlots.Add(NewWeapon);
    }

    private void DropWeapon() 
    {
        if (WeaponSlots.Count <= 1) return;

        WeaponSlots.Remove(CurWeapon);

        CurWeapon = WeaponSlots[0];
    }
    #endregion 

    private void Shoot()    
    {
        if (!CurWeapon.CanFire()) return;

        GameObject NewBullet = Instantiate(BulletPrefab, MuzzlePoint.position, Quaternion.LookRotation(MuzzlePoint.forward));
        NewBullet.GetComponent<Rigidbody>().velocity = BulletDir() * BulletSpeed;

        Rigidbody RbNewBeullet = NewBullet.GetComponent<Rigidbody>();

        RbNewBeullet.mass = ReferenceBulletSpeed / BulletSpeed;
        RbNewBeullet.velocity = BulletDir() * BulletSpeed;

        Destroy(NewBullet, 5.0f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");

    }

    public Vector3 BulletDir()
    {
        Transform Aim = Player.PlayerAim.GetAim();

        Vector3 Dir = (Aim.position - MuzzlePoint.position).normalized;

        if(Player.PlayerAim.CanAimPrecisly() == false)
            Dir.y = 0;

        //WeaponHolderTransform.LookAt(Aim);
        //MuzzlePoint.LookAt(Aim);

        return Dir;
    }

    public Transform GetMuzzlePoint() => MuzzlePoint;
    public Weapon GetCurWeapon() => CurWeapon;

    #region InputEvent
    private void AssignInputEvent()
    {
        PlayerControlls Controls = Player.Controls;

        Controls.Character.Fire.performed += context => Shoot();
        Controls.Character.Slot1.performed += context => EquipWeapon(0);
        Controls.Character.Slot2.performed += context => EquipWeapon(1);

        Controls.Character.DropCurWeapon.performed += context => DropWeapon();
        Controls.Character.Reload.performed += context =>
        {
            if(CurWeapon.CanReload())
                Player.WeaponVisual.PlayReloadAnim();
        };
    }
    #endregion
}
