using UnityEngine;

[CreateAssetMenu(fileName = "Tier3StatsSO", menuName = "Scriptable Objects/Tier3StatsSO")]
public class Tier3StatsSO : ScriptableObject
{
    // This will  be the default value initially
    public float speed = 2500f;
    public float tackleForce = 20000f;
    public float health = 550;

    // Scaling rules
    public float speedPerLevel = 50;
    public float tackleForcePerLevel = 1000;
    public float healthPerLevel = 150;

    public int GetSpeed(int level) => (int)(speed + (speedPerLevel * level));
    public int GetTackleForce(int level) => (int)(tackleForce + (tackleForcePerLevel * level));
    public int GetMaxHealth(int level) => (int)(health + (healthPerLevel * level));

}
