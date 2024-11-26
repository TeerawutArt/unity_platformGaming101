using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour{
public float fallDelay = 3f;  // เวลาที่ต้องยืนบน platform ก่อนที่จะตก
    public float fallSpeed = 5f;  // ความเร็วในการตกของ platform
    public float resetDelay = 5f; // เวลาหลังจากที่ platform ล่วงจะกลับไปที่เดิม

    private bool isPlayerOnPlatform = false;
    private float timer = 0f;

    private Vector2 originalPosition;  // ตำแหน่งเริ่มต้นของ platform
    private Vector2 targetPosition;    // ตำแหน่งที่ platform จะตกไป

    private bool isFalling = false;  // ตรวจสอบสถานะการตกของ platform
    private bool isResetting = false; // ตรวจสอบว่ากำลังรีเซ็ตตำแหน่งอยู่หรือไม่

    void Start()
    {
        originalPosition = transform.position;  // จัดเก็บตำแหน่งเริ่มต้น
        targetPosition = new Vector2(originalPosition.x, originalPosition.y - 5f);  // ปรับระยะตก (ห่างจากตำแหน่งเริ่มต้น)
    }

    void Update()
    {
        if (isPlayerOnPlatform)
        {
            timer += Time.deltaTime;  // เพิ่มเวลาที่อยู่บน platform

            if (timer >= fallDelay && !isFalling && !isResetting)  // ถ้าผู้เล่นยืนบน platform นานเกินที่กำหนด
            {
                isFalling = true;  
            }
        }

        if (isFalling)
        {
            // ใช้ MoveTowards เพื่อเคลื่อนที่ platform ลง
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);

            if ((Vector2)transform.position == targetPosition)
            {
                isFalling = false;
                StartCoroutine(ResetPlatform());  // รอเวลาแล้วกลับไปที่เดิม
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))  // ตรวจสอบว่าเป็น Player ที่ยืนอยู่บน platform
        {
            isPlayerOnPlatform = true;
        }
    }


    IEnumerator ResetPlatform()
    {
        isResetting = true;  
        yield return new WaitForSeconds(resetDelay);  
        transform.position = originalPosition;  
        isResetting = false;
        timer = 0f;
    }
}
