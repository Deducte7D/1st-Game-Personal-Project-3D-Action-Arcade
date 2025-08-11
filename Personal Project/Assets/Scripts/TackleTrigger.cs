using UnityEngine;

public class TackleTrigger : MonoBehaviour
{
    public EnemyController enemy;
    public EnemyControllerT1 enemyT1;
    //private Rigidbody radiusTrig;


    void Start()
    {
        //radiusTrig = GetComponent<Rigidbody>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("EnemyT1"))
        {
            EnemyControllerT1 enemyT1 = other.GetComponent<EnemyControllerT1>();

            if (enemyT1 != null)
            {
                enemyT1.StartSlideTackleT1(transform.root); // send player transform as target
            }
        }

        if (other.CompareTag("EnemyT2"))
        {

            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.StartSlideTackle(transform.root); // send player transform as target
            }
        }
    }
}
