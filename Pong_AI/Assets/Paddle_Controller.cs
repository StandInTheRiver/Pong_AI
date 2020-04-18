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

    // Start is called before the first frame update
    void Start()
    {
         
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            if ((isTraining) && (rb.name == "Defensive_Paddle"))     //If training is active
            {
                o_agent.GetComponent<O_Agent>().AddReward(1f);                     //Adds a reward for stopping the ball
                o_agent.GetComponent<O_Agent>().EndEpisode();                      //Ends the session because it has been successful
            }
            if ((isTraining) && (rb.name == "Offensive_Paddle"))     //If training is active
            {
                o_agent.GetComponent<O_Agent>().AddReward(0.25f);                     //Adds a small reward for stopping the ball
                o_agent.GetComponent<O_Agent>().EndEpisode();                      //Ends the session because it has been successful
            }
        }

    }



    void Update()
    {
        if(isTraining)
        {
            float paddleSize = Academy.Instance.FloatProperties.GetPropertyWithDefault("Paddle_Size", 1f);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z*paddleSize);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
