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
    private D_Agent d_agentscript;//change to O agent for O training
    private D_Agent o_agentscript;//

    // Start is called before the first frame update
    void Start()

    {
        
        moveRand(); //we call moveRand once on start to get the ball moving initially
        Invoke("freeze",1.5f); // we use invoke to call the function freeze once after 1.5 seconds, this is to prevent the ball from flying out of the game area


        //for both the two below blocks of code, we are grabbing the objects D_agent script and cacheing it so we can reference its values rather than getting it each update

        //[[Offensive]] - uncomment to train offensive
        //d_agentscript = d_agent.GetComponent<O_Agent>();
        //o_agentscript = o_agent.GetComponent<O_Agent>();


        //[[Defensive]] - uncomment to train defensive
        d_agentscript = d_agent.GetComponent<D_Agent>();
        o_agentscript = o_agent.GetComponent<D_Agent>();
    }


    //awake is called once before start
    void Awake()
    {

    }

    //function to freeze the balls yaxis to prevent it from flying outside the game area ( which the ai discovered how to do on early versions )
    void freeze()
    {
        //set the ridgid body constants to freeze y
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }


    //function to unfreeze the ball allowing it to land on the game area
    void unfreeze()
    {
        //set the constaints to none
        rb.constraints = RigidbodyConstraints.None;
    }

    // Update is called once per frame
    void Update()
    {
        //check to see if the ball is stuck, if so restart
        if (rb.velocity.x == 0 || rb.velocity.z == 0)
        {
            //depricated, the ball should no longer ever stand still
            //reset_ball();
        }

        //if the stucktimer exeeds  its limit, the ball is stuck and we reset, this is a safety catch
        if (stucktimer >= max_timer)
        {
            //restart code
            reset_ball();
        }

        //check to see if the ball hasn't hit a player paddle in some time, if so restart
        currenttime += Time.deltaTime; //set a timer and update it with the deltatime since last frame every frame
        if (currenttime >= max_timer)
        {
            //if  the timer exeeds the max timer, reset the ball
            //restart code
            Debug.Log("ball stuck, restarting");
            reset_ball();

        }

        //if the offensive timer or the defensive timer for hogging the ball goes below 0, we reset the ball
        if (offtimer <= 0f || deftimer <= 0f)
        {

            reset_ball();
        }
 
    }


    //code used to reset the game when a player scores
    void reset_ball()
    {
        Debug.Log("RESET BALL CALLED");
        goal_happened = false;//set goal to false
        //rb.velocity = new Vector3(0, 0, 0);//set speed to zero, depricated
        transform.position = ballspawn.position;//move to starting location, the ballspawn.position is setup in the game to hold the coordinates for the balls starting position
        unfreeze();//unfreeze so that the ball may land on the ground
        Invoke("freeze", 1.5f); //call freeze after 1.5 seconds to refreeze the ball
        //oldVel = rb.velocity;
        //setup timers back to default values 
        currenttime = 0f;
        offtimer = 25f;
        deftimer = 25f;
        stucktimer = 0f;
        previous_col = "none"; //set the previous collision string to none
        //speed = Academy.Instance.FloatProperties.GetPropertyWithDefault("speed", 25f); //depricated, this was an attempt to increase the learning speed by including the academy functions but was not working right
        moveRand(); //call moveRand which moves the ball in a random direction
    }




    //Unused code
    void FixedUpdate()
    {
        //store velocity for ball to be used on collision
        //oldVel = rb.velocity;
    }


    //obsolete code was once used to create the effect of the ball bouncing but this was solved using the physics materials in the edior
    void OnCollisionEnter(Collision collision)
    {
        //store collision point
        ContactPoint contact = collision.contacts[0];

        //generate new velocity
        //Vector3 reflectVel = Vector3.Reflect(oldVel, contact.normal);

        //apply new velocity
        //rb.velocity = reflectVel;
    }

    //code for training Defensive vs Defensive
    //[[Defensive]] - uncomment to train defensive
    void OnTriggerEnter(Collider other)
    {

        //if our paddle is just hitting itself over and over, start decrementing the clock, this is to prevent the ai from hogging the ball forever on its side of the court
        if (previous_col == "offcol" && other.tag == "def_col_flag_2")
        {
            offtimer -= 1f;
            currenttime = 0f;
        }

        //same as above but for the other paddle
        if (previous_col == "defcol" && other.tag == "def_col_flag")
        {
            deftimer -= 1f;
            currenttime = 0f;
        }




        //if we have a fair hit, reset the timer, this is a "volley"
        if (previous_col == "offcol" && other.tag == "def_col_flag")
        {
            //call the agentscripts reward function to give it a reward
            d_agentscript.AddReward(0.5f); //doubled
            //reset the timer when we collide with a paddle
            offtimer = 25f;
            deftimer = 25f;
            currenttime = 0f;
        }

        //same as above but for the other paddle
        if (previous_col == "defcol" && other.tag == "def_col_flag_2")
        {
            o_agentscript.AddReward(0.5f); //doubled
            //reset the timer when we collide with a paddle
            offtimer = 25f;
            deftimer = 25f;
            currenttime = 0f;
        }

        //if the ball just spawned, it has no previous collision string, we need to give the ai incentive to hit hte ball right away more so than returning a volley
        if (previous_col == "none" && other.tag == "def_col_flag")
        {
            d_agentscript.AddReward(0.75f); //+ 25
        }

        //same as above but for the other paddle
        if (previous_col == "none" && other.tag == "def_col_flag_2")
        {
            o_agentscript.AddReward(0.75f); //+ 25
        }

        //set the previous collider flag to the appropriate string
        if (other.tag == "def_col_flag")
        {
            previous_col = "defcol";
            currenttime = 0f;
            rb.velocity = rb.velocity * 1.15f; //here we are increasing hte speed of the ball by 15 percent each time it hits a paddle
        }

        //same as above but for other paddle
        if (other.tag == "def_col_flag_2")
        {
            previous_col = "offcol";
            currenttime = 0f;
            rb.velocity = rb.velocity * 1.15f;
        }

        //reflect is the tag for the walls, same as above, we want to increase the speed of the ball when it hits something
        if (other.tag == "Reflect")
        {
            rb.velocity = rb.velocity * 1.15f;
        }

        //off vs off
        //here is the code for scoring goals in the nets, the o_net and d_net are the walls behind each paddle respectively
        if (other.tag == "o_net")
        {
            goal_happened = true; //let our controller know that a goal has occured
            d_score++; //increment the score
            //o_agent.GetComponent<O_Agent>().AddReward(-1f); 
            //Goal is scored on Offensive Agent
            //here we set the appropriate rewards or penalties depending on who scored
            d_agentscript.AddReward(0.5f); //half
            o_agentscript.AddReward(-1.25f); // + 0.5
            d_agentscript.EndEpisode(); //here we end the episode, which lets the algorthim know that it is done collecting rewards and penalties for this round and moves to the next round
            o_agentscript.EndEpisode();
            reset_ball();
        }

        //same as above but for the other net
        if (other.tag == "d_net")
        {
            goal_happened = true;
            o_score++;
            d_agentscript.AddReward(-1.25f);          //+0.5    //Goal Scored against Defensive Agent
            o_agentscript.AddReward(0.5f);     //half       //Goal is scored by Offensive Agent
            d_agentscript.EndEpisode();
            o_agentscript.EndEpisode();
            reset_ball();
        }



    }

    //code for training Offensive vs  Offensive
    //[[Offensive]] - uncomment to train offensive
    /*void OnTriggerEnter(Collider other)
    {

        //if our paddle is just hitting itself over and over, start decrementing the clock
        if (previous_col == "offcol" && other.tag == "off_col_flag")
        {
            offtimer -= 1f;
            currenttime = 0f;
        }

        if (previous_col == "defcol" && other.tag == "off_col_flag_2")
        {
            deftimer -= 1f;
            currenttime = 0f;
        }




        //if we have a fair hit, reset the timer
        if (previous_col == "offcol" && other.tag == "off_col_flag_2")
        {
            d_agentscript.AddReward(0.25f);
            //reset the timer when we collide with a paddle
            offtimer = 25f;
            deftimer = 25f;
            currenttime = 0f;
        }

        if (previous_col == "defcol" && other.tag == "off_col_flag")
        {
            o_agentscript.AddReward(0.25f);
            //reset the timer when we collide with a paddle
            offtimer = 25f;
            deftimer = 25f;
            currenttime = 0f;
        }

        if (previous_col == "none" && other.tag == "off_col_flag_2")
        {
            d_agentscript.AddReward(0.5f);
        }

        if (previous_col == "none" && other.tag == "off_col_flag")
        {
            o_agentscript.AddReward(0.5f);
        }

        //set the previous collider flag
        if (other.tag == "off_col_flag_2")
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

        //off vs off
        if (other.tag == "o_net")
        {
            goal_happened = true;
            d_score++;
            //o_agent.GetComponent<O_Agent>().AddReward(-1f); 
            //Goal is scored on Offensive Agent
            d_agentscript.AddReward(1f);
            o_agentscript.AddReward(-0.75f);
            d_agentscript.EndEpisode();
            o_agentscript.EndEpisode();
            reset_ball();
        }

        if (other.tag == "d_net")
        {
            goal_happened = true;
            o_score++;
            d_agentscript.AddReward(-0.75f);             //Goal Scored against Defensive Agent
            o_agentscript.AddReward(1f);            //Goal is scored by Offensive Agent
            d_agentscript.EndEpisode();
            o_agentscript.EndEpisode();
            reset_ball();
        }



    } */



    //code for training Offensive vs Defensive (note this is currently not producing valuable results)
    /* here is for off vs defense
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

        
        //off vs deff
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
*/


    //this code will generated a random float in the range of -1 to 1
    public float genRand()
    {
        float rand = Random.Range(-1f, 1f);
        return rand;
    }

    //function to move the ball
    void moveRand()
    {
        //first we grab two random floats
        float randx = Random.Range(-1.0f, 1.0f);
        float randz = Random.Range(-1.0f, 1.0f);
        //float p = Random.Range(-1.0f, 1.0f); //debuging purposes
        Debug.Log(randx);
        //here we set the direciton object to the new calculated vector for speed and direction
        dir =new Vector3 (randx * speed, 0f, randz * speed);
        // rb.velocity = new Vector3(Random.Range(-1.0f, 1.0f) * speed, 0, Random.Range(-1.0f, 1.0f) * speed);
        rb.velocity = dir; //here we apply the calculated speed and direction velocity to the objects physics
        Debug.Log(rb.velocity);
    }

}
