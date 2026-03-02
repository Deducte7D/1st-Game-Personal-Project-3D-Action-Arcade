using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static ObjectPooler;
using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.GraphicsBuffer;


public class EnemyControllerT3 : MonoBehaviour
{

    private Rigidbody enemyRb;
    private Animator enemyAnim;
    public float rotationSpeed = 500f;
    //public float cacheFollowForce = 2500f; // stay and increase
    //public float followForce = 2500f;
    //public float tackleForce = 20000f;

    //public float spinForce = 50f;
    public GameObject feetTarget; // assign the FeetTarget transform
    public GameObject plane;
    
    public float tackleCooldown = 2f;
    private bool canTackle = true;
    public bool isOnGround = true;
    public MoveLeftBG movingGround;
    //public PlayerController gravityModifier;
    public float gravityModifier;
    private bool applyLocalGravity = true;
    private bool applySuperYGravity = false; //bool for object burst floated from impact
    public float maxYGravityModifier = 5f;

    private bool isTackling = false;
    private bool isSlide = false;

    public float slowMotionTimeScale;
    private float startTimeScale;
    private float startFixedDeltaTime;

    public float smashSpeed = 100f;
    public float jumpForce = 5000f;
    public float jumpDelay = 0f;
    public float landDelay;
    
    public bool isJumping = false;
    //public Animator anim;

    private bool isSageMode = false;
    public bool isSpecialing = false;
    public float specialDelay = 5f;

    //public GameObject bunshinPrefab1;
    //public GameObject bunshinPrefab2;

    // Variables for tremor
    public float landTremorRadius = 5f;
    public int maxDamage = 50;
    public float knockForce = 15f;
    public float upwardForce = 5f;
    public float rippleSpeed = 25f;
    public LayerMask damageableLayers;

    public BunshinPoolManager bunshinPool;
    public SlowMoController slowMoScript;
    public SpawnManagerV2 spawnManagerV2;

    public Tier3StatsSO statsData;
    //public LevelUpdater levelUpdater; // basically not used
    private int currentLevel;

    // allow value inspection
    [SerializeField] private float followForce;
    [SerializeField] private float tackleForce;
    [SerializeField] private int maxHealth;

    // public property for any read-only
    public float FollowForce => followForce;
    public float TackleForce => tackleForce;
    public int MaxHealth => maxHealth;

    public float initialFollowForce;

    [Header("Boundary Settings")]
    public Transform wallBorderLeft;
    public Transform enemyT3BoundarySpawnPoint;
    [SerializeField] private float minY = -20f;
    [SerializeField] private float maxY = 21f;


    //public float followForce { get; private set; }
    //public float tackleForce { get; private set; }
    //public int maxHealth { get; private set; }

    public void Initialize(int level)
    {
        currentLevel = level;
        followForce = statsData.GetSpeed(currentLevel);
        tackleForce = statsData.GetTackleForce(currentLevel);
        maxHealth = statsData.GetMaxHealth(currentLevel);
        initialFollowForce = followForce;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        enemyAnim = GetComponent<Animator>();
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;
        feetTarget = GameObject.FindWithTag("Player"); // assign target at runtime for prefab
        plane = GameObject.FindWithTag("Ground"); 
        movingGround = plane.GetComponent<MoveLeftBG>(); // reassign for prefab

        enemyRb.useGravity = false;

        if (wallBorderLeft == null)
        {
            wallBorderLeft = GameObject.Find("BorderLeftForPlayer").transform;
        }

        if (enemyT3BoundarySpawnPoint == null)
        {
            enemyT3BoundarySpawnPoint = GameObject.Find("SpawnBoundaryT3").transform;
        }
        //bunshinRb.AddForce(Vector3.down * gravityModifier * bunshinRb.mass, ForceMode.Force);

    }

