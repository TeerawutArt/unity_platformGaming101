using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

void OnTriggerEnter2D(Collider2D other)
{
    var player = other.gameObject.GetComponent<PlayerControl>();
    if(player==null) return;
    Time.timeScale = 0f; //หยุดเกม
    Debug.Log("จบเกม"); //หา ui จบเกมก่อน
    
}

}
