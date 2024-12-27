using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ImpactForce;

    private Rigidbody Rigidbody;
    private BoxCollider BoxCollition;
    private MeshRenderer MeshRenderer;
    private TrailRenderer TrailRenderer;
    [SerializeField] private GameObject BulletImpactFx;

    private Vector3 StartPos;
    private float Range;
    private bool BulletDisabled;


    private void Awake()
    {
        BoxCollition = GetComponent<BoxCollider>();
        MeshRenderer = GetComponent<MeshRenderer>();
        TrailRenderer = GetComponent<TrailRenderer>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void BulletSetup(float WeaponRange, float ImpactForce)
    {
        this.ImpactForce = ImpactForce;
        BulletDisabled = false;
        MeshRenderer.enabled = true;
        BoxCollition.enabled = true;

        TrailRenderer.time = 0.25f;
        StartPos = transform.position;
        Range= WeaponRange+ 0.5f;

    }

    private void Update()
    {
        FadeTrail();

        CheckBulletDisable();

        CheckRetrunBullet();
    }

    private void CheckRetrunBullet()
    {
        if (TrailRenderer.time < 0)
        {
            TrailRenderer.Clear();
            ObjectPool.instance.ReturnToPool(gameObject);
        }
    }

    private void CheckBulletDisable()
    {
        if (Vector3.Distance(StartPos, transform.position) > Range && BulletDisabled == false)
        {
            MeshRenderer.enabled = false;
            BoxCollition.enabled = false;
            BulletDisabled = true;
        }
    }

    private void FadeTrail()
    {
        if (Vector3.Distance(StartPos, transform.position) > Range - 1.5f)
        {
            TrailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    private Rigidbody rb => Rigidbody;

    private void OnCollisionEnter(Collision collision)
    {

        Enemy Enemy = collision.gameObject.GetComponentInParent<Enemy>();

        if(Enemy)
        {
            Vector3 Force = rb.velocity.normalized * ImpactForce;
            Rigidbody HitRb = collision.collider.attachedRigidbody;

            Enemy.GetHit();
            Enemy.HitImpact(Force, collision.contacts[0].point, HitRb);
        }


        CreateImpactFx(collision);

        TrailRenderer.Clear();
        ReturnBulletPool();
    }

    private void ReturnBulletPool() => ObjectPool.instance.ReturnToPool(gameObject);
    

    private void CreateImpactFx(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint contactPoint = collision.contacts[0];


            GameObject newImpactFX = ObjectPool.instance.GetObject(BulletImpactFx);
            newImpactFX.transform.position = contactPoint.point;

            ObjectPool.instance.ReturnToPool(newImpactFX,1f);
        }
    }
}
