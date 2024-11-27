using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;
    public Dictionary<string, List<GameObject>> pooledObjects;
    public GameObject coinPrefab;
    public GameObject powerUpPrefab;
    public GameObject collectableParent;
    public int coinAmountToPool = 10; //ไปตั้งเพิ่มเอา
    public int powerUpAmountToPool = 0;
    public bool isInitialized = false;
    private SpawnObj sc;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        sc = GetComponent<SpawnObj>();
        powerUpAmountToPool = sc.spawnPowerArea.Length;
        pooledObjects = new Dictionary<string, List<GameObject>>();
        InitializePool("coin", coinPrefab, coinAmountToPool);
        InitializePool("powerUp", powerUpPrefab, powerUpAmountToPool);
        isInitialized = true;

    }


    private void InitializePool(string key, GameObject prefab, int amount)
    {
        List<GameObject> objectPool = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject tmp = Instantiate(prefab, collectableParent.transform);
            ItemController tmpObj = tmp.AddComponent<ItemController>();
            tmpObj.index = i;
            tmp.SetActive(false);
            objectPool.Add(tmp);
        }
        pooledObjects[key] = objectPool;
    }

    public GameObject GetPooledObject(string key)
    {
        if (pooledObjects.ContainsKey(key))
        {
            foreach (GameObject obj in pooledObjects[key])
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
        }
        return null;
    }

    //ส่วนของ respawn collect ต่างๆ (ตอนนี้เหรียญไม่ได้ให้เกิดใหม่)
    public void ResetObject(GameObject obj, string key, ItemController item)
    {
        StartCoroutine(WaitAndReactivateObject(obj, key, item));
    }

    private IEnumerator WaitAndReactivateObject(GameObject _obj, string _key, ItemController _item)
    {
        yield return new WaitForSeconds(3);

        if (_key == "coin")
        {
            if (_item.index >= 0)
            {
                //สุ่มเกิดที่อื่น
                Vector2 spawnPosition = sc.GetValidSpawnPosition(sc.spawnCoinArea,sc.usedPositions);
                _item.startPosition =spawnPosition;//อย่าลืมไปเปลี่ยนจุดนี้ด้วย (งมตั้งนาน)
                sc.SpawnCoinPos(_obj, spawnPosition); // ส่งตำแหน่งให้ SpawnCoinPos
            }
        }
        else if (_key == "powerUp")
        {
            if (_item.index >= 0 && _item.index < sc.spawnPowerArea.Length)
            {
                //powerUp จะไม่มีการสุ่มตำแหน่งใหม่ เกิดจุดเดิมที่เก็บ
                Vector2 spawnPosition = sc.spawnPowerArea[_item.index].transform.position;
                sc.SpawnPowerUpPos(_obj, spawnPosition); // ส่งตำแหน่งให้ SpawnPowerUpPos
            }
        }
    }


}