using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private int BulletPoolSize = 10;

    private Queue<GameObject> BulletPool;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

    }

    private void Start()
    {
        BulletPool = new Queue<GameObject>();
        InitPool();
    }
    public GameObject GetBullet()
    {
        if(BulletPool.Count == 0)
            CreateNewBullet();

        GameObject Bullet = BulletPool.Dequeue();
        Bullet.SetActive(true);
        Bullet.transform.parent = null;
        return Bullet;
    }

    public void ReturnBullet(GameObject Bullet)
    {
        Bullet.GetComponentInChildren<TrailRenderer>().Clear();

        Bullet.SetActive(false);
        BulletPool.Enqueue(Bullet);
        Bullet.transform.parent = transform;
    }

    private void InitPool()
    {
        for(int i = 0; i < BulletPoolSize; i++)
        {
            CreateNewBullet();
        }
    }

    private void CreateNewBullet()
    {
        GameObject newBullet = Instantiate(BulletPrefab, transform);
        newBullet.SetActive(false);
        BulletPool.Enqueue(newBullet);
    }
}
