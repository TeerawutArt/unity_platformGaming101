using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    private UIController ui;
    // Start is called before the first frame update
    void Start()
    {
        ui = UIController.SharedInstance;
        
    }

void OnTriggerEnter2D(Collider2D other)
{
    var player = other.gameObject.GetComponent<PlayerControl>();
    if(player==null) return;
    Time.timeScale = 0f; //หยุดเกม
    ui.newGame = true;
    ui.OnPauseGame();
    Debug.Log("จบเกม"); 
    
}

}
