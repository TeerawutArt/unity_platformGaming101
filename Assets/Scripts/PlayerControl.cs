using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerInfo pInfo;
    public float speed = 1;
    public float jumpPower = 5;
    public float knockBackPower = 3;
    public JumpState jumpState = JumpState.Grounded;
    public bool controlEnabled = true;
    private Vector2 move = Vector2.zero;
    private SoundEffect se;
    private float fallStartY; // ตัวแปรเก็บตำแหน่งแกน Y ตอนเริ่มตก
    private bool isWalking = false; // ตัวแปรเก็บสถานะการเดิน
    private float walkThreshold = 0.3f; // กำหนดค่า threshold สำหรับการเริ่มเล่นเสียงเดิน (ทดสอบแล้วมากกว่า 0.3กำลังดี)
    private Vector2 direction = Vector2.zero;
    private PlayerAnimation pa;
    private bool damaged = false;
    private UIController ui;

    void Start()
    {
        ui = UIController.SharedInstance;
        se = SoundEffect.ShareInstance;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pInfo = GetComponent<PlayerInfo>();
        pa = GetComponentInChildren<PlayerAnimation>();

    }

    // Update is called once per frame
    void Update()
    {

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


            //check state ของตัวละคร
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
            pa.SetDirection(direction, jumpState);
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

    public void TakeDamaged(int damage)
    {
        if (damaged) return;
        pInfo.TakeDamage(damage);
        damaged = true;
        controlEnabled = false;
        if(pInfo.health>0){
        float knockbackDirection = pa.lastDirection == 0 ? -1 : 1;
        Vector2 knockback = new Vector2(knockbackDirection * knockBackPower, rb.velocity.y);
        rb.velocity = knockback;
        pa.SetDamageDirection(direction);
        StartCoroutine(HandleDamageEffect(1));
        }else{
            pa.SetDeadDirection(direction);
            StartCoroutine(LoseGame(1));
        }

    }

    private IEnumerator HandleDamageEffect(float waitTime)
    {
        // รอให้กระพริบเสร็จสิ้น
        yield return new WaitForSeconds(waitTime);
        // หลังจากกระพริบเสร็จแล้ว เปิดการควบคุมกลับ
        controlEnabled = true;
        damaged = false;
    }
        private IEnumerator LoseGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ui.OnPauseGame();
        
    }
}
