using System.Collections.Generic;
using UnityEngine;

public enum EAmmoBoxType
{
    LargeBox,
    SmallBox
}

public class PickupAmmo : Interactable
{
    [SerializeField] EAmmoBoxType AmmoBoxType;

    [System.Serializable]
    public struct AmmoData
    {
        public EWeaponType WeaponType;
        public int MinAmount;
        public int MaxAmount;
    }
    [SerializeField] List<AmmoData> SmallBoxAmmo;
    [SerializeField] List<AmmoData> LargeBoxAmmo;

    [SerializeField] private GameObject[] BoxModels;
    private WeaponController WeaponController;

    public void Start()
    {
        SetupBoxModels();

    }


    public override void Interact()
    {
        List<AmmoData> CurAmmoList;
        if(AmmoBoxType == EAmmoBoxType.SmallBox)
        {
            CurAmmoList = SmallBoxAmmo;
        }
        else
        {
            CurAmmoList = LargeBoxAmmo;
        }

        foreach (AmmoData curAmmo in CurAmmoList)
        {
            Weapon weapon = WeaponController.HasWeaponInSlot(curAmmo.WeaponType);

            AddBulletToWeapon(weapon,GetRandBulletAmount(curAmmo));
        }

        ObjectPool.instance.ReturnToPool(gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(WeaponController == null)
        {
            WeaponController = other.GetComponent<WeaponController>();
        }
    }

    private int GetRandBulletAmount(AmmoData Data)
    {
        //float min = Mathf.Min(Data.MinAmount, Data.MaxAmount);
        //float max = Mathf.Max(Data.MinAmount, Data.MaxAmount);

        int RandAmount = Random.Range(Data.MinAmount, Data.MaxAmount);

        return RandAmount;
    }

    private static void AddBulletToWeapon(Weapon weapon,int curAmmo)
    {
        if (weapon != null)
        {
            weapon.totalCapacity += curAmmo;
        }
    }
    private void SetupBoxModels()
    {
        for (int i = 0; i < BoxModels.Length; i++)
        {
            BoxModels[i].SetActive(false);

            if (i == ((int)AmmoBoxType))
            {
                BoxModels[i].SetActive(true);
                UpdateMeshAndMaterial(BoxModels[i].GetComponent<MeshRenderer>());
            }
        }
    }


}
