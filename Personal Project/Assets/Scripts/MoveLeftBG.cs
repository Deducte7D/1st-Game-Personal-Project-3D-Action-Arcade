using UnityEngine;

public class MoveLeftBG : MonoBehaviour
{

    private float speed = 17;
    private PlayerController playerControllerScript;
    private bool isMoving = true;
    //private float leftbound = -10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript =
            GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && playerControllerScript.gameOver == false)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        //if (transform.position.x < leftbound && gameObject.CompareTag("Obstacle"))
        //{
        //    Destroy(gameObject);
        //}
    }

    public float GetSpeed()
    {
        //if (isMoving)
        //{
        //    return speed;
        //}
        //else
        //{
        //    return speed = 0;
        //}
        return speed;
    }

    public void StopMovement()
    {
        isMoving = false;
    }

}
