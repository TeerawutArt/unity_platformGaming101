using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    public BoxCollider2D spawnArea;  // ขอบเขตที่เหรียญจะสุ่มเกิด
    private int coinAmount;


    void Start()
    {

        StartCoroutine(WaitForPoolAndSpawn()); // เรียก Coroutine ที่จะรอให้ pool พร้อมก่อนสร้างเหรียญ
    }

    IEnumerator WaitForPoolAndSpawn()
    {
        // รอจนกว่า pool จะพร้อมใช้งาน
        while (!ObjectPooling.SharedInstance.isInitialized)  // รอจนกว่า pool จะพร้อมใช้งาน
        {
            yield return null; // รอ 1 frame แล้วกลับไปเช็คใหม่ (yield return ค่อยข้างซับซ้อนรายละเอียดเยอะ ถ้างงหาอ่านเอานะครับ)
        }

        // เมื่อพร้อมแล้ว ให้เรียก SpawnCoins()
        coinAmount = ObjectPooling.SharedInstance.amountToPool;
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coin = ObjectPooling.SharedInstance.GetPooledObject();
            if (coin != null)
            {
                RandomSpawnCoin(coin);
            }
        }
    }
    public void RandomSpawnCoin(GameObject coin)
    {
        // สุ่มตำแหน่งภายในขอบเขตของ BoxCollider2D
        Vector2 spawnPosition = RandomPos();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, 0.5f);  // ตรวจสอบการทับซ้อน
        if (colliders.Length > 0)
        {
            Debug.Log("coin Stuck");

        }
        coin.transform.position = spawnPosition;
        coin.SetActive(true);
    }
    public Vector2 RandomPos()
    {
        float randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);  // สุ่ม X ในช่วงขอบเขต
        float randomY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);  // สุ่ม Y ในช่วงขอบเขต
        return new Vector2(randomX, randomY);
    }

}
