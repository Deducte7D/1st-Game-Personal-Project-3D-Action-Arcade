using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BunshinController : MonoBehaviour
{
    // Basically reworked controller from enemy T2

    private Rigidbody bunshinRb;
    private Animator playerAnim;

    public float rotationSpeed = 500f;

    //public float spinForce = 50f;

    //public float followForce; // used in method
    //public float tackleForce = 5000f;
    public float tackleCooldown = 1.5f;
    public float startTackleDelay = 2f;
    public float gravityModifier = 12f;

    public bool isOnGround = true;
    public bool foundTarget = false;

    private bool canTackle = true;
    private bool isTackling = false;
    private bool isSlide = false;
    public bool isDetected = false;

    public MoveLeftBG movingGround;
    public DistanceCounter distanceCounter;

    public Transform feetTarget; // assign the FeetTarget transform
    public Transform target = null; // the target that will be filled
    public Transform playerPos; // assign player's transform

    public float detectionRadius = 40f;

    public Vector3 storeDirection;

    public BunshinTackleTrigger receiverScript; // reference to ScriptB

    public Vector3 storeVelocity;

    public LayerMask enemyLayer;

    private Vector3 defaultLinearVelocity;
    private RigidbodyConstraints defaultConstraints;
    private float defaultLinearDamping;
    private float defaultAngularDamping;
    private float defaultMass;

    public float maxKnockbackSpeed;
    public float maxChaseSpeed;

    public BunshinStatsSO statsData;
    private int currentLevel;

    // allow value inspection
    [SerializeField] private float followForce;
    [SerializeField] private float tackleForce;

    [SerializeField] private float minY = -10f;

    // public property for any read-only
    public float FollowForce => followForce;
    public float TackleForce => tackleForce;

    //public float followForce { get; private set; }
    //public float tackleForce { get; private set; }

    //public float tackleCooldown { get; private set; }

    public void Initialize(int level)
    {
        currentLevel = level;
        followForce = statsData.GetSpeed(currentLevel);
        tackleForce = statsData.GetTackleForce(currentLevel);
        //tackleCooldown = statsData.GetTackleCD(level);
    }

    private void OnEnable()
    {
        target = null;
        foundTarget = false;
        isTackling = false;
        isSlide = false;

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(bunshinRb.position, bunshinRb.position + bunshinRb.linearVelocity);
    //}


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bunshinRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        //bunshinRb.AddForce(Vector3.down * gravityModifier * bunshinRb.mass, ForceMode.Force);
        feetTarget = GameObject.Find("FeetTarget").transform;
        playerPos = GameObject.Find("Player").transform;
        movingGround = FindFirstObjectByType<MoveLeftBG>();
        storeVelocity = bunshinRb.linearVelocity;
        bunshinRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Save defaults
        defaultLinearVelocity = bunshinRb.linearVelocity;
        defaultConstraints = bunshinRb.constraints;
        defaultLinearDamping = bunshinRb.linearDamping;
        defaultAngularDamping = bunshinRb.angularDamping;
        defaultMass = bunshinRb.mass;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTackling == false && target == null)
        {
            ChasePlayer();
        }
        
        if (target == null || !target.gameObject.activeInHierarchy)
        {
            FindTarget();
        }

        if (distanceCounter == null)
        {
            distanceCounter = FindFirstObjectByType<DistanceCounter>();
        }

        if (transform.position.y < minY)
        {
            DequeueFallOffMap();
        }

        // for movement and rotation
        if (!isSlide)
        {
            // Animate based on Rigidbody velocity (horizontal only)
            Vector3 movement = new Vector3(bunshinRb.linearVelocity.x, 0, bunshinRb.linearVelocity.z);

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
        
    }

    void FixedUpdate()
    {
        // Apply extra gravity only if knocked back or rising
        //if (bunshinRb.linearVelocity.y > 0.1f)
        //{
        //    bunshinRb.AddForce(Physics.gravity * gravityModifier, ForceMode.Acceleration);
        //}

        if (target != null && target.gameObject.activeInHierarchy)
        {
            ChaseTarget();
        }

        ////if (distanceCounter.levelEnded == false) 
        //if (!distanceCounter.levelEnded)
        //{
        //    // To drag object on plane
        //    float groundSpeed = movingGround.GetSpeed();

        //    Vector3 dragVelocity = Vector3.left * groundSpeed;
        //    // bunshinRb.MovePosition(bunshinRb.position + dragVelocity * Time.fixedDeltaTime);
        //    bunshinRb.AddForce(bunshinRb.position + dragVelocity * Time.fixedDeltaTime);
        //}

    }
    void FindTargetNew()
    {
        // Collect all colliders within detection radius
        //Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        float closestDist = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider col in hits)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);

            // Update immediately if this one is closer
            if (dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = col.transform;
            }
        }

        // Assign target (null if none found)
        target = closestEnemy;

        // Pass target to another script
        receiverScript.ReceiveTransform(target);
    }


    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        // initializes closestDist with the value of positive infinity.
        float closestDist = Mathf.Infinity; // ensures that after checking all objects, closestDist holds the smallest actual distance.
        
        Transform closestEnemy = null;

        foreach (Collider col in hits)
        {
            if (col.CompareTag("EnemyT1") || col.CompareTag("EnemyT2") || col.CompareTag("WifeEnemy"))
            {
                // calculate distance between the collided object and the bunshin
                float dist = Vector3.Distance(transform.position, col.transform.position);

                // replaces to smallest distance, basically closest enemy should be targeted
                // and initiate target detection of the collided obj
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestEnemy = col.transform;
                }

                //isDetected = true;
            }
        }

        if (closestEnemy != null && closestEnemy.gameObject.activeInHierarchy)
        {
            target = closestEnemy;
            receiverScript.ReceiveTransform(target);
            ChaseTarget();
        }
        else
        {
            target = null;
            ChasePlayer();
        }

    }

    void ChaseTarget()
    {
        foundTarget = true;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.magnitude > 0.1f) // prevent stall
        {
            bunshinRb.AddForce(dir.normalized * followForce, ForceMode.Force);
            //bunshinRb.AddForce(dir.normalized * followForce, ForceMode.VelocityChange);
            //bunshinRb.linearVelocity = Vector3.ClampMagnitude(bunshinRb.linearVelocity, maxChaseSpeed);
            //bunshinRb.linearVelocity = dir.normalized * followForce;


            bunshinRb.linearDamping = defaultLinearDamping;
            bunshinRb.angularDamping = defaultAngularDamping;
        }
        else
        {
            // fallback: keep moving toward player or forward
            Vector3 fallbackDir = (playerPos.position - transform.position).normalized;
            bunshinRb.AddForce(fallbackDir * followForce, ForceMode.Force);
            //bunshinRb.linearVelocity = Vector3.ClampMagnitude(bunshinRb.linearVelocity, maxKnockbackSpeed);
        }

    }

    void ChasePlayer()
    {
        // reset target make sure
        target = null;

        // to generate random seed for each bunshin to make distance to follow player
        System.Random rng = new System.Random(gameObject.GetInstanceID());

        float ranRange;

        if (UnityEngine.Random.value < 0.4f) // return randome float between 0.0 - 1.0, this is to randomize spawn on X and Z position
        {
            // Pick from -15 to -15
            // ranRange = Random.Range(-15f, -5f);
            ranRange = (float)(rng.NextDouble() * 10 - 15); // scale of 10, shift -15 therefore start at -5 
        }
        else
        {
            // Pick from 5 to 15
            // ranRange = Random.Range(5f, 15f);
            ranRange = (float)(rng.NextDouble() * 10 + 5); // scale of 10, shift +5 therefore start at 15
        }

        Vector3 rangeVicinity = new Vector3(ranRange, 0f, ranRange);

        // a new direction where bunshin will stay around the area of the player when not detected
        // Vector3 playerVicinity = playerPos.position + new Vector3(Random.Range(-20f, 20f), 0f, Random.Range(-20f, 20f));
        Vector3 playerVicinity = playerPos.position + rangeVicinity;
        // hopefully each bunshin has their own randomized position that does not require loop

        // Direction from bunshin to targeted player vicinity 
        Vector3 direction = (playerVicinity - transform.position);
        direction.y = 0f; // optional: ignore vertical offset for grounded movement

        storeDirection = direction;

        ApplyForceChasePlayer();
    }

    void ApplyForceChasePlayer()
    {
        if (storeDirection.magnitude > 0.1f)
        {
            bunshinRb.AddForce(storeDirection.normalized * (followForce * 0.333f), ForceMode.Force);
            //bunshinRb.linearVelocity = Vector3.ClampMagnitude(bunshinRb.linearVelocity, maxKnockbackSpeed);
        }
        if (storeDirection.magnitude < 0.1f)
        {
            storeDirection = (playerPos.position - transform.position).normalized;
            bunshinRb.AddForce(storeDirection.normalized * followForce, ForceMode.Force);
            //bunshinRb.linearVelocity = Vector3.ClampMagnitude(bunshinRb.linearVelocity, maxKnockbackSpeed);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Tackle Trigger
    public void StartSlideTackleBunshin(bool isExist, Transform target)
    {
        if (isExist && target != null)
        {
            // Debug.Log("TackleBunshinTriggered");

            if (!canTackle) return;

            new WaitForSeconds(startTackleDelay);

            Vector3 tackleDir = (target.position - transform.position).normalized;
            tackleDir.y = 0f;

            //Debug.DrawRay(transform.position, tackleDir * 10f, Color.red, 1f);

            isTackling = true;
            isSlide = true;
            bunshinRb.constraints = RigidbodyConstraints.FreezePositionY;
            bunshinRb.linearVelocity = Vector3.zero; // Stop previous momentum
            bunshinRb.AddForce(tackleDir * tackleForce, ForceMode.Impulse);
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
            //foundTarget = false;
            StartCoroutine(ResetTackle(5f));
            bunshinRb.constraints &= ~RigidbodyConstraints.FreezePositionY; // remove Y freeze
        }

        return;
    }

    // Reset Coroutine
    private System.Collections.IEnumerator ResetTackle(float delay)
    {
        yield return new WaitForSeconds(tackleCooldown);
        bunshinRb.linearVelocity = storeVelocity;
        isTackling = false;
        isSlide = false;
        canTackle = true;
        playerAnim.SetBool("Tackle_bool", false);
        //playerAnim.SetBool("Crouch_up", true);
        playerAnim.SetBool("Crouch_up", false);

        // Restore defaults
        bunshinRb.linearVelocity = defaultLinearVelocity; // or Vector3.zero if you want a clean reset
        bunshinRb.constraints = defaultConstraints;
        bunshinRb.linearDamping = defaultLinearDamping;
        bunshinRb.angularDamping = defaultAngularDamping;
        bunshinRb.mass = defaultMass;

    }

    private void DequeueFallOffMap()
    {

    }
}
