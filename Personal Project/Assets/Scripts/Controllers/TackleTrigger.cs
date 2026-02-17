using UnityEngine;

public class TackleTrigger : MonoBehaviour
{
    public EnemyController enemy;
    public EnemyControllerT1 enemyT1;
    public KeeperController keeper;
    public SpawnManagerV2 spawnManagerV2;

    public float disableTrig = 0;
    public bool canInitiateCI = true;
    //private Rigidbody radiusTrig;


    void Start()
    {
        //radiusTrig = GetComponent<Rigidbody>();
        if (spawnManagerV2 == null)
        {
            spawnManagerV2 = FindFirstObjectByType<SpawnManagerV2>();
        }
    }

    private void Update()
    {
        canInitiateCI = keeper.canInitiateCI;
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

        // trigger intercept for closing in
        if (other.CompareTag("Keeper") && disableTrig == 0 && canInitiateCI == true)
        {
            KeeperController keeper = other.GetComponent<KeeperController>();

            if (keeper != null)
            {
                keeper.InitiateCloseIntercept(); // call intercept
                disableTrig = 1;
                Debug.Log("Intercepted...");
                spawnManagerV2.NopeGoal();
            }

        }
    }

    public void ResetInterceptTrigCondition()
    {
        disableTrig = 0;
    }

}
