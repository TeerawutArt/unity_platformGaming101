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
        se = gameObject.GetComponentInParent<SoundEffect>();

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerControl>();

        if (player != null)
        {
            if (gameObject.CompareTag("coin"))
            {
                player.OnCollectingItem("coin");
                // เล่นเสียงเอฟเฟกต์
                se.PlayCollectSoundEffect();
                gameObject.SetActive(false);
                op.ResetCoin(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
