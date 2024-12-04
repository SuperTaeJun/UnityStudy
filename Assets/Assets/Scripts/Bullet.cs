using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject BulletImpactFx;

    private Vector3 StartPos;
    private float Range;

    public void BulletSetup(float WeaponRange)
    {
        StartPos = transform.position;
        Range= WeaponRange;

    }

    private void Update()
    {
        if (Vector3.Distance(StartPos, transform.position) > Range)
            ObjectPool.instance.ReturnBullet(gameObject);
    }

    private Rigidbody rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx(collision);
        ObjectPool.instance.ReturnBullet(gameObject);
    }

    private void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contactPoint = collision.contacts[0];
            GameObject newImpactFX = Instantiate(BulletImpactFx, contactPoint.point, Quaternion.LookRotation(contactPoint.normal));
            Destroy(newImpactFX, 1f);
        }
    }
}
