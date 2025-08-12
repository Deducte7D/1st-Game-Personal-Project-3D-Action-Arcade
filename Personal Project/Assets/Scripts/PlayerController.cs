using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private Rigidbody playerRb;
    private Animator playerAnim;
    public float speed = 1550.0f;
    public float jumpForce = 1000.0f;
    public bool isOnGround = true;
    public float rotationSpeed = 500f;
    public float gravityModifier;
    public bool gameOver = false;
    public MoveLeftBG movingGround;
    public DistanceCounter levelEndStatus;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerRb.useGravity = false;

        //Physics.gravity *= gravityModifier;
        //playerRb.AddForce(Vector3.down * gravityModifier * playerRb.mass, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        playerRb.AddForce(movement * speed);

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
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            //dirtParticle.Stop();
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

}
