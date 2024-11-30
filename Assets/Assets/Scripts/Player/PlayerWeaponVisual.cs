using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisual : MonoBehaviour
{
    private Player Player;
    private Animator Anim;

    [SerializeField] private WeaponModel[] WeaponModels;
    [SerializeField] private BackupWeaponModel[] BackupWeaponModels;

    [Header("Rig")]
    [SerializeField] float RigIncreseSpeed = 6.0f;
    private bool bShouldbeIncreseRigWeight =false;
    private Rig rig;

    [Header("LeftHandIK")]
    [SerializeField] private TwoBoneIKConstraint LeftHandIk;
    [SerializeField] private Transform LeftHandIkTarget;
    [SerializeField] private float LeftHandIncreseSpeed = 4.0f;
    private bool bShouldIncreseLeftHandIkWight = false;



    private void Start()
    {
        Player = GetComponent<Player>();
        Anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        WeaponModels = GetComponentsInChildren<WeaponModel>(true);
        BackupWeaponModels = GetComponentsInChildren<BackupWeaponModel>(true);
    }

    private void Update()
    {
        UpdateRigWeight();
        UpdateLeftHandIkWeight();
    }

    public WeaponModel CurWeaponModel()
    {
        WeaponModel WeaponModel = null;

        EWeaponType weaponType = Player.WeaponController.GetCurWeapon().WeaponType;
        for (int i = 0; i < WeaponModels.Length; i++)
        {
            if (WeaponModels[i].WeaponType == weaponType)
            {
                Debug.Log(weaponType);
                WeaponModel = WeaponModels[i];
            }
        }
        return WeaponModel;
    }


    public void PlayFireAnim() => Anim.SetTrigger("Fire");
    public void PlayReloadAnim()
    {
        float ReloadSpeed = Player.WeaponController.GetCurWeapon().ReloadSpeed;

        Anim.SetTrigger("Reload");
        Anim.SetFloat("ReloadSpeed", ReloadSpeed);
        PauseRig();
    }
    public void PlayWeaponEquipAnim()
    {
        ESwapType swapType = CurWeaponModel().SwapType;

        float EquipAnimSpeed = Player.WeaponController.GetCurWeapon().SwapSpeed;

        Debug.Log(swapType);

        LeftHandIk.weight = 0; 
        PauseRig();
        Anim.SetFloat("WeaponSwapType", ((float)swapType));
        Anim.SetTrigger("WeaponSwap");
        Anim.SetFloat("SwapSpeed", EquipAnimSpeed);

    }

    private void UpdateLeftHandIkWeight()
    {
        if (bShouldIncreseLeftHandIkWight)
        {
            if (LeftHandIk.weight >= 1)
                bShouldIncreseLeftHandIkWight = false;
            LeftHandIk.weight += LeftHandIncreseSpeed * Time.deltaTime;
        }
    }
    private void UpdateRigWeight()
    {
        if (bShouldbeIncreseRigWeight)
        {
            if (rig.weight >= 1)
                bShouldbeIncreseRigWeight = false;
            rig.weight += RigIncreseSpeed * Time.deltaTime;
        }
    }

    private void PauseRig()
    {
        rig.weight = 0;
    }
    public void MaximizeRigWeight() => bShouldbeIncreseRigWeight = true ;
    public void MaximizeLeftHandIkWeight() => bShouldIncreseLeftHandIkWight = true;

    public void SwitchOnCurWeaponModel()
    {
        int AnimIndex = ((int)CurWeaponModel().HoldType);

        SwitchOffBackupWeaponModels();
        SwitchOffWeaponModels();

        if(Player.WeaponController.HasOneWeapon()==false)
            SwitchOnBackupWeaponModels();

        SwitchAnimLayer(AnimIndex);
        CurWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }

    public void SwitchOnBackupWeaponModels() 
    {
        EWeaponType weaponType = Player.WeaponController.GetBackupWeapon().WeaponType;

        foreach(BackupWeaponModel backupModel in BackupWeaponModels)
        {
            if(backupModel.WeaponType == weaponType)
                backupModel.gameObject.SetActive(true);
        }
    }
    public void SwitchOffWeaponModels()
    {
        for (int i = 0; i < WeaponModels.Length; i++)
        {
            WeaponModels[i].gameObject.SetActive(false);
        }
    }

    public void SwitchOffBackupWeaponModels()
    {
        foreach(BackupWeaponModel backupModel in BackupWeaponModels)
        {
            backupModel.gameObject.SetActive(false);
        }
    }
    private void AttachLeftHand()
    {
        Transform TargetTransform = CurWeaponModel().HoldPoint;

        LeftHandIkTarget.localPosition = TargetTransform.localPosition;
        LeftHandIkTarget.localRotation = TargetTransform.localRotation;
    }
    private void SwitchAnimLayer(int LayerIndex)
    {
        for(int i = 1;i < Anim.layerCount;i++) 
        {
            Anim.SetLayerWeight(i, 0);
        }
        Anim.SetLayerWeight(LayerIndex, 1);
    }
}

