using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    public BoxCollider2D spawnArea;
    private int coinAmount;
    private int powerUpAmount;
    private readonly int maxAttempts = 10;

    void Start()
    {
        StartCoroutine(WaitForPoolAndSpawn());
    }

    IEnumerator WaitForPoolAndSpawn()
    {
        while (!ObjectPooling.SharedInstance.isInitialized)
        {
            yield return null;
        }

        coinAmount = ObjectPooling.SharedInstance.coinAmountToPool;
        powerUpAmount = ObjectPooling.SharedInstance.powerUpAmountToPool;
        SpawnItems();
    }

    void SpawnItems()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coin = ObjectPooling.SharedInstance.GetPooledObject("coin");
            if (coin != null)
            {
                RandomSpawnItem(coin);
            }
        }
        for (int i = 0; i < powerUpAmount; i++)
        {
            GameObject powerUp = ObjectPooling.SharedInstance.GetPooledObject("powerUp");
            if (powerUp != null)
            {
                RandomSpawnItem(powerUp);
            }
        }
    }

    public void RandomSpawnItem(GameObject _gameObject)
    {
        Vector2 spawnPosition;
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            spawnPosition = RandomPos();
            //check pos ที่จะเกิดดูก่อน (ใช่ถ้าวนลูปครบแล้วมันยังทับกันอยู่ ก็ช่างมันละวู้)
            if (IsPositionFree(spawnPosition, 0.5f))
            {
                _gameObject.transform.position = spawnPosition;
                _gameObject.SetActive(true);
                return;
            }
        }
    }

    public Vector2 RandomPos()
    {
        float randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        float randomY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        return new Vector2(randomX, randomY);
    }

    public bool IsPositionFree(Vector2 pos, float size)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, new Vector2(size, size), 0);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Ground") || collider.CompareTag("coin"))
            {
                return false;
            }
        }
        return true;
    }
}