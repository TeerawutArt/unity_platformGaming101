using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public double health = 3;
    public double point = 0;
    // Start is called before the first frame update
    void Start()
    {

    }
    public double UpdateHealth(double _health, string _event)
    {

        return health;
    }
    public double UpdatePoint(double _point)
    {
        point += _point;

        return point;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
