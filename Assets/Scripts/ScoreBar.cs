using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    public static ScoreBar SharedInstance;
    private float score = 0f;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        SharedInstance = this;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddScore(float points)
    {

        score = points;
        scoreText.text = score.ToString();
    }

}
