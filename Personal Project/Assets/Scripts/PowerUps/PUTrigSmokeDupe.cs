using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PUTrigSmokeDupe : MonoBehaviour
{

    public float copies = 3f;
    public float duration = 0f;
    public float floatSpeed = 2f; // units per second 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
                manager.ApplyShadowClone(copies, duration);
            }

            // Hide or destroy after pickup
            gameObject.SetActive(false);
        }

    }
}
