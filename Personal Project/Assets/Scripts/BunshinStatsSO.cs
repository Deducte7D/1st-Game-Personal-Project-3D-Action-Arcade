using UnityEngine;

[CreateAssetMenu(fileName = "BunshinStatsSO", menuName = "Scriptable Objects/BunshinStatsSO")]
public class BunshinStatsSO : ScriptableObject
{
    // This will  be the default value initially
    public float speed = 1500;
    public float tackleForce = 5000;
    //public float tackleCD = 3;

    // Scaling rules
    public float speedPerLevel = 30;
    public float tackleForcePerLevel = 50;
    //public float tackleCDPerLevel = -0.1f;

    public int GetSpeed(int level) => (int)(speed + (speedPerLevel * level));
    public int GetTackleForce(int level) => (int)(tackleForce + (tackleForcePerLevel * level));
    //public int GetTackleCD(int level) => (int)(tackleCD + (tackleCDPerLevel * level));

}

