using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Single : MonoBehaviour
{
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        float startX = Random.Range(0, 2) == 0 ? -1 : 1;
        float startY = Random.Range(0, 2) == 0 ? -1 : 1;

        GetComponent<Rigidbody>().velocity = new Vector3(speed * startX, speed * startY, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x >= 16f)
        {
            this.transform.position = new Vector3(speed * 0, speed * 0, 0f);
        }
        if (this.transform.position.x <= -16f)
        {
            this.transform.position = new Vector3(speed * 0, speed * 0, 0f);
        }
    }
}
