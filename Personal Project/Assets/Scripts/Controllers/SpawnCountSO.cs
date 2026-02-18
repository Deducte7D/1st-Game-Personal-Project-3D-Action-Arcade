using UnityEngine;

[CreateAssetMenu(fileName = "SpawnCountSO", menuName = "Scriptable Objects/SpawnCountSO")]
public class SpawnCountSO : ScriptableObject
{
    // This will  be the default value initially
    public float spawnCount = 1;

    // Scaling rules
    public float spawnCountPerLevel = 1;

    public int GetSpawnCount(int level)
    {
        return (int)(spawnCount + (level / 2));
    }

}
