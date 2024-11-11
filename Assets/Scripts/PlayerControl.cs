using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        pInfo = GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            float inputX = Input.GetAxis("Horizontal");
            move.x = inputX * speed;

            if (jumpState == JumpState.Grounded && Input.GetKeyDown(KeyCode.Space))
            {
                /*  rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);  หมายเหตุ ที่ไม่ใช้ addForce เพราะมันทำให้ พฤติกรรมการกระโดด 2 ครั้งดูแปลกๆ (ไม่เชื่อลองใช้ดู)*/
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpState = JumpState.Jumping;

            }
            else if (pInfo.doubleJump && Input.GetKeyDown(KeyCode.Space))//ใช้ความกระโดด 2 ครั้ง
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpState = JumpState.Jumping;
                pInfo.DoubleJumpState(false);
            }
        }
        else
        {
            move.x = 0;
        }

        if (rb.velocity.y > 0.1f) // ตัวละครกำลังกระโดดขึ้น
        {
            jumpState = JumpState.Jumping;
        }
        else if (rb.velocity.y < -0.1f) // ตัวละครกำลังตกลง
        {
            jumpState = JumpState.Falling;
        }

        if (rb.velocity.y == 0)
        {
            jumpState = JumpState.Grounded;
        }

        move.y = rb.velocity.y;
        rb.velocity = new Vector2(move.x, move.y);

        Vector2 direction = new Vector2(move.x, move.y);
        FindObjectOfType<PlayerAnimation>().SetDirection(direction, jumpState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
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
}
