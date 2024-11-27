using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int index = 0;
    public float moveSpeed = 1f;
    public float moveHeight = 0.15f;
      public Vector2 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {    
        float newY = Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector2(startPosition.x, startPosition.y + newY);
        
    }
}