    void OnEnable()
    {
        // attack are recurring already prob do not need.
        // but to prevent enemybunshin disable visual bug, better reset attack routine
        // should be on tacketriggerT3.cs
        //followForce = cacheFollowForce;
        canTackle = true;
        isOnGround = true;
        isJumping = false;
        isTackling = false;
        isSlide = false;
        isSageMode = false;
        isSpecialing = false;

        if (spawnManagerV2 != null)
        {
            spawnManagerV2.isAliveT3 = true;
        }
        
}

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (wallBorderLeft == null)
        {
            wallBorderLeft = GameObject.Find("BorderLeftForPlayer").transform;
        }

        if (enemyT3BoundarySpawnPoint == null)
        {
            enemyT3BoundarySpawnPoint = GameObject.Find("SpawnBoundaryT3").transform;
        }

        // for fall of map
        if (transform.position.y < minY)
        {
            ResetPositionBoundary();
        }

        // dot product : if negative, enemy is behind wall
        Vector3 toEnemy = transform.position - wallBorderLeft.position;
        float dot = Vector3.Dot(wallBorderLeft.forward, toEnemy);
        //Debug.Log("Dot value: " + dot);

        if (dot < -23f) // enemy behind wall condition
        {
            ResetPositionBoundary();
        }

        if (slowMoScript == null)
        {
            slowMoScript = FindFirstObjectByType<SlowMoController>();
        }

        if (bunshinPool == null)
        {
            bunshinPool = FindFirstObjectByType<BunshinPoolManager>();
        }

        if (spawnManagerV2 == null)
        {
            spawnManagerV2 = FindFirstObjectByType<SpawnManagerV2>();
        }

        if (transform.position.y > maxY)
        {
            applySuperYGravity = true;
            applyLocalGravity = false;
        }
        else if (transform.position.y < maxY)
        {
            applySuperYGravity = false;
            applyLocalGravity = true;
        }

        if (!isSlide)
        {
            // Animate based on Rigidbody velocity (horizontal only)
            Vector3 movement = new Vector3(enemyRb.linearVelocity.x, 0, enemyRb.linearVelocity.z);

            // Calculate movement speed (excluding vertical)
            float currentSpeed = movement.magnitude;
            enemyAnim.SetFloat("Speed_f", currentSpeed);

            //rotate face direction
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }



        // Only follow when not tackling
        if (!isTackling)
        //if (!isTackling && !isSlide)
        {
            // Direction from enemy to feet target
            Vector3 direction = (feetTarget.transform.position - transform.position);
            direction.y = 0f; // optional: ignore vertical offset for grounded movement

            // Apply force to follow the feet
            enemyRb.AddForce(direction.normalized * followForce);
        }

