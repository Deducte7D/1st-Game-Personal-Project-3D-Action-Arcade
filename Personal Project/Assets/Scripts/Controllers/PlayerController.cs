using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;
    private Animator playerAnim;
    //public float speed = 1550.0f;
    //public float jumpForce = 1000.0f;
    public bool isOnGround = true;
    public float rotationSpeed = 500f;
    public float gravityModifier;
    public bool gameOver = false;
    public MoveLeftBG movingGround;
    public DistanceCounter levelEndStatus;
    //public Transform playerPosInit; // the starting transform
    public Transform spawnPoint;
    public Vector3 playerSpawnPoint;
    public Vector3 playerMovement;

    private bool inputMovement = true;

    public PlayerStatsSO statsData;
    public LevelUpdater levelUpdater;
    private int currentLevel;

    // allow value inspection
    [SerializeField] public float speed;
    [SerializeField] public float jumpForceVar;

    // public property for any read-only
    public float SpeedForce => speed;
    public float JumpForce => jumpForceVar;

    public float initialSpeed;

    //public float followForce { get; private set; }
    //public float tackleForce { get; private set; }
    //public int maxHealth { get; private set; }

    public void Initialize(int level)
    {
        currentLevel = level;
        speed = statsData.GetSpeed(currentLevel);
        jumpForceVar = statsData.GetJump(currentLevel);

        initialSpeed = speed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerRb.useGravity = false;

        //GetInitPlayerPos(); // store player position when game/scene start

        currentLevel = levelUpdater.currentLevel;

        Initialize(currentLevel);

        //Physics.gravity *= gravityModifier;
        //playerRb.AddForce(Vector3.down * gravityModifier * playerRb.mass, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputMovement)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
            playerRb.AddForce(movement * speed);

            playerMovement = movement;

            // Calculate movement speed (excluding vertical)
            float currentSpeed = movement.magnitude;
            playerAnim.SetFloat("Speed_f", currentSpeed);

            //rotate face direction
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space) && isOnGround /*&& !gameOver*/)
            {
                //playerAudio.PlayOneShot(jumpSound, 1.0f);
                playerRb.AddForce(Vector3.up * jumpForceVar, ForceMode.Impulse);
                isOnGround = false;
                playerAnim.SetTrigger("Jump_trig");
                //dirtParticle.Stop();
            }
        }

    }

    void FixedUpdate()
    {
        Vector3 customGravity = Vector3.down * 9.81f * gravityModifier;
        playerRb.AddForce(customGravity, ForceMode.Acceleration);

        bool levelEnd = levelEndStatus.levelEnded;

        if (!levelEnd){
            float groundSpeed = movingGround.GetSpeed();

            Vector3 dragVelocity = Vector3.left * groundSpeed;
            playerRb.MovePosition(playerRb.position + dragVelocity * Time.fixedDeltaTime);
        }
        

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            //dirtParticle.Play();
        }
        //else if (collision.gameObject.CompareTag("Obstacle"))
        //{
        //    gameOver = true;
        //    playerAudio.PlayOneShot(crashSound, 1.0f);
        //    Debug.Log("Game Over!");
        //    playerAnim.SetBool("Death_b", true);
        //    playerAnim.SetInteger("DeathType_int", 1);
        //    explosionParticle.Play();
        //    dirtParticle.Stop();
        //}
    }

    public void DeathHandler()
    {
        // --- Slow Motion ---
        StartCoroutine(SlowMotionDeath());

        // --- PHYSICS CHANGES ---

        // Reduce weight/mass
        playerRb.mass = 0.3f;

        // Remove drag (damping)
        playerRb.linearDamping = 0f;
        playerRb.angularDamping = 0f;

        // Remove rotation constraints
        playerRb.constraints = RigidbodyConstraints.None;


        // disable animation
        // playerAnim.enabled = false;
        playerAnim.SetFloat("Speed_f", 0f);
        playerAnim.SetInteger("Animation_int",9);

        // --- DISABLE PLAYER CONTROL ---
        //if (this != null)
        //    this.enabled = false;
        inputMovement = false;

        Debug.Log("Player died - physics + control changed + slow mo");
    }

    private IEnumerator SlowMotionDeath()
    {
        // Slow down time
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Stay slow for 1.5 seconds (real time)
        yield return new WaitForSecondsRealtime(1.5f); // why use waitforsecondsrealtime, ignores timescale

        // Return to normal speed
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    // method just to get the spawn position of player transform to use back for next level
    //public void GetInitPlayerPos()
    //{
    //    playerPosInit = gameObject.transform;
    //    playerSpawnPoint = playerPosInit.position;
    //}

    public void PlayerPosToInit()
    {
        //playerRb.MovePosition(playerSpawnPoint);
        playerRb.position = spawnPoint.position;
        playerRb.linearVelocity = Vector3.zero; // clear movement
        //playerRb.MovePosition(spawnPoint.position);
    }

}
