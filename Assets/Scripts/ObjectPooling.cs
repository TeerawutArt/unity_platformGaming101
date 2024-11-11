using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public GameObject collectableParent;
    public int amountToPool;
    public bool isInitialized = false;
    public SpawnCoin sc;
    void Awake()
    {
        SharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        sc = GetComponent<SpawnCoin>();
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool, collectableParent.transform);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
        isInitialized = true;  // เพื่อบอกว่า pool พร้อมใช้งานแล้ว ก่อนจะไปเรียกใช้มัน
    }


    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPooledObject()
    {
        if (pooledObjects == null || pooledObjects.Count == 0)
        {
            return null;
        }
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
    public void ResetCoin(GameObject coin)
    {
        StartCoroutine(WaitAndReactivateCoin(coin));  // เรียกใช้ Coroutine เพื่อหน่วงเวลา
    }

    // Coroutine สำหรับรอเวลา 3 วินาทีและทำให้เหรียญกลับมา
    private IEnumerator WaitAndReactivateCoin(GameObject coin)
    {
        yield return new WaitForSeconds(3);  // รอ 3 วินาที
        sc.RandomSpawnCoin(coin);

    }
}
