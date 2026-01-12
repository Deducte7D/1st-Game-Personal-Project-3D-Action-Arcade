using UnityEngine;

public class StatsIncrementManager : MonoBehaviour
{

    private PlayerController playerController;
    private BunshinController bunshinController;
    private EnemyControllerT1 enemyControllerT1;
    private EnemyController enemyControllerT2;
    private EnemyControllerT3 enemyControllerT3;
    private KeeperController keeperController;
    private SpawnManagerV2 spawnManagerV2;

    [System.Serializable]
    public class Stats
    {
        public float speed;
        public float jump;
        public float tackle;
        public float tackleCooldown;
        public float unitsSpawn;
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
        playerController = GetComponent<PlayerController>();
        bunshinController = GetComponent<BunshinController>();
        enemyControllerT1 = GetComponent<EnemyControllerT1>();
        enemyControllerT2 = GetComponent<EnemyController>();
        enemyControllerT3 = GetComponent<EnemyControllerT3>();
        keeperController = GetComponent<KeeperController>();
        spawnManagerV2 = GetComponent<SpawnManagerV2>();

        localLevelCounter = 1;
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

        keeperStats.speed = 30;

        bunshinStats.speed = 30;
        bunshinStats.tackle = 1000;
        bunshinStats.tackleCooldown = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // separated incStats per object to transfer needed value to objects' scripts

    public void PlayerIncrementStats(float waveCount, float levelCount)
    {
 
        tempLevelCount = levelCount; // store temp value
        currentLevelCount = tempLevelCount; // store value to not root with compared variable

        // every 2 level incr no. of spawn
        localLevelCounter += 1;
        holdLevelCounter = 1;


        if (currentLevelCount < levelCount) // only level up then increase stats
        {
            
            playerController.speed += playerStats.speed;
            playerController.jumpForce += playerStats.jump;

            if (localLevelCounter - holdLevelCounter == 2)
            {
                holdLevelCounter += 2; // match the localLeverCounter to reset difference
                spawnManagerV2.objectsPerWave += 1; // increase spawnunits in spawnmanager
            }
            
        }

    }

    public void EnemyT1IncrementStats(float waveCount, float levelCount)
    {

        tempLevelCount = levelCount; // store temp value
        currentLevelCount = tempLevelCount; // store value to not root with compared variable

        if (currentLevelCount < levelCount) // only level up then increase stats
        {
            enemyControllerT1.followForce += tier1Stats.speed;
            enemyControllerT1.tackleForce += tier1Stats.tackle;
            enemyControllerT1.tackleCooldown += tier1Stats.tackleCooldown;
        }
    }

    public void EnemyT2IncrementStats(float waveCount, float levelCount)
    {

        tempLevelCount = levelCount; // store temp value
        currentLevelCount = tempLevelCount; // store value to not root with compared variable

        if (currentLevelCount < levelCount) // only level up then increase stats
        {
            enemyControllerT2.followForce += tier2Stats.speed;
            enemyControllerT2.tackleForce += tier2Stats.tackle;
            enemyControllerT2.tackleCooldown += tier2Stats.tackleCooldown;
        }
    }

    public void EnemyT3IncrementStats(float waveCount, float levelCount)
    {

        tempLevelCount = levelCount; // store temp value
        currentLevelCount = tempLevelCount; // store value to not root with compared variable

        if (currentLevelCount < levelCount) // only level up then increase stats
        {
            enemyControllerT3.cacheFollowForce += tier3Stats.speed;
            enemyControllerT3.tackleForce += tier3Stats.tackle;
        }
    }

    public void KeeperIncrementStats(float waveCount, float levelCount)
    {

        tempLevelCount = levelCount; // store temp value
        currentLevelCount = tempLevelCount; // store value to not root with compared variable

        if (currentLevelCount < levelCount) // only level up then increase stats
        {
            keeperController.followForce += keeperStats.speed;
        }
    }

    public void BunshinIncrementStats(float waveCount, float levelCount)
    {

        tempLevelCount = levelCount; // store temp value
        currentLevelCount = tempLevelCount; // store value to not root with compared variable

        if (currentLevelCount < levelCount) // only level up then increase stats
        {
            bunshinController.followForce += bunshinStats.speed;
            bunshinController.tackleForce += bunshinStats.tackle;
            bunshinController.tackleCooldown += bunshinStats.tackleCooldown;
        }
    }

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
