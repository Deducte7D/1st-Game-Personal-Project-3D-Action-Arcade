using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BunshinTackleTrigger : MonoBehaviour
{
    // Copy of TackleTrigger.cs but modified (no need keeper detection + include all enemy in 1)

    public BunshinController bunshin;

    private Transform storedTransform; // field to hold the value
    private Transform sendTarget;

    public List<Transform> enemyT1; // assign in Inspector
    public List<Transform> enemyT2;
    public Transform enemyT3;

    public bool prefabExist = true;

    //private Rigidbody radiusTrig;

    // possible idea is to
    // - instantiate every prefab of enemy
    // - but make sure conditions super restricted w/o overlapping

    void Start()
    {
        //radiusTrig = GetComponent<Rigidbody>();
        //bunshin = GetComponent<BunshinController>();

    }

    private void Update()
    {
        
    }


    // method to receive transform value from bunshincontroller script
    public void ReceiveTransform(Transform t)
    {
        storedTransform = t;
        prefabExist = true;
        
        //Debug.Log("Current target transform = " + storedTransform);
        //Debug.Log("Got transform from: " + t.name);
        // You can now use t.position, t.rotation, etc.
    }


    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by: " + other.name);

        //BunshinController bunshin = other.GetComponent<BunshinController>(); 
        // "other" is the collider that entered the trigger, which is an enemy, NOT the bunshin.
        // so the line always returns null

        //-------------------------------------------------

        if (other.CompareTag("EnemyT1") || other.CompareTag("EnemyT2") || other.CompareTag("WifeEnemy"))
        {

            //Debug.Log("Trigger entered");
            bunshin.StartSlideTackleBunshin(prefabExist, storedTransform); // send enemy transform as target
            prefabExist = false;
            //storedTransform = null;
        }

        //-------------------------------------------------

        //if (storedTransform != null && prefabExist)
        //{
        //    if (other.CompareTag("EnemyT1") || other.CompareTag("EnemyT2") || other.CompareTag("WifeEnemy"))
        //    {
        //        Debug.Log("Trigger entered");
        //        bunshin.StartSlideTackleBunshin(prefabExist, storedTransform); // send player transform as target
        //        prefabExist = false;
        //        storedTransform = null;
        //    }
        //}


        //if (other.CompareTag("EnemyT1") || other.CompareTag("EnemyT2") || other.CompareTag("WifeEnemy"))
        //{
        //    sendTarget = bunshin.FindTarget();
        //    bunshin.StartSlideTackleBunshin(sendTarget); // send player transform as target
        //}

        //if (other.CompareTag("EnemyT1") || other.CompareTag("EnemyT2") || other.CompareTag("WifeEnemy"))
        //{

        //    //if (bunshin != null)
        //    //{
        //    //    Transform sendTarget = bunshin.target;
        //    //    Debug.Log("Target Location = " + sendTarget);
        //    //    bunshin.StartSlideTackleBunshin(sendTarget); // send player transform as target
        //    //}

        //    sendTarget = storedTransform;

        //    //Debug.Log("Current target transform = " + sendTarget);

        //    //Transform sendTarget = bunshin.target;
        //    //Debug.Log("Target Location = " + sendTarget);
        //    bunshin.StartSlideTackleBunshin(sendTarget); // send player transform as target

        //    // well currently is passes the transform of the script which is not an object reference


        //}

        //// if null go next, if not null execute
        //if (other.CompareTag("EnemyT1"))
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (enemyT1[i] != null)
        //        {
        //            storedTransform = enemyT1[i];
        //            //sendTarget = bunshin.FindTarget();
        //            //bunshin.StartSlideTackleBunshin(sendTarget); // send player transform as target
        //            bunshin.StartSlideTackleBunshin(storedTransform);
        //        }
        //    }
        //}
        //else if (other.CompareTag("EnemyT2"))
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (enemyT2[i] != null)
        //        {
        //            storedTransform = enemyT2[i];
        //            //sendTarget = bunshin.FindTarget();
        //            //bunshin.StartSlideTackleBunshin(sendTarget); // send player transform as target
        //            bunshin.StartSlideTackleBunshin(storedTransform); // still not working
        //        }
        //    }
        //}
        //else if (other.CompareTag("WifeEnemy"))
        //{
        //    storedTransform = enemyT3;
        //    //sendTarget = bunshin.FindTarget();
        //    //bunshin.StartSlideTackleBunshin(sendTarget); // send player transform as target
        //    bunshin.StartSlideTackleBunshin(storedTransform);
        //}


    }
}
