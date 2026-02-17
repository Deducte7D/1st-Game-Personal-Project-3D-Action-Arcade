using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// attached to object that triggers it, therefore this script is on playerhitbox object instead of player object
public class PlayerHealth : MonoBehaviour
{
    //public Rigidbody playerRb;
    public PlayerController playerController;

    public int maxHealth = 100;
    public int currentHealth;

    public UnityEvent <int, int> onHealthChanged; // current, max
    public UnityEvent onDeath;

    public PowerUpManager powerUpManager;

    public DamageFlashUI damageFlash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //playerRb = GetComponent<Rigidbody>();
        PlayerController playerController = GetComponent<PlayerController>();

        currentHealth = maxHealth;
        if (onHealthChanged != null)
            onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int dmg)
    {

        if (powerUpManager != null && powerUpManager.isShieldActive)
        {
            Debug.Log("Shield active – no damage taken");
            return; //skips this function
        }

        currentHealth -= dmg;
        Debug.Log("Player took: " + dmg);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (onHealthChanged != null)
            onHealthChanged.Invoke(currentHealth, maxHealth);

        if (damageFlash != null)
            damageFlash.Flash();

        if (currentHealth == 0)
        {
            if (onDeath != null)
                onDeath.Invoke();
            playerController.DeathHandler();
            damageFlash = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // guane 
    }

    public void ResetPlayerHP()
    {
        currentHealth = maxHealth; // reset hp back to preset maxhealth
    }

    //private void DeathHandler()
    //{
    //    // --- Slow Motion ---
    //    StartCoroutine(SlowMotionDeath());

    //    // --- PHYSICS CHANGES ---

    //    // Reduce weight/mass
    //    playerRb.mass = 0.3f;

    //    // Remove drag (damping)
    //    playerRb.linearDamping = 0f;
    //    playerRb.angularDamping = 0f;

    //    // Remove rotation constraints
    //    playerRb.constraints = RigidbodyConstraints.None;

    //    // --- DISABLE PLAYER CONTROL ---
    //    if (playerController != null)
    //        playerController.enabled = false;

    //    Debug.Log("Player died - physics + control changed + slow mo");
    //}

    //private IEnumerator SlowMotionDeath()
    //{
    //    // Slow down time
    //    Time.timeScale = 0.2f;
    //    Time.fixedDeltaTime = 0.02f * Time.timeScale;

    //    // Stay slow for 1.5 seconds (real time)
    //    yield return new WaitForSecondsRealtime(1.5f); // why use waitforsecondsrealtime, ignores timescale

    //    // Return to normal speed
    //    Time.timeScale = 1f;
    //    Time.fixedDeltaTime = 0.02f;
    //}
}
