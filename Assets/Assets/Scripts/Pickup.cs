using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    [SerializeField] private WeaponData WeaponData;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<WeaponController>()?.PickupWeapon(WeaponData);
    }
}
