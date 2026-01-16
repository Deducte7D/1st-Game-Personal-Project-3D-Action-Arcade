using UnityEngine;

[CreateAssetMenu(fileName = "KeeperStatsSO", menuName = "Scriptable Objects/KeeperStatsSO")]
public class KeeperStatsSO : ScriptableObject
{
    // This will  be the default value initially
    public float speed = 5000;

    // Scaling rules
    public float speedPerLevel = 30;

    public int GetSpeed(int level) => (int)(speed + (speedPerLevel * level));

}

