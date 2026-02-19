using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class BallGoalTrigger : MonoBehaviour
{

    public Rigidbody ballRb;
    public SpawnManagerV2 spawnManagerV2;
    public DistanceCounter distanceCounter;
    public bool canTrigger;
    public float triggerDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();

        //if (spawnManagerV2 == null)
        //{
        //    spawnManagerV2 = FindFirstObjectByType<SpawnManagerV2>();
        //}

        spawnManagerV2 = FindFirstObjectByType<SpawnManagerV2>();

        canTrigger = true;
        triggerDelay = 3;
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("GoalArea") && canTrigger == true)
        {
            canTrigger = false;
            // Call GoalEvent();
            Debug.Log("Goal! Level Complete!");
            distanceCounter.CounterToRecord();
            spawnManagerV2.YepGoal();
            
        }

        if (other.CompareTag("NoGoalArea") && canTrigger == true)
        {
            canTrigger = false;
            // Call NoGoalEvent();
            Debug.Log("What an Unlucky Shot...");
            spawnManagerV2.NopeGoal();
            
        }
    }

    // should fix the sudden noGoal or yepGoal
    public void triggerCooldown()
    {
        StartCoroutine(DelayedCooldown());
    }

    private IEnumerator DelayedCooldown()
    {
        if(canTrigger == false)
        {
            yield return new WaitForSeconds(3f); // adjust to match animation length
            canTrigger = true;
        }
    }

}
