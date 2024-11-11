using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    public string[] idleDirections = { "Idle_right", "Idle_left" };
    public string[] runDirections = { "Run_right", "Run_left" };
    public string[] jumpDirections = { "Jump_right", "Jump_left" };
    public string[] fallingDirections = { "Jump_Fall_right", "Jump_Fall_left" };
    int lastDirection;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void SetDirection(Vector2 _direction, JumpState _jumpState)
    {
        string[] directionArray = null;
        if (_direction.magnitude < 0.01 && _jumpState == JumpState.Grounded)
        {
            directionArray = idleDirections;
        }
        //ไม่ใช้ switch case เพราะ เกลียดมัน
        else if (_jumpState == JumpState.Grounded)
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(_direction);
        }
        else if (_jumpState == JumpState.Jumping)
        {
            directionArray = jumpDirections;
            lastDirection = DirectionToIndex(_direction);

        }
        else
        {
            directionArray = fallingDirections;
            lastDirection = DirectionToIndex(_direction);
        } //falling
        anim.Play(directionArray[lastDirection]);

    }


    private int DirectionToIndex(Vector2 _direction)
    {

        if (_direction.x > 0)
        {
            lastDirection = 0; // ขวา
        }
        else if (_direction.x < 0)
        {
            lastDirection = 1; // ซ้าย
        }

        return lastDirection;
    }

}
