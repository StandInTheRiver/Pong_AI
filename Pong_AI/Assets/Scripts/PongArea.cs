using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class PongArea : MonoBehaviour 
{
    public Bumper1Agent bumper1Agent;                   //The agent that controls the Bumpers
    public Bumper2Agent bumper2Agent;
    public TextMeshPro cumulativeRewardText_Bump1;            //Cumulative reward displayed
    public TextMeshPro cumulativeRewardText_Bump2;
    public Ball ball;
    public int Paddle_1_Score;
    public int Paddle_2_Score;
    
    public void ResetArea()
    {
        PlaceBumper1();                                 //Resets Bumper 1 to original position
        PlaceBumper2();                                 //Resets Bumper 2 to original position
        SpawnBall();                                    //Spawns the ball in a random location and direction
    }

    public void SpawnBall()
    {
        float startX = Random.Range(0, 2) == 0 ? -1 : 1;
        float startY = Random.Range(0, 2) == 0 ? -1 : 1;

        ball.direction = new Vector3(startX, startY, 0);
        return;
    }

    public void PlaceBumper1()      //Places bumper 1 in the starting position
    {
        Rigidbody rigidbody = bumper1Agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        bumper1Agent.transform.SetPositionAndRotation(new Vector3(-12,0,0), Quaternion.Euler(new Vector3(0,90,90)));
        return;
    }

    public void PlaceBumper2()      //Places bumper 2 in the starting position
    {
        Rigidbody rigidbody = bumper2Agent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        bumper2Agent.transform.SetPositionAndRotation(new Vector3(12, 0, 0), Quaternion.Euler(new Vector3(0,90,90)));
        return;
    }

    private void Start()
    {
        ResetArea();
        GameObject.Find("PongManager").GetComponent<CountScore>().Paddle_2_Score = 0;
        GameObject.Find("PongManager").GetComponent<CountScore>().Paddle_1_Score = 0;
    }

    private void Update()
    {
        cumulativeRewardText_Bump1.text = bumper1Agent.GetCumulativeReward().ToString("0.00");        //Displays the reward for Bumper 1     Can be removed...
        cumulativeRewardText_Bump2.text = bumper2Agent.GetCumulativeReward().ToString("0.00");        //Displays the reward for Bumper 2
        if (ball.transform.position.x >= 16f)
        {
            GameObject.Find("PongManager").GetComponent<CountScore>().Paddle_1_Score++;
            if (bumper2Agent.isTraining)     //If training is active
            {
                FindObjectOfType<Bumper2Agent>().AddReward(-1f);
                FindObjectOfType<Bumper2Agent>().EndEpisode();
            }
        }
        if (ball.transform.position.x <= -16f)
        {
            GameObject.Find("PongManager").GetComponent<CountScore>().Paddle_2_Score++;
            if (bumper1Agent.isTraining)     //If training is active
            {
                FindObjectOfType<Bumper1Agent>().AddReward(-1f);
                FindObjectOfType<Bumper1Agent>().EndEpisode();
            }
        }
    }

}
