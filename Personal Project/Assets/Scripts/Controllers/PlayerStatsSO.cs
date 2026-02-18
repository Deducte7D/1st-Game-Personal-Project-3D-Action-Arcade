using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Scriptable Objects/PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject
{
    // This will  be the default value initially
    public float speed = 2050;
    public float jump = 1000;

    // Scaling rules
    public float speedPerLevel = 100;
    public float jumpPerLevel = 50;

    public int GetSpeed(int level) => (int)(speed + (speedPerLevel * level));
    public int GetJump(int level) => (int)(jump + (jumpPerLevel * level));
}
