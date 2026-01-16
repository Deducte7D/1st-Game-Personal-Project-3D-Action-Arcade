using UnityEngine;
using System.Collections;
using System;

public class SpawnManagerV2 : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] prefabToSpawn;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 0f;
    [SerializeField] private bool autoStart = true;

    [Header("Wave Settings")]
    [SerializeField] private bool useWaves = true;
    [SerializeField] public int objectsPerWave = 3; // for T1 and T2
    [SerializeField] private int PUPerWave = 1; // for PU
    [SerializeField] public int APUPerWave = 1; // for APU
    [SerializeField] public int SmokeDupeUnits = 3; // for custom smoke dupe only needed for defeating enemy
    [SerializeField] private int numberSpawnBossGoal = 1; // for Boss and Goal
    [SerializeField] private float waveInterval = 5f;
    [SerializeField] private float currentWaveCount = 1; // indefinite
    [SerializeField] private float currentLevelCount = 0; // indefinite

    [SerializeField] private float smokeDupeInterval = 5f;
    [SerializeField] private bool isAliveT3 = true;

    //[SerializeField] private float speedIncrease; // indefinite
    //[SerializeField] private float T3HPIncrease; // indefinite
    
    // [SerializeField] public bool isKeeperInstanced = false;

    // public event Action<GameObject> OnObjectSpawned;

    private Coroutine spawnRoutine;

    public bool isKeeperInstanced = false;

    public ObjectPooler pooler;

    public StatsIncrementManager statsIncrementManager;

    public LevelUpdater levelUpdater;

    //public StatsIncrementManagerV2 statsIncrementManagerV2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelUpdater.LevelUp();
        currentLevelCount = levelUpdater.currentLevel;
        // bila nk start anjei 
        if (autoStart)
            StartSpawning(currentWaveCount, currentLevelCount);
    }

    private void FixedUpdate()
    {
        if (isKeeperInstanced == true)
        {
            isKeeperInstanced = true;
        }
    }

    private void Update()
    {
        currentLevelCount = levelUpdater.currentLevel;
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
        currentLevelCount = levelCount;
        // for this section please recaliberate logic
        // bcos it seems weird but its following the algorithm needed
        while (currentWaveCount <= 7) // limit wave to 6 end and then level up
        {
            if (currentWaveCount < 5)
            {
                for (int i = 0; i < objectsPerWave; i++)
                {
                    
                    // rework method call
                    // declare prefab variable, with sprites array list inside
                    // the declared prefab variable sprites are randomized first
                    // then the spawnfrompool method is called using that parameter

                    string[] enemyTypes = { "T1S1Pool", "T1S2Pool", "T1S3Pool", "T2S1Pool", "T2S2Pool", "T2S3Pool" };
                    string chosenType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];

                    // spawn method call
                    pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

                    //SpawnObject();
                    //SpawnT1T2(currentWaveCount, currentLevelCount);

                }
                for (int i = 0; i < PUPerWave; i++)
                {
                    string[] enemyTypes = { "BubbleShield", "LightningSpeed", "SmokeDupe" };
                    string chosenType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];

                    // spawn method call
                    pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

                    //spawnpowerups
                    //SpawnPowerUp(currentWaveCount, currentLevelCount);
                }
                for (int i = 0; i < APUPerWave; i++)
                {
                    string[] enemyTypes = { "SlowPuddle", "GroundNoJump", "EggDupe" };
                    string chosenType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];

                    // spawn method call
                    pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

                    //spawnpowerups
                    //SpawnPowerUp(currentWaveCount, currentLevelCount);
                }

                yield return new WaitForSeconds(waveInterval);
                currentWaveCount++;
            }
            else if (currentWaveCount == 5) // spawn T3
            {
                for (int i = 0; i < numberSpawnBossGoal; i++)
                {
                    string[] enemyTypes = { "T3" };
                    string chosenType = enemyTypes[0];

                    // spawn method call
                    pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

                    //SpawnObject();
                    // SpawnT3(currentWaveCount, currentLevelCount);
                    // spawnEndEnemy = true; // wait wave end
                }
                //for testing 
                for (int i = 0; i < SmokeDupeUnits; i++)
                {
                    string[] enemyTypes = { "SmokeDupe" };
                    string chosenType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];

                    // spawn method call
                    pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

                    //spawnpowerups
                    //SpawnPowerUp(currentWaveCount, currentLevelCount);
                }
                //while (isAliveT3)
                //{
                //    string[] enemyTypes = { "SmokeDupe" };
                //    string chosenType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];

                //    // spawn method call
                //    pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

                //    //spawnpowerups
                //    //SpawnPowerUp(currentWaveCount, currentLevelCount);
                //    new WaitForSeconds(smokeDupeInterval);
                //}
                // PLS INCLUDE CONDITION TO CHECK T3 IS DEAD AND COUNT UP AND END WHILE LOOP FOR SMOKE DUPE SPAWN
                yield return new WaitForSeconds(waveInterval);
                currentWaveCount++;
            }
            else if (currentWaveCount == 6) // spawn goal
            {
                for (int i = 0; i < numberSpawnBossGoal; i++)
                {

                    string[] enemyTypes = { "Keeper" };
                    string chosenType = enemyTypes[0];

                    // spawn method call
                    pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

                    // state for prefab keeper spawned
                    isKeeperInstanced = true;

                    // declare new instant variable
                    string[] enemyTypes2 = { "GoalPost" };
                    string chosenType2 = enemyTypes2[0];

                    // spawn method call
                    // pooler.SpawnFromPool(chosenType2, currentWaveCount, currentLevelCount);

                    //SpawnObject();
                    //SpawnGoal(currentWaveCount, currentLevelCount);
                    //SpawnKeeper(currentWaveCount, currentLevelCount);
                    // wait wave and level to be done then state below
                    // spawnEndEnemy = true;
                    // levelCount++;
                }
                yield return new WaitForSeconds(waveInterval);
                currentWaveCount++;
            }
            else if (currentWaveCount > 6)
            {
                Debug.Log("Resetted Wave, Leveled Up");
                // Level completed

                // Here should be the method call where stats are increased
                // passing the counter value of currentWaveCount and currentLevelCount



                // The wave reset and level counter
                currentWaveCount = 1;
                //currentLevelCount++;
                levelUpdater.LevelUp();
                statsIncrementManager.PlayerIncrementStats(currentWaveCount, currentLevelCount);
                //statsIncrementManager.EnemyT1IncrementStats(currentWaveCount, currentLevelCount);
                //statsIncrementManager.EnemyT2IncrementStats(currentWaveCount, currentLevelCount);
                //statsIncrementManager.EnemyT3IncrementStats(currentWaveCount, currentLevelCount);
                //statsIncrementManager.KeeperIncrementStats(currentWaveCount, currentLevelCount);
                //statsIncrementManager.BunshinIncrementStats(currentWaveCount, currentLevelCount);
            }


        }
    }
}
