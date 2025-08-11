using UnityEngine;

public class EnemyControllerT1 : MonoBehaviour
{

    private Rigidbody enemyRb;
    private Animator playerAnim;
    public float rotationSpeed = 500f;
    public float followForce = 1550f;
    //public float spinForce = 50f;
    public Transform feetTarget; // assign the FeetTarget transform
    public float tackleForce = 5000f;
    public float tackleCooldown = 3f;
    private bool canTackle = true;
    public bool isOnGround = true;
    public MoveLeftBG movingGround;
    public float gravityModifier;
    private bool isTackling = false;
    private bool isSlide = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        //enemyRb.AddForce(Vector3.down * gravityModifier * enemyRb.mass, ForceMode.Force);
        feetTarget = GameObject.Find("FeetTarget").transform;
        if (movingGround == null)
        {
            movingGround = FindFirstObjectByType<MoveLeftBG>();
        }
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

    private System.Collections.IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

}
