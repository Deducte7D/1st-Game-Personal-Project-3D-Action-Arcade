using UnityEngine;

public class LevelUpdater : MonoBehaviour
{
    public int currentLevel = 0;

    public void LevelUp()
    {
        currentLevel++;
        // Optionally notify pooled objects to refresh stats
    }
}
