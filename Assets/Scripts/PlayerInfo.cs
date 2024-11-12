using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    private HealthBar hb;
    private ScoreBar sb;
    private UIController uic;
    public float health;
    public float maxHp = 3;

    public float point = 0;
    public bool doubleJump = false;
    // Start is called before the first frame update

    void Start()
    {
        health = maxHp;
        hb = HealthBar.SharedInstance;
        sb = ScoreBar.SharedInstance;
        uic = UIController.SharedInstance;
    }
    public float UpdateHealth(float _health, string _event)
    {

        return health;
    }
    public float UpdatePoint(float _point)
    {
        point += _point;
        sb.AddScore(point);
        return point;
    }

    // Update is called once per frame
    void Update()
    {


    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        hb.UpdateHealthBar(health, maxHp);


    }
    public void Heal(float healingAmount)
    {
        health += healingAmount;
        if (health >= maxHp)
        {
            health = maxHp;
        }
        hb.UpdateHealthBar(health, maxHp);
    }
    public void DoubleJumpState(bool _state)
    {
        if (_state == true) doubleJump = true;

        else if (_state == false) doubleJump = false;

        uic.UpdatePowerUp(_state);
    }
}
