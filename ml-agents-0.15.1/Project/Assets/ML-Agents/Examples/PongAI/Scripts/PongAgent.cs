﻿using MLAgents;
using MLAgents.Sensors;
using MLAgents.SideChannels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PongAgent : Agent
{
    public Ball ball;
    public float speed;
    public int paddleSize;

    public override List<float> CollectState()
    {
        List<float> states = new List<float>();

        states.Add(ball.transform.position.x / ball.borders.right);
        states.Add(ball.transform.position.y / ball.borders.top);

        states.Add(transform.position.y / (ball.borders.top - transform.localScale.y / 2));

        states.Add(ball.direction.x);
        states.Add(ball.direction.y);

        states.Add(ball.speed / 30f);
        states.Add(paddleSize / 10f);

        return states;
    }

    public override void AgentStep(float[] action)
    {
        switch ((int) action[0])
        {
            case 0:
                reward = 0.001f;
                break;
            case 1:
                if (transform.position.y < ball.borders.top - transform.localScale.y / 2)
                    transform.position += new Vector3(0, Time.deltaTime * speed, 0);
                break;

            case 2:
                if (transform.position.y > ball.borders.bottom + transform.localScale.y / 2)
                    transform.position -= new Vector3(0, Time.deltaTime * speed, 0);
                break;
        }

        if(ball.transform.position.x < ball.borders.left - ball.margin)
        {
            reward = -1;
            done = true;

            ball.scorePL.text = (int.Parse(ball.scorePL.text) + 1).ToString();
        }

        if (ball.transform.position.x > ball.borders.right + ball.margin)
        {
            ball.scoreAI.text = (int.Parse(ball.scoreAI.text) + 1).ToString();
            done = true;
        }
    }

    public override void AgentReset()
    {
        ball.transform.position = new Vector3(0, 0, 0);

        if (FindObjectOfType<PongAcademy>().isTraining)
        {
            ball.direction = new Vector3(Random.Range(0.1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }
        else
        { 
            Vector3 shootDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;

            if (shootDir.x < 0.1f && shootDir.x > -0.1f)
            {
                shootDir.x = 0.1f;
            }

            ball.direction = shootDir;
        }

        ball.GetComponent<TrailRenderer>().Clear();
        transform.position = new Vector3(-22, 0, 0);

        transform.localScale = new Vector3(1, FindObjectOfType<PongAcademy>().paddleScale, 1);
        ball.speed = FindObjectOfType<PongAcademy>().ballSpeed;
    }
}
