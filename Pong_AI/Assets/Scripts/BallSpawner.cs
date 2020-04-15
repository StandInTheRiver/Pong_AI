using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class BallSpawner : MonoBehaviour
{

    public GameObject ballPrefab;
    public PongArea BallSpawn;

    // Start is called before the first frame update
    void Start()
    {
        var pongBall = Resources.Load<Ball>("Ball/Ball");
        ballPrefab = (pongBall as Ball).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (BallSpawn.spawnBall)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        
        //GO.transform.parent = GameObject.FindObjectsOfType<PongArea>();
    }
}
