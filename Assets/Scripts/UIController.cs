using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//จริงๆควรรวม ui ทั้งหมดไว้ในนี้(healthBar,scoreBar,powerUp)แล้วเขียนโค้ดจัดการทั้งหมดในนี้ แต่ช่างมันทำไปเยอะแล้ว
public class UIController : MonoBehaviour
{
    public UnityEngine.UI.Image PowerUp;
    public static UIController SharedInstance;
    // Start is called before the first frame update
    void Awake()
    {
        SharedInstance = this;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdatePowerUp(bool state)
    {


        PowerUp.enabled = state;


    }
}
