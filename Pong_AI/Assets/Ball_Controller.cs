using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class Ball_Controller : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 oldVel;
    public float speed;
    public float offtimer = 10f;
    public float deftimer = 10f;
    public float stucktimer = 10f;
    public float currenttime;
    public string previous_col;
    private float max_timer = 25f;
    public int o_score;
    public int d_score;
    public bool goal_happened;
    public Transform ballspawn;
    private float prev_randz;
    private float prev_randx;
    public Vector3 dir;
    public GameObject o_agent;
    public GameObject d_agent;

    // Start is called before the first frame update
    void Start()

    {
        moveRand();
    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //check to see if the ball is stuck, if so restart
        if (rb.velocity.x == 0 || rb.velocity.z == 0)
        {
            //reset_ball();
        }

        if (stucktimer >= max_timer)
        {
            //restart code
            reset_ball();
        }

        //check to see if the ball hasn't hit a player paddle in some time, if so restart
        currenttime += Time.deltaTime;
        if (currenttime >= max_timer)
        {
            //restart code
            Debug.Log("ball stuck, restarting");
            reset_ball();

        }


        if (offtimer <= 0f || deftimer <= 0f)
        {
            reset_ball();
        }
 
    }

    void reset_ball()
    {
        Debug.Log("RESET BALL CALLED");
        goal_happened = false;//set goal to false
        //rb.velocity = new Vector3(0, 0, 0);//set speed to zero
        transform.position = ballspawn.position;//move to starting location
        //oldVel = rb.velocity;
        currenttime = 0f;
        offtimer = 25f;
        deftimer = 25f;
        stucktimer = 0f;
        speed = Academy.Instance.FloatProperties.GetPropertyWithDefault("speed", 25f);
        moveRand();
    }




    void FixedUpdate()
    {
        //store velocity for ball to be used on collision
        //oldVel = rb.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        //store collision point
        ContactPoint contact = collision.contacts[0];

        //generate new velocity
        //Vector3 reflectVel = Vector3.Reflect(oldVel, contact.normal);

        //apply new velocity
        //rb.velocity = reflectVel;
    }

    void OnTriggerEnter(Collider other)
    {
       
        //if our paddle is just hitting itself over and over, start decrementing the clock
        if (previous_col == "offcol" && other.tag == "off_col_flag")
        {
            offtimer -= 1f;
            currenttime = 0f;
        }

        if (previous_col == "defcol" && other.tag == "def_col_flag")
        {
            deftimer -= 1f;
            currenttime = 0f;
        }

        //if we have a fair hit, reset the timer
        if (previous_col == "offcol"  && other.tag == "def_col_flag")
        {
            //reset the timer when we collide with a paddle
            offtimer = 25f;
            deftimer = 25f;
            currenttime = 0f;
        }

        if (previous_col == "defcol" && other.tag == "off_col_flag")
        {
            //reset the timer when we collide with a paddle
            offtimer = 25f;
            deftimer = 25f;
            currenttime = 0f;
        }


        //set the previous collider flag
        if (other.tag == "def_col_flag")
        {
            previous_col = "defcol";
            currenttime = 0f;
            rb.velocity = rb.velocity * 1.15f;
        }

        if (other.tag == "off_col_flag")
        {
            previous_col = "offcol";
            currenttime = 0f;
            rb.velocity = rb.velocity * 1.15f;
        }

        if (other.tag == "Reflect")
        {
            rb.velocity = rb.velocity * 1.15f;
        }

        

        if (other.tag == "o_net")
        {
            goal_happened = true;
            d_score++;
            //o_agent.GetComponent<O_Agent>().AddReward(-1f);             //Goal is scored on Offensive Agent
            d_agent.GetComponent<D_Agent>().EndEpisode();
            o_agent.GetComponent<O_Agent>().EndEpisode();
            reset_ball();
        }

        if (other.tag =="d_net")
        {
            goal_happened = true;
            o_score++;
            d_agent.GetComponent<D_Agent>().AddReward(-1f);             //Goal Scored against Defensive Agent
            o_agent.GetComponent<O_Agent>().AddReward(1f);            //Goal is scored by Offensive Agent
            d_agent.GetComponent<D_Agent>().EndEpisode();               
            o_agent.GetComponent<O_Agent>().EndEpisode();
            reset_ball();
        }

    }

    public float genRand()
    {
        float rand = Random.Range(-1f, 1f);
        return rand;
    }


    void moveRand()
    {
        float randx = Random.Range(-1.0f, 1.0f);
        float randz = Random.Range(-1.0f, 1.0f);
        float p = Random.Range(-1.0f, 1.0f);
        Debug.Log(randx);
        dir =new Vector3 (randx * speed, 0f, randz * speed);
        // rb.velocity = new Vector3(Random.Range(-1.0f, 1.0f) * speed, 0, Random.Range(-1.0f, 1.0f) * speed);
        rb.velocity = dir;
        Debug.Log(rb.velocity);
    }

}
