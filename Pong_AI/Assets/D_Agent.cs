using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using TMPro;

public class D_Agent : Agent
{
    public GameObject ball;
    public GameObject paddle;
    public Paddle_Controller paddlescript;
    public TextMeshPro Blue_Score;
    // Start is called before the first frame update
    void Start()
    {
        Blue_Score.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        Blue_Score.text = ball.GetComponent<Ball_Controller>().d_score.ToString("00");
    }
    private void FixedUpdate()
    {
        // Request a decision every 5 steps. RequestDecision() automatically calls RequestAction(),
        // but for the steps in between, we need to call it explicitly to take action using the results
        // of the previous decision
        if (StepCount % 5 == 0)
        {
            RequestDecision();
        }
        else
        {
            RequestAction();
        }
    }

    public override void OnActionReceived(float[] vectorAction) //Available actions Stay still, Move up, and Move down
    {
        switch ((int)vectorAction[0])
        {
            case 0:
                if (paddlescript.isTraining)     //If training is active
                {
                    AddReward(0.001f);                        //small reward for staying still
                }
                break;
            case 1: //Move up
                paddle.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, paddlescript.speed);
                break;
            case 2://Move Down
                paddle.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -paddlescript.speed);
                break;
        }

        if (ball.GetComponent<Ball_Controller>().goal_happened == true)            //Goal is scored against bumper 2
        {
            //AddReward(-1f);
            EndEpisode();
        }
    }



    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ball.GetComponent<Transform>().position); //xyz postion
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity); //xyz velocity  
        sensor.AddObservation(this.GetComponent<Transform>().position); // my own position      
    }
}
