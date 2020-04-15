using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Bumper2Agent : Agent
{
    public float speed;             //may not need these...
    public Ball ball;
    public Vector3 direction;
    private PongArea pongArea;
    new private Rigidbody rigidbody;
    public bool isTraining;
    public Bumper1Agent bumper1;

    void Awake()
    {
        transform.localScale = new Vector3(3, 1, 0.5f);
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnActionReceived(float[] vectorAction) //Available actions Stay still, Move up, and Move down
    {
        switch ((int)vectorAction[0])
        {
            case 0:
                if (isTraining)     //If training is active
                {
                    AddReward(0.001f);                        //small reward for staying still
                }
                break;
            case 1: //Move up
                transform.position += new Vector3(0, Time.deltaTime * speed, 0);
                break;
            case 2://Move Down
                transform.position -= new Vector3(0, Time.deltaTime * speed, 0);
                break;
        }

        if (ball.transform.position.x > ball.borders.right)            //Goal is scored against bumper 2
        {
            //AddReward(-1f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            if (isTraining)     //If training is active
            {
                FindObjectOfType<Bumper2Agent>().AddReward(1f);                     //Adds a reward for stopping the ball
                FindObjectOfType<Bumper2Agent>().EndEpisode();                      //Ends the session because it has been successful
            }
        }

    }
    public override float[] Heuristic()
    {
        var Action = 0f;
        if (!isTraining)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // move up
                Action = 1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                // move down
                Action = 2f;
            }
        }

        return new float[] { Action };
    }

    public void SetResetParameters()           //Reset the agent and area
    {
        pongArea.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ball.direction.x);
        sensor.AddObservation(ball.direction.y);
        sensor.AddObservation(ball.transform.position.x / ball.borders.right);      //Ball position x
        sensor.AddObservation(ball.transform.position.y / ball.borders.left);       //Ball position x
        sensor.AddObservation(ball.speed);        // /30f???
        sensor.AddObservation(transform.position.y / (ball.borders.top - transform.localScale.y / 2));      //Bumper position

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
}