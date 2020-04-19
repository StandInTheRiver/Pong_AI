using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
using TMPro;

public class o_agent_vs : Agent
{
    public GameObject ball;
    public GameObject paddle;
    public Paddle_Controller paddlescript;
    public TextMeshPro Red_Score;
    //public Ball_Controller redscoretext;
    public vs_ball_controller redscoretext;
    //public offensive_ball_controller redscoretext;
    public Rigidbody paddlerb;
    public bool defensive_paddle_mode;

    //comments for the code can be found in the same as D_Agent.cs, these scripts are very similar
    // Start is called before the first frame update
    void Start()
    {
        Red_Score.text = "0";

        redscoretext = ball.GetComponent<vs_ball_controller>();
        paddlerb = paddle.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (defensive_paddle_mode == true)
        {
            Red_Score.text = redscoretext.d_score.ToString("00"); //update the score text 
        }

        if (defensive_paddle_mode == false)
        {
            Red_Score.text = redscoretext.o_score.ToString("00"); //update the score text 
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

    public override void OnActionReceived(float[] vectorAction) //Available actions Stay still, Move up, and Move down
    {
        if (vectorAction[0] > 0)
        {
            paddlerb.velocity = new Vector3(0, 0, paddlescript.speed);
        }
        if (vectorAction[0] < 0)
        {
            paddlerb.velocity = new Vector3(0, 0, -paddlescript.speed);
        }

        if (redscoretext.goal_happened == true)            //Goal is scored against bumper 2
        {
            //AddReward(-1f);
            EndEpisode();
        }
    }



    /* public override void CollectObservations(VectorSensor sensor)
     {
         sensor.AddObservation(ball.GetComponent<Transform>().position); //xyz postion
         sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity); //xyz velocity  
         sensor.AddObservation(this.GetComponent<Transform>().position); // my own position      
     } */

    /*  public override void OnEpisodeBegin()
      {
          if (paddle.GetComponent<Paddle_Controller>().isTraining)
          {
              float paddleSize = Academy.Instance.FloatProperties.GetPropertyWithDefault("Paddle_Size", 1f);
              paddle.GetComponent<Transform>().transform.localScale = new Vector3(paddle.GetComponent<Transform>().transform.localScale.x, paddle.GetComponent<Transform>().transform.localScale.y, paddleSize);
          }
      } */

}
