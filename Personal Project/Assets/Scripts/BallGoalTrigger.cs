using UnityEngine;

public class BallGoalTrigger : MonoBehaviour
{

    public Rigidbody ballRb;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("GoalArea"))
        {
            // Call GoalEvent();
            Debug.Log("Goal! Level Complete!");

        }

        if (other.CompareTag("NoGoalArea"))
        {
            // Call NoGoalEvent();
            Debug.Log("What an Unlucky Shot...");

        }
    }
}
