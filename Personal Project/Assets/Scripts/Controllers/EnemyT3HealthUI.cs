using UnityEngine;
using UnityEngine.UI;

public class EnemyT3HealthUI : MonoBehaviour
{
    public Image fillImage;
    public EnemyT3Health enemyT3Health;

    void Start()
    {
        UpdateHealthBar(enemyT3Health.currentHealth, enemyT3Health.maxHealth);
    }

    public void UpdateHealthBar(int current, int max)
    {
        float percent = (float)current / max;
        fillImage.fillAmount = percent;
    }
}
