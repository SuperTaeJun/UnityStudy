using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAxe : MonoBehaviour
{
    [SerializeField] private GameObject ImpactFX;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform AxeVisual;

    private Vector3 Dir;
    private Transform Player;
    private float FlySpeed;
    private float RotSpeed;
    private float Timer = 1f;
    private TrailRenderer TrailRenderer;

    private void Awake()
    {
        TrailRenderer = GetComponentInChildren<TrailRenderer>();
    }

        public void AxeSetup(float FlySpeed, Transform Player, float Timer)
    {
        RotSpeed = 1500f;

        this.FlySpeed = FlySpeed;
        this.Player = Player;
        this.Timer = Timer;
    }

    private void Update()
    {
        AxeVisual.Rotate(Vector3.right * RotSpeed * Time.deltaTime);
        Timer -= Time.deltaTime;

        if(Timer > 0)
            Dir = Player.position + Vector3.up - transform.position;

        rb.velocity = Dir * FlySpeed;
        transform.forward = rb.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        Player player = other.GetComponent<Player>();

        if(bullet || player)
        {
            GameObject NewFX = ObjectPool.instance.GetObject(ImpactFX);
            NewFX.transform.position = transform.position;

            ObjectPool.instance.ReturnToPool(gameObject);
            ObjectPool.instance.ReturnToPool(NewFX,1f);
            TrailRenderer.Clear();
        }

    }
}
