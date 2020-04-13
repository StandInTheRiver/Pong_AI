using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class Ball : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 direction;
    public float margin = 0;
    public float hitPoint;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(speed * direction.x, speed * direction.y, 0f);
    }

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

    public Borders borders = new Borders(11.9813f, -4.018704f, -16f, 16f);
    
    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x >= 16f)
        {
            this.transform.position = new Vector3(speed * 0, speed * 0, 0f);
        //    GameObject.Find("PongManager").GetComponent<CountScore>().Paddle_1_Score++;
        }
        if (this.transform.position.x <= -16f)
        {
            this.transform.position = new Vector3(speed * 0, speed * 0, 0f);
        //    GameObject.Find("PongManager").GetComponent<CountScore>().Paddle_2_Score++;
        }
    }
}