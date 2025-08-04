using UnityEngine;

public class TackleTrigger : MonoBehaviour
{
    public EnemyController enemy;
    //private Rigidbody radiusTrig;


    void Start()
    {
        //radiusTrig = GetComponent<Rigidbody>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Enemy"))
        {

            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.StartSlideTackle(transform.root); // send player transform as target
            }
        }
    }
}
