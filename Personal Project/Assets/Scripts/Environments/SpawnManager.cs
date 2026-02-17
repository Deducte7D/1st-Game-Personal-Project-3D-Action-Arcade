using UnityEngine;
using System.Collections;
using System;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] prefabToSpawn;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private bool autoStart = true;

    [Header("Wave Settings")]
    [SerializeField] private bool useWaves = true;
    [SerializeField] private int objectsPerWave = 3; // for T1 and T2
    [SerializeField] private int PUPerWave = 1; // for PU
    [SerializeField] private int numberSpawnBossGoal = 1; // for Boss and Goal
    [SerializeField] private float waveInterval = 5f;
    [SerializeField] private float currentWaveCount = 1; // indefinite
    [SerializeField] private float currentLevelCount = 1; // indefinite
    [SerializeField] private float speedIncrease; // indefinite
    [SerializeField] private float T3HPIncrease; // indefinite

    // [SerializeField] public bool isKeeperInstanced = false;

    public event Action<GameObject> OnObjectSpawned;

    private Coroutine spawnRoutine;

    public bool isKeeperInstanced = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // bila nk start anjei 
        if (autoStart)
            StartSpawning(currentWaveCount, currentLevelCount);
    }

    private void FixedUpdate()
    {
        if(isKeeperInstanced == true)
        {
            isKeeperInstanced = true;
        }
    }

    public void StartSpawning(float waveCount, float levelCount)
    {
        // spawn with combination based on bool state
        if (spawnRoutine == null && currentWaveCount < 5)
        {
            // decide if using wave or interval spawner
            spawnRoutine = StartCoroutine(useWaves ? WaveSpawner(currentWaveCount, currentLevelCount) : IntervalSpawner());
        }
    }

    // stop spawn routine
    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    // use IS
    private IEnumerator IntervalSpawner()
    {
        while (true)
        {
            //SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // use WS
    private IEnumerator WaveSpawner(float waveCount, float levelCount)
    {
        // for this section please recaliberate logic
        // bcos it seems weird but its following the algorithm needed
        while (currentWaveCount <= 6) // limit wave to 6 end and then level up
        {
            if (currentWaveCount < 5)
            {
                for (int i = 0; i < objectsPerWave; i++)
                {
                    //SpawnObject();
                    SpawnT1T2(currentWaveCount, currentLevelCount);

                }
                for (int i = 0; i < PUPerWave; i++)
                {
                    //spawnpowerups
                    SpawnPowerUp(currentWaveCount, currentLevelCount);
                }
                yield return new WaitForSeconds(waveInterval);
                currentWaveCount++;
            }
            else if (currentWaveCount == 5) // spawn T3
            {
                for (int i = 0; i < numberSpawnBossGoal; i++)
                {
                    //SpawnObject();
                    SpawnT3(currentWaveCount, currentLevelCount);
                    // spawnEndEnemy = true; // wait wave end
                }
                yield return new WaitForSeconds(waveInterval);
                currentWaveCount++;
            }
            else if (currentWaveCount == 6) // spawn goal
            {
                for (int i = 0; i < numberSpawnBossGoal; i++)
                {
                    //SpawnObject();
                    SpawnGoal(currentWaveCount, currentLevelCount);
                    SpawnKeeper(currentWaveCount, currentLevelCount);
                    // wait wave and level to be done then state below
                    // spawnEndEnemy = true;
                    // levelCount++;
                }
                yield return new WaitForSeconds(waveInterval);
                currentWaveCount++;
            }
            else if(currentWaveCount > 6)
            {
                // Level completed
                currentWaveCount = 1;
                currentLevelCount++;
            }
                

        }
    }

    // spawn functions belowww

    private void SpawnT1T2(float waveCount, float levelCount)
    {

        if (prefabToSpawn == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: Missing prefab or spawn points.");
            return;
        }

        if (waveCount <= 2 && levelCount == 1)
        {
            Transform point = spawnPoints[UnityEngine.Random.Range(0, 2)]; // array 0 - 2
            GameObject obj = Instantiate(prefabToSpawn[UnityEngine.Random.Range(0, 2)], point.position, point.rotation);
            OnObjectSpawned?.Invoke(obj);

        }
        if (waveCount > 2 && waveCount <= 4 && levelCount == 1)
        {
            Transform point = spawnPoints[UnityEngine.Random.Range(0, 2)]; // array 0 - 2 3 - 5
            GameObject obj = Instantiate(prefabToSpawn[UnityEngine.Random.Range(0, 2)], point.position, point.rotation);
            OnObjectSpawned?.Invoke(obj);
        }
        
        
    }

    private void SpawnT3(float waveCount, float levelCount)
    {
        if (prefabToSpawn == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: Missing prefab or spawn points.");
            return;
        }

        // spawn boss any 3 position
        Transform point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)]; 
        GameObject obj = Instantiate(prefabToSpawn[6], point.position, point.rotation);

        OnObjectSpawned?.Invoke(obj);
        
    }

    private void SpawnKeeper(float waveCount, float levelCount)
    {
        if (prefabToSpawn == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: Missing prefab or spawn points.");
            return;
        }

        Transform point = spawnPoints[5]; // only middle spawn point array 1
        GameObject obj = Instantiate(prefabToSpawn[7], point.position, point.rotation);

        OnObjectSpawned?.Invoke(obj);

        // state for prefab keeper spawned
        isKeeperInstanced = true;
    }

    private void SpawnGoal(float waveCount, float levelCount)
    {
        if (prefabToSpawn == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: Missing prefab or spawn points.");
            return;
        }

        Transform point = spawnPoints[4]; // only middle spawn point array 1
        GameObject obj = Instantiate(prefabToSpawn[8], point.position, Quaternion.Euler(0f, -90f, 0f));

        OnObjectSpawned?.Invoke(obj);
    }

    private void SpawnPowerUp(float waveCount, float levelCount)
    {
        if (prefabToSpawn == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: Missing prefab or spawn points.");
            return;
        }

        Transform point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject obj = Instantiate(prefabToSpawn[UnityEngine.Random.Range(9, 11)], point.position, point.rotation);

        OnObjectSpawned?.Invoke(obj);
    }

    private void SpawnAntiPU(float waveCount, float levelCount)
    {
        if (prefabToSpawn == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("SpawnManager: Missing prefab or spawn points.");
            return;
        }

        Transform point = spawnPoints[3]; // spawn point follow on array 3
        GameObject obj = Instantiate(prefabToSpawn[0], point.position, point.rotation);

        OnObjectSpawned?.Invoke(obj);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
