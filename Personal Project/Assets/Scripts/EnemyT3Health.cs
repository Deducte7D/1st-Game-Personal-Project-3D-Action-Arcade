using UnityEngine;
using UnityEngine.Events;

public class EnemyT3Health : MonoBehaviour
{
    //public Rigidbody enemyT3rb;
    public EnemyControllerT3 enemyT3Controller;

    public int maxHealth = 550; // 11 hits by bunshin
    public int currentHealth;

    public UnityEvent<int, int> onHealthChanged; // current, max
    public UnityEvent onDeath;

    public Tier3StatsSO statsData;

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

        currentHealth = maxHealth;
        if (onHealthChanged != null)
            onHealthChanged.Invoke(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth = enemyT3Controller.maxHealth;
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
