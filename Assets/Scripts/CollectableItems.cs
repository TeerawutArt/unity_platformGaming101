using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItems : MonoBehaviour
{

    private SoundEffect se;
    private ObjectPooling op;

    // Start is called before the first frame update

    void Start()
    {
        op = ObjectPooling.SharedInstance;
        se = SoundEffect.ShareInstance;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerControl>();

        if (player != null)
        {
            se.PlaySoundEffect("collectSound");
            //เหรียญ
            if (gameObject.CompareTag("coin"))
            {
                player.OnCollectingItem("coin");
                gameObject.SetActive(false);

            }
            //power up double jump
            if (gameObject.CompareTag("double_jump"))
            {
                ItemController doubleJump = gameObject.GetComponent<ItemController>();
                player.OnCollectingItem("double jump");
                gameObject.SetActive(false);
                op.ResetObject(gameObject,"powerUp",doubleJump.index);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
