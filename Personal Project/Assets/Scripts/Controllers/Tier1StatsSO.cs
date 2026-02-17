using UnityEngine;

[CreateAssetMenu(fileName = "Tier1StatsSO", menuName = "Scriptable Objects/Tier1StatsSO")]
public class Tier1StatsSO : ScriptableObject
{
    // This will  be the default value initially
    public float speed = 1550;
    public float tackleForce = 10000;
    public float tackleCD = 3;

    // Scaling rules
    public float speedPerLevel = 100;
    public float tackleForcePerLevel = 50;
    public float tackleCDPerLevel = -0.1f;

    public int GetSpeed(int level) => (int)(speed + (speedPerLevel * level));
    public int GetTackleForce(int level) => (int)(tackleForce + (tackleForcePerLevel * level));
    public int GetTackleCD(int level) => (int)(tackleCD + (tackleCDPerLevel * level));

}
