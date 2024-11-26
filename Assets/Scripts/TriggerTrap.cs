using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {        
    var player = other.gameObject.GetComponent<PlayerControl>();
    if(player==null) return;
    //เปิดใช้งานกับดัก
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
