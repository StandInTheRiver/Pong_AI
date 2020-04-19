using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Paddle_Controller : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public bool isTraining;
    public GameObject o_agent;
    public D_Agent d_agentscript;
    public O_Agent o_agentscript;

    // Start is called before the first frame update
    void Start()
    {

    }

    //depricated code block ,used to be used to apply rewards but was utilized elsewhere
   /* private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            if ((isTraining) && (rb.name == "Defensive_Paddle"))     //If training is active
            {
                o_agent.GetComponent<D_Agent>().AddReward(1f);                     //Adds a reward for stopping the ball
                o_agent.GetComponent<D_Agent>().EndEpisode();                      //Ends the session because it has been successful
            }
            if ((isTraining) && (rb.name == "Offensive_Paddle"))     //If training is active
            {
                o_agent.GetComponent<O_Agent>().AddReward(0.25f);                     //Adds a small reward for stopping the ball
                o_agent.GetComponent<O_Agent>().EndEpisode();                      //Ends the session because it has been successful
            }
        }

    } */




    // Update is called once per frame
    void FixedUpdate()
    {
        //gets the input from the player and moves the paddle accordingly
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector3(0, 0, speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector3(0, 0, -speed);
        }

    }




}
