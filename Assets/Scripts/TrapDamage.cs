using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
    var player = other.gameObject.GetComponent<PlayerControl>();
    if(player==null) return;
        player.TakeDamaged(1);
    }
}
