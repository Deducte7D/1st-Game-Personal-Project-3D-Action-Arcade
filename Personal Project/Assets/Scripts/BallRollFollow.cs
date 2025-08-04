using UnityEngine;

public class BallRollFollow : MonoBehaviour
{

    public Transform feetTarget; // assign the FeetTarget transform
    public Rigidbody ballRb;
    public float followForce = 10f;
    public float spinForce = 50f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //// Direction from ball to feet target
        //Vector3 direction = (feetTarget.position - transform.position);
        //direction.y = 0f; // optional: ignore vertical offset for grounded movement

        //// Apply force to follow the feet
        //ballRb.AddForce(direction.normalized * followForce);

        

        if (feetTarget != null)
        {
            // Match position exactly
            transform.position = feetTarget.position;

            // Add spinning torque for realism (e.g., roll along x-axis)
            Vector3 forwardDir = feetTarget.forward;
            ballRb.AddTorque(forwardDir * spinForce);

            //// Optional: Match rotation with player (if you want ball to rotate too)
            //transform.rotation = feetTarget.rotation;
        }
    }
}
