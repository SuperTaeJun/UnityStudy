using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisual : MonoBehaviour
{
    private Animator Anim;

    private bool bIsSwappingWeapon = false;

    #region WeaponTransform
    [SerializeField] private Transform[] WeaponTransform;

    [SerializeField] private Transform Pistol;
    [SerializeField] private Transform Revolver;
    [SerializeField] private Transform Rifle;
    [SerializeField] private Transform ShotGun;
    [SerializeField] private Transform Sniper;

    private Transform CurWeapon;
    #endregion

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
        SwitchOnWeapon(Pistol);
        Anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
    }

    private void Update()
    {
        CheckWeaponSwitch();

        UpdateRigWeight();
        UpdateLeftHandIkWeight();
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

    private void PlayerSwapWeaponAnim(eSwapType swapType)
    {
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

    private void SwitchOnWeapon(Transform Type)
    {
        SwitchOffWeapon();
        Type.gameObject.SetActive(true);
        CurWeapon = Type;

        AttachLeftHand();
    }
    private void SwitchOffWeapon()
    {
        for (int i = 0; i < WeaponTransform.Length; i++)
        {
            WeaponTransform[i].gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        Transform TargetTransform = CurWeapon.GetComponentInChildren<LeftHandTargetTransform>().transform;

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
    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SwitchOnWeapon(Pistol);
            SwitchAnimLayer(1);
            PlayerSwapWeaponAnim(eSwapType.SideSwap);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SwitchOnWeapon(Revolver);
            SwitchAnimLayer(1);
            PlayerSwapWeaponAnim(eSwapType.SideSwap);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SwitchOnWeapon(Rifle);
            SwitchAnimLayer(1);
            PlayerSwapWeaponAnim(eSwapType.BackSwap);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SwitchOnWeapon(ShotGun);
            SwitchAnimLayer(2);
            PlayerSwapWeaponAnim(eSwapType.BackSwap);
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            SwitchOnWeapon(Sniper);
            SwitchAnimLayer(3);
            PlayerSwapWeaponAnim(eSwapType.BackSwap);
        }
    }
}

public enum eSwapType
{
    SideSwap,
    BackSwap
};