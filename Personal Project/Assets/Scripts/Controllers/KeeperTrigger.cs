using UnityEngine;
using System.Collections;

public class KeeperTrigger : MonoBehaviour
{

    public KeeperController keeper;
    //public BallRollFollow ballShootFx;
    public BallRollFollow ballrollfollowScript;
    public GameObject ball; // assign the ballTarget transform

    public bool isLanded;
    public bool isDiving;
    //private bool isCharging = false;

    //private float shootPower = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // assigns for runtime for prefab
        ball = GameObject.FindWithTag("Ball");
        ballrollfollowScript = ball.GetComponent<BallRollFollow>();

    }

    // Update is called once per frame
    void Update()
    {
        bool toDive = ballrollfollowScript.isReleased;

        Vector3 targetBall = ball.transform.position;

        if (toDive == true)
        {
            keeper.InitiateDive(targetBall);
        }

    }

    // process,
    // 1. from BallRollFollow.cs, ball is released
    // 2. send bool status to KeeperTrigger.cs to initiate dive
    // 3. dive functions in KeeperController.cs
}
