using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisual : MonoBehaviour
{
    private Player Player;
    private Animator Anim;
    private bool bIsSwappingWeapon = false;

    [SerializeField] private WeaponModel[] WeaponModels;

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



    public void PlayReloadAnim()
    {
        if (bIsSwappingWeapon) return;

        Anim.SetTrigger("Reload");
        PauseRig();
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

    public void PlayWeaponEquipAnim()
    {
        ESwapType swapType = CurWeaponModel().SwapType;

        Debug.Log(swapType);

        LeftHandIk.weight = 0; 
        PauseRig();
        Anim.SetFloat("WeaponSwapType", ((float)swapType));
        Anim.SetTrigger("WeaponSwap");

        SetbIsSwappingWeapon(true);
    }
    public void SetbIsSwappingWeapon(bool bIsSwapping)
    {
        bIsSwappingWeapon = bIsSwapping;
        Anim.SetBool("IsSwapping", bIsSwappingWeapon);

    }
    public void SwitchOnCurWeaponModel()
    {
        int AnimIndex = ((int)CurWeaponModel().HoldType);
        SwitchAnimLayer(AnimIndex);

        Player.WeaponVisual.SwitchOffWeapon();
        CurWeaponModel().gameObject.SetActive(true);

        AttachLeftHand();
    }
    public void SwitchOffWeapon()
    {
        for (int i = 0; i < WeaponModels.Length; i++)
        {
            WeaponModels[i].gameObject.SetActive(false);
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

