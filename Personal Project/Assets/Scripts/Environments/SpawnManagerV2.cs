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
    //[SerializeField] public int objectsPerWave = 3; // for T1 and T2
    [SerializeField] private int PUPerWave = 1; // for PU
    [SerializeField] public int APUPerWave = 1; // for APU
    [SerializeField] public int SmokeDupeUnits = 3; // for custom smoke dupe only needed for defeating enemy
    [SerializeField] private int numberSpawnBossGoal = 1; // for Boss and Goal
    [SerializeField] private float waveInterval = 5f;
    [SerializeField] private float currentWaveCount = 1; // indefinite but start with 1
    [SerializeField] private float currentLevelCount = 0; // indefinite

    [SerializeField] private float smokeDupeInterval = 3f;
    [SerializeField] public bool isAliveT3 = true;
    [SerializeField] public bool isGoal = false;
    [SerializeField] public bool isFailAttempt = false;
    [SerializeField] public bool isT3SpawnedOnce = false;

    //[SerializeField] private float speedIncrease; // indefinite
    //[SerializeField] private float T3HPIncrease; // indefinite

    // [SerializeField] public bool isKeeperInstanced = false;

    // public event Action<GameObject> OnObjectSpawned;

    private Coroutine spawnRoutine;

    public bool isKeeperInstanced = false;

    public ObjectPooler pooler;
    //public StatsIncrementManager statsIncrementManager;
    public LevelUpdater levelUpdater;
    public DistanceCounter distanceCounter;
    public ScoringSys scoreManager;
    public PlayerController playerController;
    public PlayerHealth playerHealth;
    public BallRollFollow ballController;
    public KeeperController keeperController;
    public GoalPostController goalPostController;
    public GameUIManager gameUIManager;
    public TackleTrigger playerTackleTrigger;
    public BallGoalTrigger ballGoalTrigger;

    public SpawnCountSO statsData;

    [SerializeField] private float spawnCount; // T1 and T2 only

    [Header("Object Settings")]
    [SerializeField] private GameObject goalDetectScreen;
    [SerializeField] private GameObject noGoalDetectScreen;


    //public StatsIncrementManagerV2 statsIncrementManagerV2;

    public void Initialize()
    {
        spawnCount = statsData.GetSpawnCount((int)currentLevelCount); // increase by 1 every 2 levels
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();

        levelUpdater.LevelUp();
        currentLevelCount = levelUpdater.currentLevel;
        // bila nk start anjei 
        if (autoStart)
            StartSpawning(currentWaveCount, currentLevelCount);

        if (goalDetectScreen == null && noGoalDetectScreen == null)
        {
            goalDetectScreen = GameObject.Find("GoalDetect");
            noGoalDetectScreen = GameObject.Find("NoGoalDetect");
        }

        if (goalDetectScreen != null && noGoalDetectScreen != null)
        {
            goalDetectScreen.SetActive(false);
            noGoalDetectScreen.SetActive(false);
        }

        scoreManager.GameOverResetScore(); // reset score but not saving that 0 value

        Resets();

    }

    private void FixedUpdate()
    {
        if (isKeeperInstanced == true)
        {
            isKeeperInstanced = true;
        }
        else if (isKeeperInstanced == false)
        {
            isKeeperInstanced = false;
        }
    }

    private void Update()
    {
        currentLevelCount = levelUpdater.currentLevel;

        if (keeperController == null)
        {
            keeperController = FindFirstObjectByType<KeeperController>();
        }

        if (goalPostController == null)
        {
            goalPostController = FindFirstObjectByType<GoalPostController>();
        }

        if (scoreManager == null)
        {
            scoreManager = FindFirstObjectByType<ScoringSys>();
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
        currentLevelCount = levelCount;
        // for this section please recaliberate logic
        // bcos it seems weird but its following the algorithm needed
        while (currentWaveCount <= 7) // limit wave to 6 end and then level up
        {
            // reset statements here should be safe
            if(currentWaveCount == 0)
            {
                //Resets(); // reset for new level preparations

                // delay before enemy spawn
                yield return new WaitForSeconds(1); // 2 secs should be ok
                currentWaveCount++;
            }

            if (currentWaveCount < 5 && currentWaveCount > 0)
            {
                for (int i = 0; i < spawnCount; i++)
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
                // Only spawn if boss not already in hierarchy
                if (GameObject.FindWithTag("WifeEnemy") == null && isT3SpawnedOnce == false)
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
                    isT3SpawnedOnce = true;
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
                    //yield return new WaitForSeconds(smokeDupeInterval);
                }

                if (isAliveT3 == true) // shud fixed the non icrement on new level
                {
                    //distanceCounter.levelDistance += 100;
                    distanceCounter.CounterIncrement();
                }
                else if (isAliveT3 == false)
                {
                    yield return new WaitForSeconds(4.5f); // wait a few secs before count up to spawn keeper
                    currentWaveCount++;
                }

                yield return new WaitForSeconds(smokeDupeInterval);

                //InvokeRepeating(nameof(SpawnSmokeDupes), 0f, smokeDupeInterval);

                


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
                //yield return new WaitForSeconds(waveInterval); // this might be preventing the delayed increment of distance
                //currentWaveCount++;



            }
            else if (currentWaveCount == 6) // spawn goal
            {
                for (int i = 0; i < numberSpawnBossGoal; i++)
                {

                    if(!isKeeperInstanced) //is not false = true, thus execute
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
                        pooler.SpawnFromPool(chosenType2, currentWaveCount, currentLevelCount);
                    }

                    //SpawnObject();
                    //SpawnGoal(currentWaveCount, currentLevelCount);
                    //SpawnKeeper(currentWaveCount, currentLevelCount);
                    // wait wave and level to be done then state below
                    // spawnEndEnemy = true;
                    // levelCount++;
                }
                

                if (goalDetectScreen != null && noGoalDetectScreen != null)
                {
                    goalDetectScreen.SetActive(true);
                    noGoalDetectScreen.SetActive(true);
                }

                if (isGoal == true)
                {
                    //distanceCounter.CounterToRecord();
                    currentWaveCount++;
                }
                if (isFailAttempt == true)
                {
                    // end the game record score
                    
                }

                yield return new WaitForSeconds(waveInterval);
                //currentWaveCount++;
            }
            //else if (currentWaveCount > 6)
            //{
            //    // MUST DELAY BEFORE WAVE COUNT 0 RIGHT AWAY
            //    // BECOZ WHEN ITS ZERO ALL HAVE RESET


            //    Debug.Log("Resetted Wave, Leveled Up");
            //    // Level completed

            //    // Here should be the method call where stats are increased
            //    // passing the counter value of currentWaveCount and currentLevelCount

            //    // The wave reset and level counter
            //    // set to 0 first for reset
            //    //currentWaveCount = 0;
            //    //currentLevelCount++;
            //    levelUpdater.LevelUp();

            //    currentLevelCount = levelUpdater.currentLevel;

            //    playerController.Initialize((int)currentLevelCount); // call player stats increment
            //    Initialize(); // for spawn increase by 1 each 2 levels

            //    //statsIncrementManager.PlayerIncrementStats(currentWaveCount, currentLevelCount);
            //    //statsIncrementManager.EnemyT1IncrementStats(currentWaveCount, currentLevelCount);
            //    //statsIncrementManager.EnemyT2IncrementStats(currentWaveCount, currentLevelCount);
            //    //statsIncrementManager.EnemyT3IncrementStats(currentWaveCount, currentLevelCount);
            //    //statsIncrementManager.KeeperIncrementStats(currentWaveCount, currentLevelCount);
            //    //statsIncrementManager.BunshinIncrementStats(currentWaveCount, currentLevelCount);
            //    yield return new WaitForSeconds(5); // temporary delay
            //}


        }
    }

    // methods to check/allow conditions
    public void EnemyT3StatusDead()
    {
        isAliveT3 = false;
    }

    public void YepGoal()
    {
        isGoal = true;
        gameUIManager.WinGame();
    }

    public void NopeGoal()
    {
        isFailAttempt = true;
        gameUIManager.LoseGame();
    }

    public void CallLevelUpdateStatsIncrement()
    {
        // MUST DELAY BEFORE WAVE COUNT 0 RIGHT AWAY
        // BECOZ WHEN ITS ZERO ALL HAVE RESET

        Debug.Log("Resetted Wave, Leveled Up");
        // Level completed

        // Here should be the method call where stats are increased
        // passing the counter value of currentWaveCount and currentLevelCount

        // The wave reset and level counter
        // set to 0 first for reset
        //currentWaveCount = 0;
        //currentLevelCount++;
        levelUpdater.LevelUp();

        currentLevelCount = levelUpdater.currentLevel;

        playerController.Initialize((int)currentLevelCount); // call player stats increment
        Initialize(); // for spawn increase by 1 each 2 levels

        
        new WaitForSeconds(3); // temporary delay
    }

    // call by nextlevelbuttonhandler
    public void ResetWaveCount()
    {
        currentWaveCount = 0;
    }

    // method to reset 
    public void Resets()
    {
        // this if condition should be executed first to decide scoring before bool reset
        if (isGoal == true)
        {
            scoreManager.ResetScore(); // reset score but not saving that 0 value
        }
        else if (isFailAttempt == true)
        {
            scoreManager.GameOverResetScore(); // reset score and save it making it 0 again
        }

        isAliveT3 = true;
        isGoal = false;
        isFailAttempt = false;
        isKeeperInstanced = false; // to reset keeper spawn check for ball script
        isT3SpawnedOnce = false;

        if (goalDetectScreen != null && noGoalDetectScreen != null)
        {
            goalDetectScreen.SetActive(false);
            noGoalDetectScreen.SetActive(false);
        }
        
        distanceCounter.ResetLevelStatus();
        // + method call for player position initial
        playerController.PlayerPosToInit();
        playerHealth.ResetPlayerHP();
        ballController.BallBoolReset();
        playerTackleTrigger.ResetInterceptTrigCondition(); // reset keeper intercept condition
        ballGoalTrigger.triggerCooldown();


        if (keeperController != null)
        {
            keeperController.ResetKeeperBool();
            keeperController.ResetKeeperAnimation();
            keeperController.KeeperDequeue();
        }

        if (goalPostController != null)
        {
            goalPostController.GoalPostDequeue();

        }

        gameUIManager.HidePanels();
    }

    //private void SpawnSmokeDupes()
    //{
    //    for (int i = 0; i < SmokeDupeUnits; i++)
    //    {
    //        string[] enemyTypes = { "SmokeDupe" };
    //        string chosenType = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];

    //        // spawn method call
    //        pooler.SpawnFromPool(chosenType, currentWaveCount, currentLevelCount);

    //        //spawnpowerups
    //        //SpawnPowerUp(currentWaveCount, currentLevelCount);
    //        //yield return new WaitForSeconds(0);
    //    }

    //}


}
