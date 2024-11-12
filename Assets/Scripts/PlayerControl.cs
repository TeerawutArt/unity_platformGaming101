using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerInfo pInfo;
    public float speed = 1;
    public float jumpPower = 5;
    public JumpState jumpState = JumpState.Grounded;
    public bool controlEnabled = true;
    private Vector2 move = Vector2.zero;
    private SoundEffect se;
    private float fallStartY; // ตัวแปรเก็บตำแหน่งแกน Y ตอนเริ่มตก
    private bool isWalking = false; // ตัวแปรเก็บสถานะการเดิน
    private float walkThreshold = 0.3f; // กำหนดค่า threshold สำหรับการเริ่มเล่นเสียงเดิน (ทดสอบแล้วมากกว่า 0.3กำลังดี)
    private Vector2 direction = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        se = SoundEffect.ShareInstance;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnDamaged();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        if (controlEnabled)
        {
            float inputX = Input.GetAxis("Horizontal");
            move.x = inputX * speed;

            if (Mathf.Abs(inputX) > walkThreshold && jumpState == JumpState.Grounded) // ตรวจสอบว่ากำลังเดินบนพื้นและความเร็วมากกว่า threshold
            {
                if (!isWalking) // เริ่มเล่นเสียงเดินถ้ายังไม่เล่น
                {
                    se.PlaySoundEffect("walk");
                    isWalking = true;
                }
            }
            else // หยุดเสียงเดินเมื่อหยุดเคลื่อนที่
            {
                if (isWalking)
                {
                    se.StopSoundEffect("walk");
                    isWalking = false;
                }
            }

            if (jumpState == JumpState.Grounded && Input.GetKeyDown(KeyCode.Space))
            {
                se.PlaySoundEffect("jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpState = JumpState.Jumping;
            }
            else if (pInfo.doubleJump && Input.GetKeyDown(KeyCode.Space)) // ใช้ความสามารถกระโดด 2 ครั้ง
            {
                se.PlaySoundEffect("jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpState = JumpState.Jumping;
                pInfo.DoubleJumpState(false);
            }
        }
        else
        {
            move.x = 0;
            if (isWalking)
            {
                se.StopSoundEffect("walk");
                isWalking = false;
            }
        }

        if (rb.velocity.y > 0.1f) // ตัวละครกำลังกระโดดขึ้น
        {
            jumpState = JumpState.Jumping;
        }
        else if (rb.velocity.y < -0.1f) // ตัวละครกำลังตกลง
        {
            if (jumpState != JumpState.Falling) // เริ่มบันทึกระยะการตก
            {
                fallStartY = transform.position.y;
            }
            jumpState = JumpState.Falling;
        }

        if (rb.velocity.y == 0)
        {
            jumpState = JumpState.Grounded;
        }

        move.y = rb.velocity.y;
        rb.velocity = new Vector2(move.x, move.y);

        direction = new Vector2(move.x, move.y);
        FindObjectOfType<PlayerAnimation>().SetDirection(direction, jumpState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // เช็คว่าระยะตกลงมามีอย่างน้อย 0.01 หน่วย (แก้บัคสะดุด platform)
            if (jumpState == JumpState.Falling && Mathf.Abs(transform.position.y - fallStartY) >= 0.01f)
            {
                se.PlaySoundEffect("jumpOnGround");
            }
            jumpState = JumpState.Grounded;
        }
    }

    public void OnCollectingItem(string _item)
    {
        if (_item == "coin")
        {
            pInfo.UpdatePoint(1);
        }
        if (_item == "double jump")
        {
            pInfo.DoubleJumpState(true);
        }
    }

    public void OnDamaged()
    {
        pInfo.TakeDamage(1);
        // หยุดการบังคับ
        controlEnabled = false;

        // ให้ตัวละครกระเด็นไปข้างหลัง
        Vector2 knockback = new Vector2(100, 0); // ทิศทางข้างหลัง 1 หน่วย
        rb.velocity = new Vector2(knockback.x * 20, rb.velocity.y); // ใช้ velocity เพื่อลากตัวละครถอยหลัง

        // เล่นอนิเมชั่นโดนโจมตี
        FindObjectOfType<PlayerAnimation>().SetDamageDirection(direction);

        // เรียก Coroutine ให้ตัวละครกระพริบและกลับมาบังคับได้
        StartCoroutine(HandleDamageEffect());
    }

    private IEnumerator HandleDamageEffect()
    {
        // รอให้กระพริบเสร็จสิ้น
        yield return new WaitForSeconds(2f);

        // หลังจากกระพริบเสร็จแล้ว เปิดการควบคุมกลับ
        controlEnabled = true;
    }
}
