using UnityEngine;
using System.Collections;

public class KeeperController : MonoBehaviour
{
    private Rigidbody KeeperRb;
    private Animator KeeperAnim;

    public Transform feetTarget;

    public float moveSpeed;
    public float landSpeed;
    public float rotationSpeed;
    public float followForce;
    public float jumpForce;
    public float gravityModifier;

    bool isTracing = true;
    bool isDiving = false;
    public bool isGrounded = true;
    public bool isBallTouched = false;
    public bool isActiveGT = false;
    public bool isCaught = false;
    public bool isIntercepting = false;
    public bool isAttachedSnap = true;
    public bool boolIntercepted = false;
    public bool isDebugged, isDebugged2 = false;
    public bool canInitiateCI = true;

    private bool applyLocalGravity = true;
    private bool applyRunTowardsBall = true;

    public float divingDelay;
    public float catchDelay;
    public float closeInterceptDelay;
    public float GroundLayDelay;

    public Transform Ball; // assign the FeetTarget transform
    public PlayerController playercontrollerScript;

    public BallRollFollow ballrollfollowScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        KeeperRb = GetComponent<Rigidbody>();
        KeeperAnim = GetComponent<Animator>();
        KeeperAnim.SetBool("Static_b", false);

        KeeperRb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isActiveGT == true)
        {
            // to update and transfer bool status for ground touch
            isActiveGT = true;
        }

        if (isCaught == true)
        {
            // to update and transfer bool status for catch
            isCaught = true;
        }

        if(isIntercepting == true)
        {
            // to update and transfer bool status for intercept
            isIntercepting = true;
        }

        if (boolIntercepted == true)
        {
            // to update and transfer bool status for intercepted for locking catch
            boolIntercepted = true;
        }

        if (canInitiateCI == false) 
        {
            // to make sure close intercept is unexecutable after other action is done
            canInitiateCI = false;
        }

        if (applyLocalGravity)
        {
            Vector3 customGravity = Vector3.down * 9.81f * gravityModifier;
            KeeperRb.AddForce(customGravity, ForceMode.Acceleration);
        }

        if(applyRunTowardsBall)
        {
            bool toDive = ballrollfollowScript.isReleased;

            if (isTracing == true)
            {
                // Animate based on Rigidbody velocity (horizontal only)
                Vector3 movement = new Vector3(KeeperRb.linearVelocity.x, 0, KeeperRb.linearVelocity.z);

                // variable for keeper rortation
                Vector3 rotation2player = playercontrollerScript.playerMovement;
                rotation2player.x = -rotation2player.x;

                Vector3 rotation2ball = Ball.position;

                // make sure facing only the left side
                if (rotation2player.x > 0)
                {
                    rotation2player.x = -rotation2player.x;
                }

                // Calculate movement speed (excluding vertical)
                //float currentSpeed = movement.magnitude;
                float currentSpeed = moveSpeed;
                KeeperAnim.SetFloat("Speed_f", currentSpeed);

                //rotate face direction
                if (rotation2player.sqrMagnitude > 0.0001f)
                {

                    // Ignore Y so keeper stays upright when rotating towards ball
                    Vector3 flatDir = new Vector3(rotation2ball.x, 0f, 0f);

                    if (toDive == true)
                    {
                        Quaternion toRotationBall = Quaternion.LookRotation(flatDir, Vector3.up);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotationBall, rotationSpeed * Time.deltaTime);
                    }
                    else
                    {
                        Quaternion toRotation = Quaternion.LookRotation(rotation2player, Vector3.up);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                    }
                }

                // Only follow when not diving
                if (!isDiving)
                {
                    // Direction from enemy to feet target
                    Vector3 direction = (Ball.position - transform.position);
                    direction.y = 0f; // optional: ignore vertical offset for grounded movement
                    direction.x = 0f;

                    // Apply force to follow the feet
                    KeeperRb.AddForce(direction.normalized * followForce);
                }
            }
        }

    }

    public void InitiateDive(Vector3 targetPosition)
    {
        if (!isDiving)
        {
            StartCoroutine(DivingRoutine(targetPosition));
        }
    }

    IEnumerator DivingRoutine(Vector3 targetPosition)
    {
        isDiving = true;
        applyRunTowardsBall = false;
        isTracing = false; 

        KeeperAnim.applyRootMotion = false;

        //KeeperRb.constraints &= ~RigidbodyConstraints.FreezePositionX;

        yield return new WaitForSeconds(divingDelay);

        // Optional: disable gravity or control physics
        // enemyRb.useGravity = false;
        // applyLocalGravity = false;

        Vector3 resetcustomGravity = Vector3.down * 0;
        KeeperRb.AddForce(resetcustomGravity, ForceMode.Acceleration);

        //stop run
        KeeperAnim.SetFloat("Speed_f", 0f);
        followForce = 0f;
        KeeperRb.linearVelocity = Vector3.zero;

        // Pose Tackle = Dive
        KeeperAnim.SetTrigger("Dive_trig");

        // Animate based on Rigidbody velocity (horizontal only)
        Vector3 movement = new Vector3(KeeperRb.linearVelocity.x, 0, KeeperRb.linearVelocity.z);
        // Calculate movement speed (excluding vertical)
        //float currentSpeed = movement.magnitude;
        //float currentSpeed = moveSpeed;
        float currentSpeed = 5000f;
        KeeperAnim.SetFloat("Speed_f", currentSpeed);
        followForce = 5000f;

        yield return new WaitForSeconds(0.5f);

        // Jump toward ball's last position
        Vector3 predictTargetPosition = ballrollfollowScript.keeperPredict;

        if (isDebugged == false)
        {
            Debug.Log("Predict Position: " + predictTargetPosition);
            isDebugged = true;
        }
        

        Vector3 jumpdirection = (predictTargetPosition - transform.position).normalized;
        //Vector3 direction = predictTargetPosition.normalized;
        //direction.y = 1f; // Add upward force for the jump arc

        //KeeperRb.linearVelocity = Vector3.zero; // Reset any current velocity
        KeeperRb.AddForce(jumpdirection * jumpForce, ForceMode.Impulse);
        // allow mass on gravity to land dive
        KeeperRb.AddForce(Vector3.down * gravityModifier * KeeperRb.mass, ForceMode.Force);

        isGrounded = false;

        // include function to (dive) (45 degree) rotate right or left base on the shot position
        //KeeperRb.constraints &= ~RigidbodyConstraints.FreezeRotationZ;

        if(jumpdirection.z > 0f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -45f);
            isActiveGT = true;
        }
        else if (jumpdirection.z < 0f) 
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 45f);
            isActiveGT = true;
        }



        // also for dive, release from last position constraint (maybe play around w/ speed/force 1st)... no need for now
        // 

        canInitiateCI = false;

    }

    public void InitiateGroundLay()
    {
        StartCoroutine(GroundLayRoutine());
    }

    // and upon touching ground, fixed horizontal position on that pose (90 degree)
    IEnumerator GroundLayRoutine()
    {
        Vector3 predictTargetPosition = ballrollfollowScript.keeperPredict;

        if (isDebugged2 == false)
        {
            Debug.Log("Predict Position: " + predictTargetPosition.z);
            isDebugged2 = true;
        }

        Vector3 jumpdirection = (predictTargetPosition - transform.position).normalized;

        if (jumpdirection.z > 0f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -80f);
        }
        else if (jumpdirection.z < 0f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 80f);
        }

        canInitiateCI = false;

        yield return new WaitForSeconds(GroundLayDelay);
    }

    public void InitiateCatched()
    {   
        StartCoroutine(CatchedRoutine());        
    }

    IEnumerator CatchedRoutine()
    {
        isBallTouched = true;
        
        // Access dive to catch transition
        KeeperAnim.SetBool("isBallTouch_bool", true);
        isCaught = true;

        // reposition rotation, keeper must land on feet (upright)... no need

        // include reposition of ball to keeper's hand
        // create object "hand_target" where ball will be reposition as caught
        // most  probably change the hitbox from knee level until hand extend during dive
        // allow logical area for catching ball on hand
        // leg will only deflect ball (w/ enough speed)

        canInitiateCI = false;

        yield return new WaitForSeconds(catchDelay);
    }

    public void InitiateCloseIntercept()
    {
        isIntercepting = true;
        isAttachedSnap = false;
        StartCoroutine(CloseInterceptRoutine());
    }

    // allow keeper to dash to player when too close
    IEnumerator CloseInterceptRoutine()
    {
        // 3. release keeper from a few position constraints
        KeeperRb.constraints = RigidbodyConstraints.None;
        KeeperRb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionY;
        //  (if want grounded only)

        // 1. access ball position
        Vector3 interceptPosition = feetTarget.position;

        Vector3 interceptDirection = (interceptPosition - transform.position);

        // 2. release ball from lock playerfeet (done by trigger)

        // 4. dashforce to position (follow vector)
        // revalue force (currently the dash force)(multiply from base jumpForce)
        jumpForce *= 1.2f;
        //KeeperRb.linearVelocity = Vector3.zero; // Reset any current velocity if needed
        KeeperRb.AddForce(interceptDirection * jumpForce, ForceMode.Impulse);
        KeeperAnim.SetBool("isIntercept", true);

        // 5. apply a part of CatchedRoutine inside this function (not all)
        boolIntercepted = true;

        // prevent/stop follow player
        applyRunTowardsBall = false;
        // 6. instant gameover function call (to be made later)

        // based on player's hitbox radius made (or new one for balancing)

        // instant gameover
        // 


        yield return new WaitForSeconds(closeInterceptDelay);
    }

    //Keeper thought process
    // Keeper movement
    // - Tracing position of ball (follow horizontally by moving vertically
    // - Upon palyer shoot, dive
    // Dive position based on shoot power, if low = y axis low value, if high = y axis high value
    // On Dive, restriction to line of ball tracing = disabled
    // - Upon player radius range near the goalpost or keeper, keeper automatically front tackle player/crouch down (choose 1 pose) & hardcoded ball placed on keeper's hand
    // Ball hit/struck/touch keeper = game over... later update
    // Extra cool stuff to apply,
    // Keeper lean on goalpost, wipe mouth, check watch


    // Animate sync control
    // When ball caught do landing
    // If ball untouched after dive, follow ball until touch
    // If ball goal, stay in dive position (touch ground, fully rotate horizontal)
    // 


    // conditions for fail shootout
    // - goal entry does not detect ball 3 secs after shootout
    // - keeper touch the ball
    // - wall out detection

    // Don't forget to disable shootball function when keeper caught ball
}
