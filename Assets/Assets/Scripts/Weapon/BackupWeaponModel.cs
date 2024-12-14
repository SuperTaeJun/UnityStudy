using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHangType
{
    LowBackHang,
    BackHang,
    SideHang
}

public class BackupWeaponModel : MonoBehaviour
{
    public EWeaponType WeaponType;
    [SerializeField] private EHangType HangType;

    public void Activate(bool activeated) => gameObject.SetActive(activeated);
    public bool HangTypeIs(EHangType type) => this.HangType == type;

}
