using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public UnityEngine.UI.Image healthBar;
    public static HealthBar SharedInstance;

    // Start is called before the first frame update
    void Start()
    {
        SharedInstance = this;


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateHealthBar(float health, float maxHp)
    {

        healthBar.fillAmount = health / maxHp;



    }
}
