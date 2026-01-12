using GLTFast.Schema;
using UnityEngine;

public class PUTrigLightningSpeed : MonoBehaviour
{

    public float speedBoostAmount = 1000f;
    public float duration = 5f;
    public float floatSpeed = 2f; // units per second 
    

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= -170f && transform.position.x >= -190f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Move left at a constant speed
            transform.position += Vector3.left * floatSpeed * Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // react when contacted w/ ball
        if (other.CompareTag("Player"))
        {
            PowerUpManager manager = other.GetComponent<PowerUpManager>();

            if (manager != null)
            {
                manager.ApplyLightning(speedBoostAmount, duration);
            }

            // Hide or destroy after pickup
            gameObject.SetActive(false);
        }

    }
}
