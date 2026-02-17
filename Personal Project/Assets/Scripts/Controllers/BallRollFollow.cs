using UnityEngine;

public class BallRollFollow : MonoBehaviour
{
    public KeeperController keeperScript;
    public GameObject Keeper;

    public SpawnManagerV2 spawnScript;

    public Transform feetTarget; // assign the FeetTarget transform
    public GameObject CatchTarget; // not used for prefab
    public Rigidbody ballRb;

    public float followForce = 10f;
    public float spinForce = 50f;

    private bool isAttached = true; // Ball starts attached
    public bool isReleased = false; // Bool transfer for trigger
    public bool isCatched = false;
    public bool isKeeperIntercept = false;
    public bool isIntercepted = false;
    public bool isIntercepting = false;
    public bool checkKeeperSpawn;

    private float shootPower = 0f;
    private float maxShootPower = 300f;
    private float chargeRate = 120f; // how fast it charges

    public Vector3 ballPositionPredict = new Vector3(0,0,0);
    public Vector3 keeperPredict = new Vector3(0, 0, 0);

    public Camera aimCamera;  // Assign this in the Inspector


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        SpawnManagerV2 spawnScript = GetComponent<SpawnManagerV2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkKeeperSpawn = spawnScript.isKeeperInstanced;

        if (checkKeeperSpawn)
        {
            Keeper = GameObject.Find("Keeper_Prefab(Clone)");
            keeperScript = Keeper.GetComponent<KeeperController>();
            CatchTarget = GameObject.FindWithTag("CatchTarget");
        }

        

        //// Direction from ball to feet target
        //Vector3 direction = (feetTarget.position - transform.position);
        //direction.y = 0f; // optional: ignore vertical offset for grounded movement

        //// Apply force to follow the feet
        //ballRb.AddForce(direction.normalized * followForce);

        isIntercepting = keeperScript.isIntercepting;
        // in keeper script initialized " true " to allow snapping
        // however, during intercept, transfer false to release ball
        if (isIntercepting == true)
        {
            isAttached = keeperScript.isAttachedSnap;
        }

        if (isAttached && feetTarget != null)
        {
            //Stick to player feet
            // Match position exactly
            transform.position = feetTarget.position;

            // Add spinning torque for realism (e.g., roll along x-axis)
            Vector3 forwardDir = feetTarget.forward;
            ballRb.AddTorque(forwardDir * spinForce);

            //// Optional: Match rotation with player (if you want ball to rotate too)
            //transform.rotation = feetTarget.rotation;
        }

        isCatched = keeperScript.isCaught;

        if (isReleased && isCatched && CatchTarget.transform.position != null)
        {
            //Stick to keeper hand
            // Match position exactly
            transform.position = CatchTarget.transform.position;
            ballRb.angularVelocity = Vector3.zero;
            followForce = 0f;
            spinForce = 0f;
            // apply constraints to prevent moving
            ballRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        }

        isKeeperIntercept = keeperScript.isIntercepting;
        isIntercepted = keeperScript.boolIntercepted;

        if (isKeeperIntercept)
        {
            // release constraints first (ball initial constrainst are released), and release from snapping position
            // reposition after intercept fully executed
            
            if (isIntercepted)
            {
                transform.position = CatchTarget.transform.position;
                // Debug.Log("Position of the catch targert: " + CatchTarget.transform.position);
                ballRb.angularVelocity = Vector3.zero;
                followForce = 0f;
                spinForce = 0f;
                // apply constraints to prevent moving
                ballRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
                
            }
            
        }


    }

    void Update()
    {
        // Charge shot
        if (Input.GetKey(KeyCode.Q) && isAttached)
        {
            shootPower += chargeRate * Time.deltaTime;
            shootPower = Mathf.Clamp(shootPower, 0f, maxShootPower);
            //Debug.Log("Charging Power: " + shootPower);
        }

        // Release ball
        if (Input.GetKeyUp(KeyCode.Q) && isAttached)
        {
            ShootBall0();
        }

        // Check prediction position
        if (ballPositionPredict != Vector3.zero) {
            keeperPredict = ballPositionPredict;
        }
    }

    void ShootBall0()
    {
        isAttached = false;

        Ray ray = aimCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 shootDirection = (hit.point - transform.position).normalized;
            //Vector3 shootDirection = hit.point.normalized;
            ballPositionPredict = hit.point;
            ballRb.isKinematic = false;
            ballRb.AddForce(shootDirection * shootPower, ForceMode.Impulse);
        }

        //// 1. Get mouse position in screen space
        //Vector3 mousePos = Input.mousePosition;

        //// 2. Assign depth (distance into the scene from the camera)
        //mousePos.z = 60f; // adjust based on camera distance

        //// 3. Convert mouse screen position into world space
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        //// 4. Calculate direction from player/object to world mouse position
        ////Vector3 shootDirection = (worldPos - transform.position).normalized;
        //Vector3 shootDirection = worldPos.normalized;

        //// Store shoot direction for keeper prediction
        //ballPositionPredict = worldPos;

        //// 5. Apply force to the ball
        //ballRb.isKinematic = false;
        //ballRb.AddForce(shootDirection * shootPower, ForceMode.Impulse);

        //Debug.Log("Shot ball towards: " + worldPos + " with power: " + shootPower);

        Debug.Log("Shot ball towards: " + hit.point + " with power: " + shootPower);

        isReleased = true;

        shootPower = 0f; // reset
    }

    public void BallBoolReset()
    {
        isAttached = true; // Ball starts attached
        isReleased = false; // Bool transfer for trigger

        // reference to keepercontroller's bool check that pls
        isCatched = false; 
        isKeeperIntercept = false;
        isIntercepted = false;
        isIntercepting = false;

        // reset ball position after all bool reset
        // better call reset of keepercontroller first before executing the reset position
        // can even call the method here if not include the init animation of the keeper

    }

    public void BallPosReset()
    {
        // seems like when the bool reset, conditions can re apply ball to feettarget

    }

    //void ShootBall()
    //{
    //    isAttached = false;

    //    // Direction towards mouse world position
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    Vector3 shootDirection;

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        shootDirection = (hit.point - transform.position).normalized;
    //    }
    //    else
    //    {
    //        shootDirection = Camera.main.transform.forward; // fallback
    //    }

    //    ballRb.isKinematic = false;
    //    ballRb.AddForce(shootDirection * shootPower, ForceMode.Impulse);

    //    Debug.Log("Shot ball with power: " + shootPower);

    //    shootPower = 0f; // reset
    //}

    //public void AttachBall(Transform target)
    //{
    //    isAttached = true;
    //    feetTarget = target;
    //    ballRb.isKinematic = true; // so it doesn't roll away
    //}

}
