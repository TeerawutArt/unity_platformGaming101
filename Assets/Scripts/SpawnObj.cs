using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObj : MonoBehaviour
{
    public BoxCollider2D[] spawnCoinArea;
    public BoxCollider2D[] spawnPowerArea;
    public float minimumCoinOffset = 1.5f; // ระยะห่างขั้นต่ำระหว่างเหรียญ
    private int coinAmount;
    private int powerUpAmount;
    private int randomIndex;
    private BoxCollider2D selectedBox;
    public List<Vector2> usedPositions = new List<Vector2>();

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

    public void SpawnItems()
    {
        // Spawn coins
        for (int i = 0; i < coinAmount; i++)
        {
            GameObject coin = ObjectPooling.SharedInstance.GetPooledObject("coin");
            if (coin != null)
            {
                Vector2 spawnPosition = GetValidSpawnPosition(spawnCoinArea, usedPositions);
                if (spawnPosition != Vector2.zero)
                {
                    SpawnCoinPos(coin, spawnPosition);
                }
            }
        }
        for (int i = 0; i < powerUpAmount; i++)
        {
            GameObject powerUp = ObjectPooling.SharedInstance.GetPooledObject("powerUp");
            if (powerUp != null && i < spawnPowerArea.Length) 
            {
                SpawnPowerUpPos(powerUp, spawnPowerArea[i].transform.position);
            }
        }
    }



    public Vector2 GetValidSpawnPosition(BoxCollider2D[] spawnAreas, List<Vector2> usedPositions)
    {
        // พยายามสุ่มตำแหน่งใหม่ในพื้นที่ที่เลือก
        for (int attempt = 0; attempt < 10; attempt++) // ลองสุ่มไม่เกิน 10 ครั้ง
        {
            randomIndex = Random.Range(0, spawnAreas.Length);
            selectedBox = spawnAreas[randomIndex];
            Vector2 randomPosition = GetRandomPositionInBox(selectedBox);

            // ตรวจสอบว่าตำแหน่งในแกน x ไม่ใกล้กับตำแหน่งที่ใช้ไปแล้ว
            bool isValid = true;
            foreach (Vector2 usedPosition in usedPositions)
            {
                if (Mathf.Abs(randomPosition.x - usedPosition.x) < minimumCoinOffset) // เช็คเฉพาะแกน x
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                usedPositions.Add(randomPosition); // บันทึกตำแหน่งที่ใช้
                return randomPosition;
            }
        }

        // ถ้าหาตำแหน่งที่เหมาะสมไม่ได้ ให้คืนค่า Vector2.zero
      return Vector2.zero; 
    }

    private Vector2 GetRandomPositionInBox(BoxCollider2D box)
    {
        float randomX = Random.Range(box.bounds.min.x, box.bounds.max.x); //สุ่มในขอบเขตทั้งหมดของ box
        float randomY = Random.Range(box.bounds.min.y, box.bounds.max.y);
        return new Vector2(randomX, randomY);
    }

    public void SpawnCoinPos(GameObject _gameObject, Vector2 _spawnPosition)
    {
    _gameObject.SetActive(true); // เปิดใช้งาน GameObject
    _gameObject.transform.position = _spawnPosition; // ตั้งค่าตำแหน่งใหม่
  
    }

    public void SpawnPowerUpPos(GameObject _gameObject, Vector2 _spawnPosition)
    {

        _gameObject.transform.position = _spawnPosition;
        _gameObject.SetActive(true);
    }
}