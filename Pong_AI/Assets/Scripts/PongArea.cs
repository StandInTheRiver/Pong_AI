using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

//unused code, was used in early versions of the game
public class PongArea : MonoBehaviour 
{
    public Bumper1Agent bumper1Agent;                   //The agent that controls the Bumpers
    public Bumper2Agent bumper2Agent;
    public TextMeshPro cumulativeRewardText_Bump1;            //Cumulative reward displayed
    public TextMeshPro cumulativeRewardText_Bump2;
    public GameObject BallPreFab;
    public BallSpawner ballSpawner;
    public bool spawnBall;
    public GameObject ballPrefab;
    public Vector3 size;
    public Vector3 center;

    public void ResetArea()
    {
        center = ballSpawner.transform.position;
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 10, size.x / 10), Random.Range(-size.y / 10, size.y / 10), bumper1Agent.transform.position.z);
        var ball = Resources.Load<Ball>("Ball/Ball");
        ballPrefab = (ball as Ball).gameObject;
        ball.speed = Academy.Instance.FloatProperties.GetPropertyWithDefault("speed", 8f);
        ball.GetComponent<Rigidbody>().velocity = center + new Vector3(Random.Range(((ball.speed*(center.x)+(-size.x / 10))), (ball.speed*((center.x) + (size.x / 10)))), Random.Range(((ball.speed * (center.y) + (-size.y / 10))), (ball.speed * ((center.y) + (size.y / 10)))), bumper1Agent.transform.position.z);

    }

    private void Start()
    {
        ResetArea();
        if(bumper1Agent.isTraining)
        {
            GameObject.FindGameObjectWithTag("Bump2Training").SetActive(false);
        }
        if (bumper2Agent.isTraining)
        {
            GameObject.FindGameObjectWithTag("Bump1Training").SetActive(false);
        }
        if ((!bumper2Agent.isTraining) && (!bumper1Agent.isTraining))
        {
            GameObject.FindGameObjectWithTag("Bump1Training").SetActive(false);
            GameObject.FindGameObjectWithTag("Bump2Training").SetActive(false);
        }
    }

    private void Update()
    {
        cumulativeRewardText_Bump1.text = bumper1Agent.GetCumulativeReward().ToString("0.00");        //Displays the reward for Bumper 1
        cumulativeRewardText_Bump2.text = bumper2Agent.GetCumulativeReward().ToString("0.00");        //Displays the reward for Bumper 2
    }

}
