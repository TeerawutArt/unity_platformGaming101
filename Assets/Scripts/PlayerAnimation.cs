using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer; // ใช้สำหรับการกระพริบ
    public string[] idleDirections = { "Idle_right", "Idle_left" };
    public string[] runDirections = { "Run_right", "Run_left" };
    public string[] jumpDirections = { "Jump_right", "Jump_left" };
    public string[] fallingDirections = { "Jump_Fall_right", "Jump_Fall_left" };
    public string[] damagedDirections = { "Damaged_right", "Damaged_left" };
    int lastDirection;
    bool damaged = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // รับการอ้างอิง SpriteRenderer
    }

    // ฟังก์ชันที่ใช้ในการตั้งค่าทิศทางของผู้เล่น
    public void SetDirection(Vector2 _direction, JumpState _jumpState)
    {
        string[] directionArray = null;

        if (_direction.magnitude < 0.01 && _jumpState == JumpState.Grounded && !damaged)
        {
            directionArray = idleDirections;
        }
        else if (_jumpState == JumpState.Grounded && !damaged)
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(_direction);
        }
        else if (_jumpState == JumpState.Jumping && !damaged)
        {
            directionArray = jumpDirections;
            lastDirection = DirectionToIndex(_direction);
        }
        else if (!damaged)
        {
            directionArray = fallingDirections;
            lastDirection = DirectionToIndex(_direction);
        }
        else
        {
            return;
        }

        anim.Play(directionArray[lastDirection]);
    }

    // ฟังก์ชันใหม่สำหรับการกระพริบเมื่อโดนโจมตี
    public void SetDamageDirection(Vector2 _direction)
    {
        damaged = true;
        if (damaged)
        {
            string[] directionArray = damagedDirections; // เลือกอนิเมชั่นการโดนโจมตี
            lastDirection = DirectionToIndex(_direction); // กำหนดทิศทางที่โดนโจมตี
            anim.Play(directionArray[lastDirection]);
            StartCoroutine(HandleDamageEffect()); // เรียก Coroutine สำหรับกระพริบ
        }
    }

    // ฟังก์ชันสำหรับคำนวณทิศทาง (ซ้าย/ขวา)
    private int DirectionToIndex(Vector2 _direction)
    {
        if (_direction.x > 0)
        {
            lastDirection = 0; // ขวา
        }
        else if (_direction.x < 0)
        {
            lastDirection = 1; // ซ้าย
        }

        return lastDirection;
    }

    // Coroutine สำหรับการกระพริบ
    private IEnumerator HandleDamageEffect()
    {
        float blinkDuration = 2f; // ระยะเวลาการกระพริบ
        float blinkInterval = 0.1f; // เวลาระหว่างการกระพริบแต่ละครั้ง

        float timeElapsed = 0f;
        bool isVisible = true;

        // กระพริบ sprite เป็นเวลา 2 วินาที
        while (timeElapsed < blinkDuration)
        {
            spriteRenderer.enabled = isVisible; // สลับการแสดงของ sprite
            isVisible = !isVisible; // เปลี่ยนสถานะการแสดงผล
            timeElapsed += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        // หลังจากกระพริบเสร็จแล้ว ให้แสดงผลตามปกติ
        spriteRenderer.enabled = true;
        damaged = false;

        // กลับไปที่ท่าทาง Idle ในทิศทางล่าสุด
        SetDirection(Vector2.zero, JumpState.Grounded);
    }
}
