using UnityEngine;

public class EnemyControllerT1 : MonoBehaviour
{

    private Rigidbody enemyRb;
    private Animator playerAnim;
    public float rotationSpeed = 500f;
    //public float spinForce = 50f;
    public Transform feetTarget; // assign the FeetTarget transform
    //public float followForce = 1550f;
    //public float tackleForce = 5000f;
    //public float tackleCooldown = 3f;
    private bool canTackle = true;
    public bool isOnGround = true;
    public MoveLeftBG movingGround;
    public float gravityModifier;
    private bool isTackling = false;
    private bool isSlide = false;

    public Tier1StatsSO statsData;
    private int currentLevel;

    public float followForce { get; private set; }
    public float tackleForce { get; private set; }
    public float tackleCooldown { get; private set; }

    public void Initialize(int level)
    {
        currentLevel = level;
        followForce = statsData.GetSpeed(currentLevel);
        tackleForce = statsData.GetTackleForce(currentLevel);
        tackleCooldown = statsData.GetTackleCD(currentLevel);
    }

    //public void EnemyT1IncrementStats(float waveCount, float levelCount)
    //{
    //    float level = levelCount;
    //    followForce = followForce + (100 * level);
    //    tackleForce = tackleForce + (100 * level);
    //    tackleCooldown = tackleCooldown + (-0.1f * level);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (statsData == null) 
        //{ 
        //    Debug.LogError("StatsData not assigned on " + gameObject.name);
        //    return; 
        //}

        enemyRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        //bunshinRb.AddForce(Vector3.down * gravityModifier * bunshinRb.mass, ForceMode.Force);
        feetTarget = GameObject.Find("FeetTarget").transform;
        if (movingGround == null)
        {
            movingGround = FindFirstObjectByType<MoveLeftBG>();
        }
    }

    void OnEnable()
    {
        // attack are recurring already prob do not need
        isTackling = false;
        isSlide = false;
        canTackle = true;

    }

    private void OnDisable()
    {

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
            playerAnim.SetFloat("Speed_f", currentSpeed);

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

        float groundSpeed = movingGround.GetSpeed();

        Vector3 dragVelocity = Vector3.left * groundSpeed;
        enemyRb.MovePosition(enemyRb.position + dragVelocity * Time.fixedDeltaTime);

    }


    // Tackle Trigger
    public void StartSlideTackleT1(Transform target)
    {
        if (!canTackle) return;

        Vector3 tackleDir = (target.position - transform.position).normalized;
        tackleDir.y = 0f;

        //Debug.DrawRay(transform.position, tackleDir * 10f, Color.red, 1f);

        isTackling = true;
        isSlide = true;
        enemyRb.linearVelocity = Vector3.zero; // Stop previous momentum
        enemyRb.AddForce(tackleDir * tackleForce, ForceMode.Impulse);
        //playerAnim.SetTrigger("Tackle_trig");
        playerAnim.SetBool("Tackle_bool", true);
        transform.rotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, 0f);
        //playerAnim.enabled = false;
        // Lay back by rotating 90 degrees around X-axis
        //if (transform.parent != null)
        //{
        //    // After rotating parent
        //    Transform parent = transform.parent;
        //transform.parent.rotation = Quaternion.Euler(90f, transform.parent.rotation.eulerAngles.y, 0f);
        //    // Adjust Y position to ground level (you may need to tweak the Y value)
        //    Vector3 pos = parent.position;
        //    pos.y = 0f; // or another value that puts the model at ground level
        //    parent.position = pos;
        //}

        canTackle = false;
        StartCoroutine(ResetTackle(5f));

        StartCoroutine(DisableAfterDelay(gameObject, 2.5f));
    }
    
    // Reset Coroutine
    private System.Collections.IEnumerator ResetTackle(float delay)
    {
        yield return new WaitForSeconds(tackleCooldown);
        isTackling = false;
        isSlide = false;
        canTackle = true;
        playerAnim.SetBool("Tackle_bool", false);
        //playerAnim.SetBool("Crouch_up", true);
        playerAnim.SetBool("Crouch_up", false);

    }

    // disable gameobject after tackle and a few delay
    private System.Collections.IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false); 
    }

}

// Comment reset tackle function
// T1 enemy poof after 1 tackle
