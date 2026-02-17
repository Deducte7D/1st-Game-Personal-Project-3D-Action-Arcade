using UnityEngine;
using UnityEngine.Events;

public class EnemyT3Health : MonoBehaviour
{
    //public Rigidbody enemyT3rb;
    public EnemyControllerT3 enemyT3Controller;

    public int maxHealth; // 11 hits by bunshin
    public int currentHealth; 
    private int currentLevel;

    public UnityEvent<int, int> onHealthChanged; // current, max
    public UnityEvent onDeath;

    public Tier3StatsSO statsData;

    public SpawnManagerV2 spawnManagerV2;

    public LevelUpdater levelUpdater;

    //private int currentLevel;

    //public int maxHealth { get; private set; }

    //public void Initialize(int level)
    //{
    //    currentLevel = level;
    //    maxHealth = statsData.GetMaxHealth(currentLevel);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //enemyT3rb = GetComponent<Rigidbody>();
        EnemyControllerT3 enemyT3Controller = GetComponent<EnemyControllerT3>();

        SpawnManagerV2 spawnManagerV2 = GetComponent<SpawnManagerV2>();

        if (levelUpdater == null)
        {
            levelUpdater = FindFirstObjectByType<LevelUpdater>();
        }

        if (levelUpdater != null)
        {
            currentLevel = levelUpdater.currentLevel;
            maxHealth = statsData.GetMaxHealth(currentLevel);
        }

        //maxHealth = enemyT3Controller.MaxHealth;

        currentHealth = maxHealth;

        if (onHealthChanged != null)
            onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelUpdater != null)
        {
            currentLevel = levelUpdater.currentLevel;
            maxHealth = statsData.GetMaxHealth(currentLevel);
        }

        if (spawnManagerV2 == null)
        {
            spawnManagerV2 = FindFirstObjectByType<SpawnManagerV2>();
        }

        //// MaxHealth is the read-only property // bug hp is 700 when enabled
        //maxHealth = enemyT3Controller.MaxHealth;
        
    }

    public void TakeDamageT3(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log("Enemy T3 took: " + dmg);

        //if (currentHealth <= 0)
        //    Die();

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (onHealthChanged != null)
            onHealthChanged.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0)
        {
            if (onDeath != null)
                onDeath.Invoke();
            enemyT3Controller.DeathHandlerT3();
        }
    }

    protected void Die()
    {
        spawnManagerV2.isAliveT3 = false;

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;

        if (onHealthChanged != null)
            onHealthChanged.Invoke(currentHealth, maxHealth);
    }

}
