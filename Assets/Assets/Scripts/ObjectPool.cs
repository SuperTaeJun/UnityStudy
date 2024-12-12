using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private int BulletPoolSize = 10;

    private Dictionary<GameObject, Queue<GameObject>> PoolDic = new Dictionary<GameObject, Queue<GameObject>>();

    [Header("Init")]
    [SerializeField] private GameObject WeaponPickup;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

    }

    private void Start()
    {
        InitPool(WeaponPickup);
    }

    public GameObject GetObject(GameObject Prefab)
    {
        if(PoolDic.ContainsKey(Prefab)==false)
        {
            InitPool(Prefab);
        }

        if (PoolDic[Prefab].Count == 0)
        {
            CreateNewObject(Prefab);
        }

        GameObject ObjectToGet = PoolDic[Prefab].Dequeue();
        ObjectToGet.SetActive(true);
        ObjectToGet.transform.parent = null;
        return ObjectToGet;
    }
    private IEnumerator DelayReturn(GameObject ObjectToReturn, float Delay)
    {
        yield return new WaitForSeconds(Delay);

        ReturnToPool(ObjectToReturn);
    }
    public void ReturnToPool(GameObject ObjectToReturn, float Delay) => StartCoroutine(DelayReturn(ObjectToReturn, Delay));
    public void ReturnToPool(GameObject ObjectToReturn)
    {
        GameObject OrignPrefab = ObjectToReturn.GetComponent<PooledObject>().OrginPrefab;
        
        ObjectToReturn.SetActive(false);
        ObjectToReturn.transform.parent = transform;

        PoolDic[OrignPrefab].Enqueue(ObjectToReturn);

    }

    private void InitPool(GameObject Prefab)
    {
        PoolDic[Prefab] = new Queue<GameObject>();

        for(int i = 0; i < BulletPoolSize; i++)
        {
            CreateNewObject(Prefab);
        }
    }

    private void CreateNewObject(GameObject Prefab)
    {
        GameObject NewObejct = Instantiate(Prefab, transform);
        NewObejct.AddComponent<PooledObject>().OrginPrefab = Prefab;

        NewObejct.SetActive(false);
        PoolDic[Prefab].Enqueue(NewObejct);
    }
}
