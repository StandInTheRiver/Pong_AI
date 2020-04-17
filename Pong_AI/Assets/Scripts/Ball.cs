using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;
using MLAgents.Sensors;

public class Ball : MonoBehaviour
{
    public float speed = 6.5f;
    public Vector3 direction;
    public Bumper1Agent bumper1;
    public Bumper2Agent bumper2;
    private float noMovementThreshold = 0.01f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    public Vector3 size;
    public Vector3 center;
    private bool isMoving;

    public struct Borders
    {
        public float top, bottom, left, right;

        public Borders(float top, float bottom, float left, float right)
        {
            this.top = top;
            this.bottom = bottom;
            this.left = left;
            this.right = right;
        }
    }

    public Borders borders = new Borders(11.04f, -3.06f, -18.56f, 15.8f);

    void randomBall()
    {
        direction = new Vector3(Random.Range(0.1f, 3f) + Random.Range(0.1f, 0.5f), Random.Range(-1f, 3f) + Random.Range(0.1f, 0.5f), 0).normalized;
        GetComponent<Rigidbody>().velocity = new Vector3(speed * direction.x, speed * direction.y, 0f);
    }

    // Start is called before the first frame update
    void Awake()
    {
        //bumper1 = GameObject.FindGameObjectsWithTag("bumper1_tag");

        if (bumper1.isTraining || bumper2.isTraining)
        {
            speed = Academy.Instance.FloatProperties.GetPropertyWithDefault("speed", 8f);

        }
        else
        {
            speed = 8f;
        }
        randomBall();

        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }


    void Update()
    {
        if (this.transform.position.x >= borders.right)
        {
            this.transform.position = new Vector3(speed * 0, speed * 0, 0f);
            randomBall();
            if (bumper2.isTraining)     //If training is active
            {
                FindObjectOfType<Bumper2Agent>().AddReward(-1f);
            }
            FindObjectOfType<Bumper2Agent>().EndEpisode();
        }
        if (this.transform.position.x <= borders.left)
        {
            this.transform.position = new Vector3(speed * 0, speed * 0, 0f);
            randomBall();
            if (bumper1.isTraining)     //If training is active
            {
                FindObjectOfType<Bumper1Agent>().AddReward(-1f);
            }
            FindObjectOfType<Bumper1Agent>().EndEpisode();
        }

        //Store the newest vector at the end of the list of vectors
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = this.transform.position;

        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
            {
                //The minimum movement has been detected between frames
                isMoving = true;
                break;
            }
            else
            {
                isMoving = false;
            }
        }

        if(!isMoving)
        {
            isMoving = true;
            for (int i = 0; i < previousLocations.Length; i++)
            {
                previousLocations[i] = Vector3.zero;
            }
            center = this.transform.position;
            Vector3 pos = center + new Vector3(Random.Range(-size.x / 10, size.x / 10), Random.Range(-size.y / 10, size.y / 10), this.transform.position.z);
            this.GetComponent<Rigidbody>().velocity = center + new Vector3(Random.Range(((20 * (center.x) + (-size.x))), (20 * ((center.x) + (size.x)))), Random.Range(((20 * (center.y) + (-size.y))), (20 * ((center.y) + (size.y)))), this.transform.position.z);

        }
    }
}
 
 