        //// Optional: Match rotation with player (if you want enemy to rotate too)
        //transform.rotation = feetTarget.rotation;

    }

    void FixedUpdate()
    {
        if (applySuperYGravity == true)
        {
            Vector3 customGravity = Vector3.down * 9.81f * maxYGravityModifier;
            enemyRb.AddForce(customGravity, ForceMode.Acceleration);
        }

        if(applyLocalGravity)
        {
            Vector3 customGravity = Vector3.down * 9.81f * gravityModifier;
            enemyRb.AddForce(customGravity, ForceMode.Acceleration);
        }
        

        float groundSpeed = movingGround.GetSpeed();

        Vector3 dragVelocity = Vector3.left * groundSpeed;
        enemyRb.MovePosition(enemyRb.position + dragVelocity * Time.fixedDeltaTime);

    }

    public void InitiateSpecialAttack(Vector3 targetPosition, Transform target)
    {
        if (!isSageMode)
        {
            StartCoroutine(SpecialAttackRoutine(targetPosition, target));
        }
    }


    // Coroutine for facing player during special atk
    IEnumerator RotateTowardsTarget(Transform target, float duration, float rotationSpeed)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            Vector3 dir = target.position - transform.position;
            dir.y = 0; // Optional, if you want to ignore vertical angle

            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }

    IEnumerator SpecialAttackRoutine(Vector3 targetPosition, Transform target)
    {
        isSageMode = true;
        isSpecialing = true;

        yield return new WaitForSeconds(specialDelay);

        // Optional: disable gravity or control physics
        // bunshinRb.useGravity = false;
        applyLocalGravity = false;

        Vector3 resetcustomGravity = Vector3.down * 0;
        enemyRb.AddForce(resetcustomGravity, ForceMode.Acceleration);

        //stop run
        enemyAnim.SetFloat("Speed_f", 0f);
        followForce = 0f;
        enemyRb.linearVelocity = Vector3.zero;

        // cross arm
        enemyAnim.SetInteger("Animation_int", 1);

        // slowly float and reposition on middle to back position facing movement, then look forward
        // Slowly float up and move to targetPosition
        float duration = 12f; // time to float and move 12f
        float elapsed = 0f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(-1.4f, 6.2f, 3.34f); // floating in air
                                                               // Vector3 endPosition = new Vector3(targetPosition.x, targetPosition.y + 3f, targetPosition.z); // floating in air

        float spawnTimer = 0f;
        int spawnedCount = 0;
        int totalClones = 6;
        float spawnInterval = 1f;

        //StartCoroutine(RotateTowardsTarget(target, duration, rotationSpeed));

        while (elapsed < duration)
        {
            //StartCoroutine(RotateTowardsTarget(target, duration, rotationSpeed));

            //Vector3 centerPos = (startPosition + endPosition) * 0.5f;
            //Vector3 dir = target.position - centerPos;

            // Rotation toward target
            Vector3 dir = (target.position - transform.position);
            dir.y = 0; // Keep horizontal
            if (dir.sqrMagnitude > 0.001f)
            //if (Vector3.Angle(transform.forward, dir) > 1f) // Rotate if angle difference is more than 1 degree
            {
                Quaternion targetRot = Quaternion.LookRotation(dir.normalized); //.normalized
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * (Time.deltaTime * 2)); // Time.deltatime * 2 = faster changes on target dir
            }

            elapsed += Time.deltaTime;

            // Smooth position transition
            //transform.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            //float s = Mathf.Clamp01(elapsed / duration);
            //transform.position = Vector3.Lerp(startPosition, endPosition, t);
            // Position Interpolation
            enemyRb.MovePosition(Vector3.Lerp(startPosition, endPosition, t));

            // --- Spawn clones while floating ---
            spawnTimer += Time.deltaTime;
            if (spawnedCount < totalClones && spawnTimer >= spawnInterval) // ngl the second condition is pelik
            {
                spawnTimer = 0f;

                //Vector3 spawnPos = transform.position +
                //                   new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

                //// Pick randomly between two prefabs
                //GameObject prefabToSpawn = (Random.value < 0.4f) ? bunshinPrefab1 : bunshinPrefab2;

                //Instantiate(prefabToSpawn, spawnPos, transform.rotation);

                // Spawn from bunshin pool
                Vector3 spawnPos = transform.position +
                                   new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

                string[] enemyTypes = { "EBunshinT1", "EBunshinT2" };
                string chosenTypeSpawn = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Length)];

                // spawn method call

                GameObject bunshin = bunshinPool.GetBunshin(chosenTypeSpawn, spawnPos, Quaternion.identity);

                spawnedCount++;
            }


            yield return null; // Always yield so coroutine continues
        }


        // Ensure the object is exactly at endPosition
        //transform.position = endPosition;

        // Optional: Hold position in air for a second
        //yield return new WaitForSeconds(0f);

        //// Restore original constraints
        //bunshinRb.constraints = originalConstraints;

        // spawns bunshin
        // stay that position until 1,2,3 (total 6) bunshin is spawned
        
        //int totalClones = 6;
        //float spawnInterval = 1f; // seconds between spawns

        //for (int i = 0; i < totalClones; i++)
        //{
        //    // Pick a spawn position (example: around the enemy)
        //    Vector3 spawnPos = transform.position +
        //                       new Vector3(Random.Range(-2f, 2f), 3.1f, Random.Range(-2f, 2f));

        //    // Draft version using Instantiate
        //    Instantiate(bunshinPrefab, spawnPos, transform.rotation);

        //    // Wait before spawning next one
        //    yield return new WaitForSeconds(spawnInterval);
        //}


        // then join fight as normal
        // uncross arm
        enemyAnim.SetInteger("Animation_int", 0);

        // reset normal run
        // enable gravity back
        enemyRb.WakeUp();
        //bunshinRb.useGravity = true;
        //Vector3 customGravity = Vector3.down * 9.81f * gravityModifier;
        //bunshinRb.AddForce(customGravity, ForceMode.Acceleration);
        applyLocalGravity = true;

        // Animate based on Rigidbody velocity (horizontal only)
        Vector3 movement = new Vector3(enemyRb.linearVelocity.x, 0, enemyRb.linearVelocity.z);
        // Calculate movement speed (excluding vertical)
        float currentSpeed = movement.magnitude;
        enemyAnim.SetFloat("Speed_f", currentSpeed);
        followForce = initialFollowForce;

        isSageMode = false;

        yield return new WaitForSeconds(2f);

        isSpecialing = false;
    }

    public void InitiateJumpAttack(Vector3 targetPosition)
    {
        if(!isJumping)
        {
            StartCoroutine(JumpAttackRoutine(targetPosition));
        }
    }

    IEnumerator JumpAttackRoutine(Vector3 targetPosition)
    {
        isJumping = true;

        yield return new WaitForSeconds(jumpDelay);

        enemyAnim.SetFloat("Speed_f", 0f);
        followForce = 0f;

        // Optional: trigger jump animation
        enemyAnim.SetTrigger("Jump_trig");

        // Animate based on Rigidbody velocity (horizontal only)
        Vector3 movement = new Vector3(enemyRb.linearVelocity.x, 0, enemyRb.linearVelocity.z);
        // Calculate movement speed (excluding vertical)
        float currentSpeed = movement.magnitude;
        enemyAnim.SetFloat("Speed_f", currentSpeed);
        followForce = initialFollowForce;

        // smooth direction and correct distance update

        // Jump toward player's last position
        // Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 toTarget = targetPosition - transform.position;
        Vector3 direction = toTarget.normalized;
        float distance = toTarget.magnitude;

        direction.y = 1f; // Add upward force for the jump arc

        enemyRb.linearVelocity = Vector3.zero; // Reset any current velocity
        // enemyRb.AddForce(direction * jumpForce, ForceMode.Impulse);
        enemyRb.AddForce(direction * distance * jumpForce);

        // Wait for landing
        yield return new WaitForSeconds(landDelay); // Adjust timing based on jump length

        enemyRb.linearVelocity = Vector3.down * smashSpeed; // instantly smash

        // Call Tremor
        DoLandTremor();

        // Tremor Damage Area
        // Collider[] hitPlayers = Physics.OverlapSphere(transform.position, landTremorRadius, damageableLayers);

        //foreach (Collider col in hitPlayers)
        //{
        //    Debug.Log("Tremor hit: " + col.name);
        //    // You can apply damage or knockback here
        //}

        //// Optional: play landing/tremor animation
        //anim.SetTrigger("Land");

        yield return new WaitForSeconds(4f);

        isJumping = false;

    }

    // Optional: visualize the tremor radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, landTremorRadius);
    }

    public void DoLandTremor()
    {
        // Camera shake happens ONCE
        //CameraShake.Shake(0.3f, 0.5f);

        Collider[] hitPlayers = Physics.OverlapSphere(
            transform.position,
            landTremorRadius,
            damageableLayers
        );

        foreach (Collider col in hitPlayers)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);

            // Delay based on distance transfers to ripple effect
            float delay = dist / rippleSpeed;

            StartCoroutine(ApplyTremorWithDelay(col, dist, delay));
        }
    }

    private IEnumerator ApplyTremorWithDelay(Collider col, float dist, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Damage falloff
        float damagePercent = 1 - (dist / landTremorRadius); // basically further away from radius = lower percentage dmg taken
        int finalDamage = Mathf.RoundToInt(maxDamage * damagePercent);

        // Apply damage
        PlayerHealth hp = col.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(finalDamage);

        // Apply force (tremor feel)
        Rigidbody rb = col.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (col.transform.position - transform.position).normalized;
            dir.y = 0f;

            Vector3 force = dir * knockForce + Vector3.up * upwardForce;
            rb.AddForce(force, ForceMode.Impulse);
        }

        // basically later for visual effects, the tremor effect will be seen at the same time the player is dmged (sync), so it depends on the distance to determine the speed/delay.
        // AND make sure to have the visual do 1 then 2 wave first and 3rd will hit, therefore there's timing there real bad bussiness this dirty work
    }

    private void StartSlowMotion()
    {
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
    }

    private void EndSlowMotion()
    {
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
    }


    // Tackle Trigger
    public void StartSlideTackleT3(Transform target)
    {

        if (!canTackle) return;

        Vector3 tackleDir = (target.position - transform.position).normalized;
        tackleDir.y = 0f;

        //Quaternion targetRot = Quaternion.LookRotation(tackleDir.normalized);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        //Debug.DrawRay(transform.position, tackleDir * 10f, Color.red, 1f);

        isTackling = true;
        isSlide = true;

        enemyRb.linearVelocity = Vector3.zero; // Stop previous momentum
        enemyRb.AddForce(tackleDir * tackleForce, ForceMode.Impulse);
        slowMoScript.StartSlowMotion(tackleCooldown); // currently 1 sec
        //bunshinRb.mass = 30f;
        //playerAnim.speed = 0.6f; // slows animation to give heavy feel
        //bunshinRb.linearDamping = 3f;     // add this temporarily if needed
        //playerAnim.SetTrigger("Tackle_trig");
        enemyAnim.SetBool("Tackle_bool", true);

        Debug.Log("Rotate -90.");
        transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, 0f);


        //// Force reset rotation before tackle
        //transform.rotation = Quaternion.Euler(-90f, GetLookAngle(tackleDir), 0f);


        canTackle = false;
        StartCoroutine(ResetTackle(tackleCooldown));
    }

    // Reset Coroutine
    private System.Collections.IEnumerator ResetTackle(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        isTackling = false;
        isSlide = false;
        
        enemyAnim.SetBool("Tackle_bool", false);
        //playerAnim.SetBool("Crouch_up", true);
        enemyAnim.SetBool("Crouch_up", false);

        // Wait extra delay before allowing next tackle
        float standDelay = 2f; // seconds to stand up before next tackle
        //EndSlowMotion();
        //bunshinRb.linearDamping = 2f;     // add this temporarily if needed
        //bunshinRb.mass = 70f;
        //playerAnim.speed = 1f; // slows animation to give heavy feel
        yield return new WaitForSeconds(standDelay);

        canTackle = true;


    }

    //    private float GetLookAngle(Vector3 dir)
    //{
    //    if (dir == Vector3.zero) return transform.eulerAngles.y;
    //    return Quaternion.LookRotation(dir).eulerAngles.y;
    //}

    //private System.Collections.IEnumerator DelayAction(float delay)
    //{
    //    yield return new WaitForSeconds(delay);


    //}

    public void DeathHandlerT3()
    {
        // smoke effect = dead visualization

        // disable T3
        gameObject.SetActive(false);
        spawnManagerV2.EnemyT3StatusDead();
    }

    public void ResetPositionBoundary()
    {
        transform.position = enemyT3BoundarySpawnPoint.position;
        transform.rotation = enemyT3BoundarySpawnPoint.rotation;

    }

}
