using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using TMPro;

public class d_agent_vs : Agent
{
    public GameObject ball;
    public GameObject paddle;
    public Paddle_Controller paddlescript;
    public TextMeshPro Blue_Score;
   // public Ball_Controller bluescoretext;
    public vs_ball_controller bluescoretext;
    public Rigidbody paddlerb;
    public bool defensive_paddle_mode;

    // Start is called before the first frame update
    void Start()
    {
        Blue_Score.text = "0"; //set the score text
        bluescoretext = ball.GetComponent<vs_ball_controller>(); //grab components to cache
        paddlerb = paddle.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (defensive_paddle_mode == true)
        {
            Blue_Score.text = bluescoretext.d_score.ToString("00"); //update the score text 
        }

        if (defensive_paddle_mode == false)
        {
            Blue_Score.text = bluescoretext.o_score.ToString("00"); //update the score text 
        }


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

    public override void OnActionReceived(float[] vectorAction) //Available actions , Move up, and Move down
    {
        //using coninuous movement our paddle can either move up or down depending on the action received from mlagents
        if (vectorAction[0] > 0)
        {
            paddlerb.velocity = new Vector3(0, 0, paddlescript.speed);
        }
        if (vectorAction[0] < 0)
        {
            paddlerb.velocity = new Vector3(0, 0, -paddlescript.speed);
        }

        //catch to end episode in the e vent that a goal is scored
        if (bluescoretext.goal_happened == true)            //Goal is scored against bumper 2
        {
            //AddReward(-1f);
            EndEpisode();
        }


    }


    //obsolete code, we were originally using many sensors to train the pong paddles but the training time would take weeks on my computer
    /*  public override void CollectObservations(VectorSensor sensor)
      {
          sensor.AddObservation(ball.GetComponent<Transform>().position); //xyz postion
          sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity); //xyz velocity  
          sensor.AddObservation(this.GetComponent<Transform>().position); // my own position      
      } */

    //depricated code, used to be used to adjust the size of hte paddle dynamically as the ai learned and prog ressed through the curriculum
    /*  public override void OnEpisodeBegin()
      {
          if (paddle.GetComponent<Paddle_Controller>().isTraining)
          {
              float paddleSize = Academy.Instance.FloatProperties.GetPropertyWithDefault("Paddle_Size", 1f);
              paddle.GetComponent<Transform>().transform.localScale = new Vector3(paddle.GetComponent<Transform>().transform.localScale.x, paddle.GetComponent<Transform>().transform.localScale.y, paddleSize);
          }
      } */


}