using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountScore : MonoBehaviour
{
    public Text Scoreboard;
    public GameObject ball;

    public int Paddle_1_Score = 0;
    public int Paddle_2_Score = 0;

    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        Scoreboard.text = Paddle_1_Score.ToString() + " - " + Paddle_2_Score.ToString();

        print(Paddle_1_Score + " - " + Paddle_2_Score + " ---" + ball.transform.position.x + ", " + ball.transform.position.y);
    }
}
