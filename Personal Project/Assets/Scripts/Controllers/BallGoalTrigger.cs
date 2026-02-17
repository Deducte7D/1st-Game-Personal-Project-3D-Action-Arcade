using UnityEngine;

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

    public void triggerCooldown()
    {
        if (canTrigger == false)
        {
            new WaitForSeconds(triggerDelay);
            canTrigger = true;
        }
    }
}
