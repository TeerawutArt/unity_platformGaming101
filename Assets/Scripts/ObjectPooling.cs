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
    public int coinAmountToPool = 10;
    public int powerUpAmountToPool = 5;
    public bool isInitialized = false;
    public SpawnObj sc;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        sc = GetComponent<SpawnObj>();
        pooledObjects = new Dictionary<string, List<GameObject>>();
        InitializePool("coin", coinPrefab, coinAmountToPool);
        InitializePool("powerUp", powerUpPrefab, powerUpAmountToPool);
        isInitialized = true;
    }

    // Method to initialize pools for each object type
    private void InitializePool(string key, GameObject prefab, int amount)
    {
        List<GameObject> objectPool = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject tmp = Instantiate(prefab, collectableParent.transform);
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

    public void ResetObject(GameObject obj, string key)
    {
        StartCoroutine(WaitAndReactivateObject(obj, key));
    }

    private IEnumerator WaitAndReactivateObject(GameObject obj, string key)
    {
        yield return new WaitForSeconds(3);
        if (key == "coin")
        {
            sc.RandomSpawnItem(obj);
        }
        else if (key == "powerUp")
        {
            // Implement custom logic for spawning power-ups
            sc.RandomSpawnItem(obj);
        }
    }


}
