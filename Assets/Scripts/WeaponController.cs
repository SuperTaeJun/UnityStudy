using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Player Player;

    private void Start()
    {
        Player = GetComponent<Player>();

        Player.Controls.Character.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
}
