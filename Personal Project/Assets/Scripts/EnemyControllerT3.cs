using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemyControllerT3 : MonoBehaviour
{

    private Rigidbody enemyRb;
    private Animator enemyAnim;
    public float rotationSpeed = 500f;
    public float followForce = 2100f;
    //public float spinForce = 50f;
    public Transform feetTarget; // assign the FeetTarget transform
    public float tackleForce = 20000f;
    public float tackleCooldown = 3f;
    private bool canTackle = true;
    public bool isOnGround = true;
    public MoveLeftBG movingGround;
    //public PlayerController gravityModifier;
    public float gravityModifier;
    private bool applyLocalGravity = true;

    private bool isTackling = false;
    private bool isSlide = false;

    public float slowMotionTimeScale;
    private float startTimeScale;
    private float startFixedDeltaTime;

    public float smashSpeed = 100f;
    public float jumpForce = 5000f;
    public float landTremorRadius = 5f;
    public float jumpDelay = 0f;
    public LayerMask damageableLayers;
    public bool isJumping = false;
    //public Animator anim;

    private bool isSageMode = false;
    public bool isSpecialing = false;
    public float specialDelay = 0f;

    public GameObject bunshinPrefab1;
    public GameObject bunshinPrefab2;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        enemyAnim = GetComponent<Animator>();
        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;

        enemyRb.useGravity = false;
        //enemyRb.AddForce(Vector3.down * gravityModifier * enemyRb.mass, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {

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
            Vector3 direction = (feetTarget.position - transform.position);
            direction.y = 0f; // optional: ignore vertical offset for grounded movement

            // Apply force to follow the feet
            enemyRb.AddForce(direction.normalized * followForce);
        }

        //// Optional: Match rotation with player (if you want enemy to rotate too)
        //transform.rotation = feetTarget.rotation;

    }

    void FixedUpdate()
    {
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
        // enemyRb.useGravity = false;
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
            if (spawnedCount < totalClones && spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;

                Vector3 spawnPos = transform.position +
                                   new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

                // Pick randomly between two prefabs
                GameObject prefabToSpawn = (Random.value < 0.4f) ? bunshinPrefab1 : bunshinPrefab2;

                Instantiate(prefabToSpawn, spawnPos, transform.rotation);

                spawnedCount++;
            }


            yield return null; // Always yield so coroutine continues
        }


        // Ensure the object is exactly at endPosition
        //transform.position = endPosition;

        // Optional: Hold position in air for a second
        //yield return new WaitForSeconds(0f);

        //// Restore original constraints
        //enemyRb.constraints = originalConstraints;

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
        //enemyRb.useGravity = true;
        //Vector3 customGravity = Vector3.down * 9.81f * gravityModifier;
        //enemyRb.AddForce(customGravity, ForceMode.Acceleration);
        applyLocalGravity = true;

        // Animate based on Rigidbody velocity (horizontal only)
        Vector3 movement = new Vector3(enemyRb.linearVelocity.x, 0, enemyRb.linearVelocity.z);
        // Calculate movement speed (excluding vertical)
        float currentSpeed = movement.magnitude;
        enemyAnim.SetFloat("Speed_f", currentSpeed);
        followForce = 2100f;

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
        followForce = 2100f;

        // Jump toward player's last position
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 1f; // Add upward force for the jump arc

        enemyRb.linearVelocity = Vector3.zero; // Reset any current velocity
        enemyRb.AddForce(direction * jumpForce, ForceMode.Impulse);

        // Wait for landing
        yield return new WaitForSeconds(1.5f); // Adjust timing based on jump length

        enemyRb.linearVelocity = Vector3.down * smashSpeed; // instantly smash

        // Tremor Damage Area
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, landTremorRadius, damageableLayers);

        foreach (Collider col in hitPlayers)
        {
            Debug.Log("Tremor hit: " + col.name);
            // You can apply damage or knockback here
        }

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
        StartSlowMotion();
        //enemyRb.mass = 30f;
        //playerAnim.speed = 0.6f; // slows animation to give heavy feel
        //enemyRb.linearDamping = 3f;     // add this temporarily if needed
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
        EndSlowMotion();
        //enemyRb.linearDamping = 2f;     // add this temporarily if needed
        //enemyRb.mass = 70f;
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

}
