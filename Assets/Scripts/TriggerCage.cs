using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCage : MonoBehaviour
{
    public float fallSpeed = 5f; 

    private bool cageFall = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cageFall = true; 
        }
    }

    private void Update()
    {
        if (cageFall)
        {
            foreach (Transform child in transform)
            {
                Vector2 currentPosition = child.position;
                Vector2 targetPosition = new Vector2(currentPosition.x, currentPosition.y - fallSpeed * Time.deltaTime);
                child.position = targetPosition;
            }
        }
    }



}