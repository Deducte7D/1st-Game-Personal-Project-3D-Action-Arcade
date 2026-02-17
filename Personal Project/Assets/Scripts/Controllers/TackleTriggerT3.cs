using UnityEngine;
using System.Collections;

public class TackleTriggerT3 : MonoBehaviour
{
    public GameObject PlayerHitBox;
    private PlayerHitbox playerHitboxScript;


    public bool noMoreTackle = false;
    public int maxCount = 0;
    
    //private Rigidbody radiusTrig;
    

    public enum AttackPhase { Tackle, Jump, Special}
    public AttackPhase currentPhase = AttackPhase.Tackle; //AttackPhase.Tackle

    private int tackleMisses = 0; // 0
    private int jumpMisses = 0;
    private int specialMisses = 0;
    //private bool isStruck = false;

    void OnEnable()
    {
        // attack are recurring already prob do not need.
        // but to prevent enemybunshin disable visual bug, better reset attack routine
        // should be on tacketriggerT3.cs

        currentPhase = AttackPhase.Tackle; // reset to current phase back to AttackPhase.Tackle

        // can comment if needed for testing later
    }

    void Start()
    {
        //radiusTrig = GetComponent<Rigidbody>();

        // Assume ScriptA is on the same GameObject
        //playerHitboxScript = GetComponent<PlayerHitbox>();
        playerHitboxScript = PlayerHitBox.GetComponent<PlayerHitbox>();
        

        // Or if it's on another object (drag in via Inspector)
        // public GameObject otherObject;
        // scriptARef = otherObject.GetComponent<ScriptA>();

    }

    void OnTriggerEnter(Collider other) // other is other than the collider itself that attached to this obj/script (its considered as a component just like in inspector)
    {
        
        //Debug.Log("Trigger entered by: " + other.name);
        //int counts = playerHitboxScript.tackleHits; //read hits count
        bool isStruck = playerHitboxScript.isHit;

        if (!other.CompareTag("WifeEnemy")) return;

        EnemyControllerT3 enemyT3 = other.GetComponent<EnemyControllerT3>();
        if (enemyT3 == null) return;

        switch (currentPhase)
        {
            case AttackPhase.Tackle:
                
                bool isSpecialPending = enemyT3.isSpecialing;
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

                bool isJumpingPending = enemyT3.isJumping;
                if (!isStruck && !isJumpingPending)
                {
                    enemyT3.InitiateJumpAttack(transform.position);
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
                    enemyT3.InitiateSpecialAttack(transform.position, transform);
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
