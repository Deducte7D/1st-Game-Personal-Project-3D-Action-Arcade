using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpawnPointFollow : MonoBehaviour
{
    public float floatSpeedFollow = 2f; // units per second
    private GameObject playerObject;
    public Transform transformTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = GameObject.Find("Player");
        transformTarget = playerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transformTarget.position.z);
        }
    }
}
