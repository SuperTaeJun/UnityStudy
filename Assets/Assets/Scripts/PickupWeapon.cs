using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : Interactable
{
    private WeaponController WeaponController;
    [SerializeField] private WeaponData WeaponData;
    [SerializeField] private BackupWeaponModel[] backupWeaponModels;


    private void Start()
    {
        UpdateGameObject();
    }

    [ContextMenu("UpdateWeapon")]
    protected void UpdateGameObject()
    {
        gameObject.name = "PickupWeapon - " + WeaponData.WeaponType.ToString();
        UpdateWeaponModel();
    }

    public void UpdateWeaponModel()
    {
        foreach (var model in backupWeaponModels)
        {
            model.gameObject.SetActive(false);
            if (model.WeaponType == WeaponData.WeaponType)
            {
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interact()
    {
        WeaponController?.PickupWeapon(WeaponData);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(WeaponController == null)
        {
            WeaponController = other.GetComponent<WeaponController>();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
