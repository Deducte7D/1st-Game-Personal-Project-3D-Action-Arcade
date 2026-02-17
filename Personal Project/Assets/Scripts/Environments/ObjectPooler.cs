using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public EnemyControllerT1 enemyControllerT1;
    public EnemyController enemyControllerT2;
    public EnemyControllerT3 enemyControllerT3;
    public KeeperController keeperController;
    public EnemyT3Health healthT3;
    public LevelUpdater levelUpdater; // not really used here

    public int currentLevel = 1;

    [System.Serializable]
    public class Pool
    {
        // elements of class Pool
        public string poolName;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools; // declare variable 'pools' as list of class
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;

    void Awake() // method for pre building, especially for assigning stuff
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools) // iterates through each pool made in inspector
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); // create new queue, hold all inactive prefab objects

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); // copy creation of the prefab
                obj.SetActive(false); // instantly set inactive to prevent appear on scene
                objectPool.Enqueue(obj); // add to queues
            }

            poolDictionary.Add(pool.poolName, objectPool);
        }
    }

    private void Update()
    {
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
            //healthT3 = GetComponentInChildren<EnemyT3Health>();
        }

    }

    // public GameObject SpawnFromPool(string poolName, Vector3 position, Quaternion rotation)
    public GameObject SpawnFromPool(string poolName, float wavecount, float levelcount) /// will be called by spawnmanager
    {
        currentLevel = (int)levelcount; // check level

        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning("Pool with name " + poolName + " doesn't exist.");
            return null;
        }

        // if conditions (nested if needed) here should name the pool of the sprite
        // follow by the statement of spawning at the same position (spawnpoint)

        if (poolName == "T1S1Pool")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // array 0 - 2 for T1 & T2
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<EnemyControllerT1>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;

        }
        if (poolName == "T1S2Pool")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // array 0 - 2 for T1 & T2
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<EnemyControllerT1>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;

        }
        if (poolName == "T1S3Pool")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // array 0 - 2 for T1 & T2
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<EnemyControllerT1>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;

        }
        if (poolName == "T2S1Pool")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // array 0 - 2 for T1 & T2
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<EnemyController>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;

        }
        if (poolName == "T2S2Pool")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // array 0 - 2 for T1 & T2
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<EnemyController>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;

        }
        if (poolName == "T2S3Pool")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // array 0 - 2 for T1 & T2
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<EnemyController>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;

        }
        if( poolName == "T3")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // array 0 - 2 for T3
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<EnemyControllerT3>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            //// to enable radiustriggerT3 object and script
            //GameObject radTrigT3 = GameObject.Find("RadiusTriggerT3");
            //radTrigT3.SetActive(true);

            return objectToSpawn;
        }
        if (poolName == "Keeper")
        {
            Transform point = spawnPoints[5]; // only middle spawn point array 5
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.GetComponent<KeeperController>().Initialize(currentLevel);

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }
        if (poolName == "GoalPost")
        {
            Transform point = spawnPoints[4]; // only middle spawn point array 4
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = Quaternion.Euler(0f, -90f, 0f); // rotate the goalpost

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }
        if (poolName == "BubbleShield")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // 3 sapwn points
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }
        if (poolName == "LightningSpeed")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // 3 sapwn points
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }
        if (poolName == "SmokeDupe")
        {
            Transform point = spawnPoints[Random.Range(0, 2)]; // 3 sapwn points
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }
        if (poolName == "SlowPuddle")
        {
            Transform point = spawnPoints[3]; // dedicated spawn point follow at array 3 
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }
        if (poolName == "GroundNoJump")
        {
            Transform point = spawnPoints[3]; // dedicated spawn point follow at array 3 
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }
        if (poolName == "EggDupe")
        {
            Transform point = spawnPoints[3]; // dedicated spawn point follow at array 3 
            GameObject objectToSpawn = poolDictionary[poolName].Dequeue(); // takes out 1st object FIFO

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = point.position; // position I will edit based on the spawn point
            objectToSpawn.transform.rotation = point.rotation;

            poolDictionary[poolName].Enqueue(objectToSpawn); // adds back to the queue

            return objectToSpawn;
        }

        return null;
    }
}

/*
public class WaveSpawner : MonoBehaviour 
{
    public ObjectPooler pooler;

    void SpawnWave()
    {
        for (int i = 0; i < 3; i++)
        {
            string[] enemyTypes = { "EnemyType1A", "EnemyType1B", "EnemyType1C" };
            string chosenType = enemyTypes[Random.Range(0, enemyTypes.Length)];

            pooler.SpawnFromPool(chosenType, new Vector3(i * 2, 0, 0), Quaternion.identity);
        }
    }
}

public class Enemy : MonoBehaviour
{
    public int health = 100;

    void OnEnable()
    {
        // Reset state whenever object is re-enabled
        health = 100;
    }
}


*/