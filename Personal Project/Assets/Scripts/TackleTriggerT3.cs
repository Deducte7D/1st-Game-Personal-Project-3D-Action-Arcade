using UnityEngine;
using System.Collections;

public class TackleTriggerT3 : MonoBehaviour
{
    public EnemyControllerT3 enemy;
    public bool noMoreTackle = false;
    public int maxCount = 0;
    private PlayerHitbox playerHitboxScript;
    //private Rigidbody radiusTrig;
    public GameObject otherObject;

    public enum AttackPhase { Tackle, Jump, Special}
    public AttackPhase currentPhase = AttackPhase.Tackle; //AttackPhase.Tackle

    private int tackleMisses = 0; // 0
    private int jumpMisses = 0;
    private int specialMisses = 0;
    //private bool isStruck = false;



    void Start()
    {
        //radiusTrig = GetComponent<Rigidbody>();
        ;

        // Assume ScriptA is on the same GameObject
        //playerHitboxScript = GetComponent<PlayerHitbox>();
        playerHitboxScript = otherObject.GetComponent<PlayerHitbox>();

        // Or if it's on another object (drag in via Inspector)
        // public GameObject otherObject;
        // scriptARef = otherObject.GetComponent<ScriptA>();


    }

    

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered by: " + other.name);
        //int counts = playerHitboxScript.tackleHits; //read hits count
        bool isStruck = playerHitboxScript.isHit;

        if (!other.CompareTag("WifeEnemy")) return;

        switch (currentPhase)
        {
            case AttackPhase.Tackle:
                
                bool isSpecialPending = enemy.isSpecialing;
                if (!noMoreTackle && !isStruck && !isSpecialPending)
                {
                    EnemyControllerT3 enemy = other.GetComponent<EnemyControllerT3>();
                    if (enemy != null)
                    {
                        //StartCoroutine(DelayBeforeAtk());
                        enemy.StartSlideTackleT3(transform.root); // send player transform as target
                        tackleMisses++;

                        if (tackleMisses >= 2) //3 times
                        {
                            tackleMisses = 0;
                            currentPhase = AttackPhase.Jump;
                            Debug.Log("Switching to Jump Attack phase.");
                        }
                    }
                }
                break;

            case AttackPhase.Jump:

                bool isJumpingPending = enemy.isJumping;
                if (!isStruck && !isJumpingPending)
                {
                    enemy.InitiateJumpAttack(transform.position);
                    jumpMisses++;

                    if(jumpMisses >= 1) //2 times
                    {
                        jumpMisses = 0;
                        currentPhase = AttackPhase.Special;
                        Debug.Log("Switching to Special Attack phase.");
                    }
                }
                break;

            case AttackPhase.Special:
                if (!isStruck)
                {
                    enemy.InitiateSpecialAttack(transform.position, transform);
                    specialMisses++;
                    

                    if (specialMisses >= 1)
                    {
                        specialMisses = 0;
                        // After special attack, reset to tackle phase
                        currentPhase = AttackPhase.Tackle;
                        Debug.Log("Switching to Tackle Attack phase.");
                    }

                    

                    //isStruck = false; // optional: reset hit detection
                }
                break;
        }

    }

    IEnumerator DelayBeforeAtk()
    {
        yield return new WaitForSeconds(2f);  // delay for 1 second
    }

}
