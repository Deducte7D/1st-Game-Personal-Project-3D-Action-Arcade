using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Sprites;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    private PlayerController playerController;

    private float originalSpeed;
    private float originalJumpForce;

    private Coroutine lightningRoutine; // Hold reference to the coroutine
    private Coroutine puddleMudRoutine; 
    private Coroutine bubbleShieldRoutine; 
    private Coroutine shadowCloneRoutine; 
    private Coroutine enemyCloneRoutine; 
    private Coroutine unwingedRoutine;

    public bool isShieldActive = false;

    public GameObject ShieldObject; // assign in inspector
    public GameObject kageBunshin; 
    public GameObject enemyBunshin;

    public BunshinPoolManager bunshinPool;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        originalSpeed = playerController.speed;
        originalJumpForce = playerController.jumpForceVar;
    }

    public void ApplyLightning(float amount, float duration)
    {
        // If lightning is already active then restart timer
        if (lightningRoutine != null)
        {
            StopCoroutine(lightningRoutine);
            playerController.speed = originalSpeed; // reset first
            playerController.jumpForceVar = originalJumpForce; // reset first
        }

        lightningRoutine = StartCoroutine(LightningEffect(amount, duration));
    }

    private IEnumerator LightningEffect(float amount, float duration)
    {
        // If lightning is already active then restart timer
        if (puddleMudRoutine != null)
        {
            StopCoroutine(puddleMudRoutine);
            playerController.speed = originalSpeed; // reset first
        }

        // Apply boost
        playerController.speed += amount;

        yield return new WaitForSeconds(duration);

        // Revert back to normal speed
        playerController.speed = originalSpeed;

        lightningRoutine = null;
    }

    public void ApplyPuddleMud(float amount, float duration)
    {
        // If lightning is already active then restart timer
        if (puddleMudRoutine != null)
        {
            StopCoroutine(puddleMudRoutine);
            playerController.speed = originalSpeed; // reset first
        }

        puddleMudRoutine = StartCoroutine(PuddleMudEffect(amount, duration));
    }

    private IEnumerator PuddleMudEffect(float amount, float duration)
    {
        // If lightning is already active then restart timer
        if (lightningRoutine != null)
        {
            StopCoroutine(lightningRoutine);
            playerController.speed = originalSpeed; // reset first
        }

        // Apply decrease speed
        playerController.speed -= amount;

        yield return new WaitForSeconds(duration);

        // Revert back to normal speed
        playerController.speed = originalSpeed;

        puddleMudRoutine = null;
    }

    public void ApplyShield(float duration)
    {
        if (bubbleShieldRoutine != null)
        {
            StopCoroutine(bubbleShieldRoutine);
            DisableShield();
        }

        bubbleShieldRoutine = StartCoroutine(ShieldEffect(duration));
    }

    // Shield effect will nullify damage for a few seconds in PlayerHealth.cs
    private IEnumerator ShieldEffect(float duration)
    {
        EnableShield();

        yield return new WaitForSeconds(duration);

        DisableShield();
        bubbleShieldRoutine = null;
    }

    private void EnableShield()
    {
        isShieldActive = true;
        if (ShieldObject != null)
            ShieldObject.SetActive(true);
    }

    private void DisableShield()
    {
        isShieldActive = false;
        if (ShieldObject != null)
            ShieldObject.SetActive(false);
    }

    public void ApplyShadowClone(float copies, float duration)
    {
        // If shadowClone is already active then restart timer
        if (shadowCloneRoutine != null)
        {
            StopCoroutine(shadowCloneRoutine);
        }

        shadowCloneRoutine = StartCoroutine(ShadowCloneEffect(copies, duration));
    }

    private IEnumerator ShadowCloneEffect(float copies, float duration)
    {
        for (int i = 0; i < copies; i++)
        {
            // Example: spread them out along X axis
            // Vector3 spawnPos = centerPos + new Vector3(i * spacing, 0f, 0f);
            // --- Spawn clones ---
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

            string chosenTypeSpawn = "Bunshin"; // key name of prefab

            GameObject bunshin = bunshinPool.GetBunshin(chosenTypeSpawn, spawnPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(duration);

        shadowCloneRoutine = null;

        //for (int i = 0; i < copies; i++)
        //{
        //    // --- Spawn clones ---
        //    Vector3 spawnPos = transform.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

        //    // Pick prefab
        //    GameObject prefabToSpawn = kageBunshin;

        //    Instantiate(prefabToSpawn, spawnPos, transform.rotation);
        //}    

        //yield return new WaitForSeconds(duration);

        //shadowCloneRoutine = null;

        // test idling discord lol
    }

    public void ApplyEnemyClone(float copies, float duration)
    {
        // If shadowClone is already active then restart timer
        if (enemyCloneRoutine != null)
        {
            StopCoroutine(enemyCloneRoutine);
        }

        enemyCloneRoutine = StartCoroutine(EnemyCloneEffect(copies, duration));
    }

    private IEnumerator EnemyCloneEffect(float copies, float duration)
    {
        Vector3 eggSpawnPoint = new Vector3(0, 0, 0);

        for (int i = 0; i < copies; i++)
        {
            // --- Spawn clones ---
            Vector3 spawnPos = eggSpawnPoint + new Vector3(Random.Range(40f, 50f), 0f, Random.Range(-30f, 30f));

            // Pick randomly between two prefabs
            GameObject prefabToSpawn = enemyBunshin;

            Instantiate(prefabToSpawn, spawnPos, transform.rotation);
        }

        yield return new WaitForSeconds(duration);

        enemyCloneRoutine = null;
    }

    public void ApplyUnwinged(float amount, float duration)
    {
        // If Unwinged is already active then restart timer
        if (unwingedRoutine != null)
        {
            StopCoroutine(unwingedRoutine);
            playerController.jumpForceVar = originalJumpForce; // reset first
        }

        unwingedRoutine = StartCoroutine(UnwingedEffect(amount, duration));
    }

    private IEnumerator UnwingedEffect(float amount, float duration)
    {
        
        // Apply boost
        playerController.jumpForceVar = amount;

        yield return new WaitForSeconds(duration);

        // Revert back to normal speed
        playerController.jumpForceVar = originalJumpForce;

        unwingedRoutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        // pls dont shutdowneh 
    }
}
