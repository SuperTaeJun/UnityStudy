using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Player Player;

    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private Transform MuzzlePoint;


    private void Start()
    {
        Player = GetComponent<Player>();

        Player.Controls.Character.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        GameObject NewBullet = Instantiate(BulletPrefab,MuzzlePoint.position,Quaternion.LookRotation(MuzzlePoint.forward));
        NewBullet.GetComponent<Rigidbody>().velocity = MuzzlePoint.forward * BulletSpeed;

        Destroy(NewBullet, 5.0f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
}
