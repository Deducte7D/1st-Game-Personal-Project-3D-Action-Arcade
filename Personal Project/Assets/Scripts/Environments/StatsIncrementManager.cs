using UnityEngine;

// reason why other obj's stats not updating is because the reference is only instance of the first obj spawn/pooled
public class StatsIncrementManager : MonoBehaviour
{

    [SerializeField] private PlayerController playerController;
    [SerializeField] private BunshinController bunshinController;
    [SerializeField] private EnemyControllerT1 enemyControllerT1;
    [SerializeField] private EnemyController enemyControllerT2;
    [SerializeField] private EnemyControllerT3 enemyControllerT3;
    [SerializeField] private KeeperController keeperController;
    [SerializeField] private SpawnManagerV2 spawnManagerV2;
    [SerializeField] private EnemyT3Health healthT3;

    [System.Serializable]
    public class Stats
    {
        public float speed;
        public float jump;
        public float tackle;
        public float tackleCooldown;
        public float unitsSpawn;
        public int health;
    }

    public Stats playerStats;
    public Stats bunshinStats;
    public Stats tier1Stats;
    public Stats tier2Stats;
    public Stats tier3Stats;
    public Stats keeperStats;
    

    // float currentWaveCount, tempWaveCount;

    float currentLevelCount, tempLevelCount, holdLevelCounter, localLevelCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerController = GetComponent<PlayerController>();
        //bunshinController = GetComponent<BunshinController>();
        //enemyControllerT1 = GetComponent<EnemyControllerT1>();
        //enemyControllerT2 = GetComponent<EnemyController>();
        //enemyControllerT3 = GetComponent<EnemyControllerT3>();
        //keeperController = GetComponent<KeeperController>();
        //spawnManagerV2 = GetComponent<SpawnManagerV2>();
        //healthT3 = GetComponent<EnemyT3Health>();

        

        playerStats.speed = 100;
        playerStats.jump = 50;

        tier1Stats.speed = 100;
        tier1Stats.tackle = 100;
        tier1Stats.tackleCooldown = -0.1f;

        tier2Stats.speed = 150;
        tier2Stats.tackle = 50;
        tier2Stats.tackleCooldown = -0.1f;

        tier3Stats.speed = 50;
        tier3Stats.tackle = 1000;
        tier3Stats.health = 150;

        keeperStats.speed = 30;

        bunshinStats.speed = 30;
        bunshinStats.tackle = 50;
        bunshinStats.tackleCooldown = 0f;

        localLevelCounter = 1;
        currentLevelCount = 1;
        holdLevelCounter = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
        if (bunshinController == null)
        {
            bunshinController = FindFirstObjectByType<BunshinController>();
        }
        if (enemyControllerT1 == null)
        {
            enemyControllerT1 = FindFirstObjectByType<EnemyControllerT1>();
        }
        if (enemyControllerT2 == null)
        {
            enemyControllerT2 = FindFirstObjectByType<EnemyController>();
        }
        if (enemyControllerT3 == null)
        {
            enemyControllerT3 = FindFirstObjectByType<EnemyControllerT3>();
        }
        if (keeperController == null)
        {
            keeperController = FindFirstObjectByType<KeeperController>();
        }
        if (healthT3 == null)
        {
            healthT3 = FindFirstObjectByType<EnemyT3Health>();
        }
        if (spawnManagerV2 == null)
        {
            spawnManagerV2 = FindFirstObjectByType<SpawnManagerV2>();
        }

    }

    // separated incStats per object to transfer needed value to objects' scripts

    public void PlayerIncrementStats(float waveCount, float levelCount)
    {
 
        //tempLevelCount = levelCount; // store temp value
        //currentLevelCount = tempLevelCount; // store value to not root with compared variable

        // every 2 level incr no. of spawn
        localLevelCounter += 1;
        //holdLevelCounter = 1;

        
        if (levelCount > currentLevelCount) // only level up then increase stats
        {
            currentLevelCount = levelCount;

            playerController.speed += playerStats.speed;
            playerController.jumpForce += playerStats.jump;

            if (localLevelCounter - holdLevelCounter == 2)
            {
                holdLevelCounter += 2; // match the localLeverCounter to reset difference
                spawnManagerV2.objectsPerWave += 1; // increase spawnunits in spawnmanager
            }
            
        }

    }

    //public void EnemyT1IncrementStats(float waveCount, float levelCount)
    //{

    //    //tempLevelCount = levelCount; // store temp value
    //    //currentLevelCount = tempLevelCount; // store value to not root with compared variable

    //    if (levelCount > currentLevelCount) // only level up then increase stats
    //    {
    //        currentLevelCount = levelCount;

    //        enemyControllerT1.followForce += tier1Stats.speed;
    //        enemyControllerT1.tackleForce += tier1Stats.tackle;
    //        enemyControllerT1.tackleCooldown += tier1Stats.tackleCooldown;
    //    }
    //}

    //public void EnemyT2IncrementStats(float waveCount, float levelCount)
    //{

    //    //tempLevelCount = levelCount; // store temp value
    //    //currentLevelCount = tempLevelCount; // store value to not root with compared variable

    //    if (levelCount > currentLevelCount) // only level up then increase stats
    //    {
    //        currentLevelCount = levelCount;

    //        enemyControllerT2.followForce += tier2Stats.speed;
    //        enemyControllerT2.tackleForce += tier2Stats.tackle;
    //        enemyControllerT2.tackleCooldown += tier2Stats.tackleCooldown;
    //    }
    //}

    //public void EnemyT3IncrementStats(float waveCount, float levelCount)
    //{

    //    //tempLevelCount = levelCount; // store temp value
    //    //currentLevelCount = tempLevelCount; // store value to not root with compared variable

    //    if (levelCount > currentLevelCount) // only level up then increase stats
    //    {
    //        currentLevelCount = levelCount;

    //        enemyControllerT3.cacheFollowForce += tier3Stats.speed;
    //        enemyControllerT3.tackleForce += tier3Stats.tackle;
    //        healthT3.maxHealth += tier3Stats.health;
    //    }
    //}

    //public void KeeperIncrementStats(float waveCount, float levelCount)
    //{

    //    //tempLevelCount = levelCount; // store temp value
    //    //currentLevelCount = tempLevelCount; // store value to not root with compared variable

    //    if (levelCount > currentLevelCount) // only level up then increase stats
    //    {
    //        currentLevelCount = levelCount;

    //        keeperController.followForce += keeperStats.speed;
    //    }
    //}

    //public void BunshinIncrementStats(float waveCount, float levelCount)
    //{

    //    //tempLevelCount = levelCount; // store temp value
    //    //currentLevelCount = tempLevelCount; // store value to not root with compared variable

    //    if (levelCount > currentLevelCount) // only level up then increase stats
    //    {
    //        currentLevelCount = levelCount;

    //        bunshinController.followForce += bunshinStats.speed;
    //        bunshinController.tackleForce += bunshinStats.tackle;
    //        bunshinController.tackleCooldown += bunshinStats.tackleCooldown;
    //    }
    //}

    //public void PUIncrementStats(float waveCount, float levelCount)
    //{
    //    tempWaveCount = waveCount;
    //    currentWaveCount = tempWaveCount;

    //    tempLevelCount = levelCount; // store temp value
    //    currentLevelCount = tempLevelCount; // store value to not root with compared variable

    //    if (currentLevelCount < levelCount) // only level up then increase stats
    //    {

    //    }
    //}
}
