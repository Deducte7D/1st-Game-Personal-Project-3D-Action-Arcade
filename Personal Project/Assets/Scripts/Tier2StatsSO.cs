

using UnityEngine;

[CreateAssetMenu(fileName = "Tier2StatsSO", menuName = "Scriptable Objects/Tier2StatsSO")]
public class Tier2StatsSO : ScriptableObject
{
    // This will  be the default value initially
    public float speed = 2050;
    public float tackleForce = 10000;
    public float tackleCD = 3;

    // Scaling rules
    public float speedPerLevel = 150;
    public float tackleForcePerLevel = 50;
    public float tackleCDPerLevel = -0.1f;

    public int GetSpeed(int level) => (int)(speed + (speedPerLevel * level));
    public int GetTackleForce(int level) => (int)(tackleForce + (tackleForcePerLevel * level));
    public int GetTackleCD(int level) => (int)(tackleCD + (tackleCDPerLevel * level));

}

