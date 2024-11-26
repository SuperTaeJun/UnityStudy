using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private Player Player;

    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private float BulletSpeed;
    [SerializeField] private Transform MuzzlePoint;

    [SerializeField] private Transform WeaponHolderTransform;
    [SerializeField] private Transform Aim;

    private void Start()
    {
        Player = GetComponent<Player>();

        Player.Controls.Character.Fire.performed += context => Shoot();
    }

    private void Shoot()
    {
        GameObject NewBullet = Instantiate(BulletPrefab,MuzzlePoint.position,Quaternion.LookRotation(MuzzlePoint.forward));
        NewBullet.GetComponent<Rigidbody>().velocity = BulletDir() * BulletSpeed;

        Destroy(NewBullet, 5.0f);

        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    private Vector3 BulletDir()
    {
        Vector3 Dir = (Aim.position - MuzzlePoint.position).normalized;

        if(Player.PlayerAim.CanAimPrecisly() == false)
            Dir.y = 0;

        WeaponHolderTransform.LookAt(Aim);
        MuzzlePoint.LookAt(Aim);

        return Dir;
    }
}
