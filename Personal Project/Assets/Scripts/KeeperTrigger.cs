using UnityEngine;
using System.Collections;

public class KeeperTrigger : MonoBehaviour
{

    public KeeperController keeper;
    //public BallRollFollow ballShootFx;
    public BallRollFollow ballrollfollowScript;

    public bool isLanded;
    public bool isDiving;
    //private bool isCharging = false;

    //private float shootPower = 0f;

    public Transform Ball; // assign the ballTarget transform

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool toDive = ballrollfollowScript.isReleased;

        Vector3 targetBall = Ball.position;

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
