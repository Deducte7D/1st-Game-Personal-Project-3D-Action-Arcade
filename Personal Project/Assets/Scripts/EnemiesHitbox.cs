using UnityEngine;

public class EnemiesHitbox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bunshin"))
        {
            transform.parent.gameObject.SetActive(false);


            // Destroy(transform.parent.gameObject); // gets the parent Transform of the current object. then  converts that Transform reference into the actual GameObject.
            // transform.parent.gameObject.SetActive(false);
            // 
        }
    }
}
