using UnityEngine;
using System.Collections;

public class EnemyT3Hitbox : MonoBehaviour
{
    private EnemyT3Health enemyT3HP;
    public BunshinPoolManager bunshinPool;

    //public int tackleHits = 0;
    public bool isHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyT3HP = GetComponent<EnemyT3Health>();
    }

    private void Update()
    {
        if (bunshinPool == null)
        {
            bunshinPool = FindFirstObjectByType<BunshinPoolManager>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BunshinHitBox"))
        {
            enemyT3HP.TakeDamageT3(50); //boss deals 50% player's hp
            isHit = true;
            Debug.Log("Bunshin Hit T3");

            //// Reset hit after value of WaitForSeconds second
            //StartCoroutine(ResetHit());

            // Disable the hitting object
            // other.gameObject.SetActive(false);

            // Destroy the hitting object
            // Destroy(other.gameObject);
            // bunshinPool.ReturnBunshin(other.gameObject); // this only return the hitbox object since its the child object

            string chosenTypeSpawn = "Bunshin"; // key name of prefab

            bunshinPool.ReturnBunshin(chosenTypeSpawn, other.transform.root.gameObject); // this will return the root/parent of the child of 'other'


        }
    }

    //IEnumerator ResetHit()
    //{
    //    yield return new WaitForSeconds(10f);
    //    isHit = false;
    //}
}
