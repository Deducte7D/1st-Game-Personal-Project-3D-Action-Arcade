using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class BunshinPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolConfig         // class of pool, contains few elements
    {
        public string key;          // e.g "T2", "T3"
        public GameObject prefab;   // assign prefab in inspector
        public int poolSize = 10;
    }

    public List<PoolConfig> pools;  // list of that class assigned as 'pools' variable

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    // ------------------------
    //public GameObject bunshinPrefab;   // assign in Inspector
    //public int poolSize = 10;          // how many Bunshin to pre-spawn

    //private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        // Pre-spawn Bunshin and disable them
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolConfig config in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < config.poolSize; i++)
            {
                GameObject obj = Instantiate(config.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(config.key, objectPool);
        }
        
        // ----------------------------

        //// Pre-spawn Bunshin and disable them
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject obj = Instantiate(bunshinPrefab);
        //    obj.SetActive(false);
        //    pool.Enqueue(obj);
        //}
    }

    // Get Bunshin of a specific type
    public GameObject GetBunshin(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(key)) // if the dictionary doesnt have that key string
        {
            Debug.LogWarning("No pool with key" + key);
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[key];

        if (objectPool.Count > 0)
        {
            GameObject obj = objectPool.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // Optional: expand pool dynamically
            PoolConfig config = pools.Find(p => p.key == key);
            if (config != null)
            {
                GameObject obj = Instantiate(config.prefab, position, rotation);
                return obj;
                
            }
            return null;
        }
    }

    // Return bunshin back to its pool
    public void ReturnBunshin(string key, GameObject obj)
    {
        if(!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning("No pool with key :" + key);
            Destroy(obj); // fallback
            return;
        }

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }

    // ------------------

    //// Get a Bunshin from the pool
    //public GameObject GetBunshin1(Vector3 position, Quaternion rotation)
    //{
    //    if (pool.Count > 0)
    //    {
    //        GameObject obj = pool.Dequeue();
    //        obj.transform.SetPositionAndRotation(position, rotation);
    //        obj.SetActive(true);
    //        return obj;
    //    }
    //    else
    //    {
    //        // Optional: expand pool if empty
    //        GameObject obj = Instantiate(bunshinPrefab, position, rotation);
    //        return obj;
    //    }
    //}

    //// Return Bunshin back to pool
    //public void ReturnBunshin1(GameObject obj)
    //{
    //    obj.SetActive(false);
    //    pool.Enqueue(obj);
    //}


    // Do the same but for T3BunshinPool
    

}

