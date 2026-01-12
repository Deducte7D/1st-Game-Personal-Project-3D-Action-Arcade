using UnityEngine;

public class BunshinHitboxTrig : MonoBehaviour
{

    public BunshinPoolManager bunshinPool;

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
        //if (other.CompareTag("EnemyT1"))
        //{
        //    // DO HITBOX TRIGGER ON ENEMY INSTEAD LOL
        //}

        // to disabled enemy bunshin upon contact. Supossedly do not interrupt with normal enemies
        // due to wave separation and special attack, maybe need to make sure upon T3 enable, reset attack routine
        if (other.CompareTag("EnemyT1"))
        {

            //// Reset hit after value of WaitForSeconds second
            //StartCoroutine(ResetHit());

            // Disable the hitting object
            // other.gameObject.SetActive(false);

            // Destroy the hitting object
            // Destroy(other.gameObject);
            // bunshinPool.ReturnBunshin(other.gameObject); // this only return the hitbox object since its the child object

            string chosenTypeSpawn = "EBunshinT1"; // key name of prefab

            bunshinPool.ReturnBunshin(chosenTypeSpawn, other.transform.root.gameObject); // this will return the root/parent of the child of 'other'


        }
        if (other.CompareTag("EnemyT2"))
        {

            string chosenTypeSpawn = "EBunshinT2"; // key name of prefab

            bunshinPool.ReturnBunshin(chosenTypeSpawn, other.transform.root.gameObject); // this will return the root/parent of the child of 'other'


        }
        //if(other.CompareTag("Border"))
        //{

        //}
    }

}
