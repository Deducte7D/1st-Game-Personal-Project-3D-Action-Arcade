using System.Collections;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private PlayerHealth playerHP;

    //public int tackleHits = 0;
    public bool isHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerHP = GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitBoxT3"))
        {
            playerHP.TakeDamage(50); //boss deals 50% player's hp
            isHit = true;
            Debug.Log("Hit");

            // Reset hit after value of WaitForSeconds second
            StartCoroutine(ResetHit());
        }

        if (other.CompareTag("EnemyT1"))
        {
            playerHP.TakeDamage(30); // 100hp , 30/33dmg for 3 times and lef twith 10/1hp, so player may live 4 hits
            isHit = true;
            Debug.Log("Hit by T1");

            // Reset hit after value of WaitForSeconds second
            StartCoroutine(ResetHit());
        }

        if (other.CompareTag("EnemyT2"))
        {
            playerHP.TakeDamage(30); // 100hp , 30/33dmg for 3 times and lef twith 10/1hp, so player may live 4 hits
            isHit = true;
            Debug.Log("Hit by T2");

            // Reset hit after value of WaitForSeconds second
            StartCoroutine(ResetHit());
        }
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(10f);
        isHit = false;
    }

    //    void OnTriggerEnter(Collider other)
    //    {
    //        //Debug.Log("Trigger entered by: " + other.name);

    //        //TackleTriggerT3 tackleTriggerT3 = GetComponent<TackleTriggerT3>();
    //        //bool toReset = tackleTriggerT3.noMoreTackle; // refer status no tackel allowed to reset hits

    //        if (other.CompareTag("HitBoxT3"))
    //        {
    //            isHit = true;
    //            Debug.Log("Hit");

    //            //tackleHits++; // if hitboxes collide increment until 3 hits

    //            //if (tackleHits == 3  && toReset == true) { // once 3 hits
    //            //    tackleHits = 0;
    //            //}
    //        }
    //    }
}
