using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Image fillImage;
    public PlayerHealth playerHealth;

    void Start()
    {
        UpdateHealthBar(playerHealth.currentHealth, playerHealth.maxHealth);
    }

    public void UpdateHealthBar(int current, int max)
    {
        float percent = (float)current / max;
        fillImage.fillAmount = percent;
    }
}
