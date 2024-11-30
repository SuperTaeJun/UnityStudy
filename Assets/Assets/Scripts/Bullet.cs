using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject BulletImpactFx;

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
