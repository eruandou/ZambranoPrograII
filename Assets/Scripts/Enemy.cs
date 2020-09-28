using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
 
    public LifeController LifeController { get; private set; }

    [SerializeField] private int maxLife;

    private Animator anim;

    private float timerToDie = 1;

    public bool isActiveFramesOnAttack2 ;

    private AIController aIController;

    [SerializeField] private float stunTimeStart;
    private float stunTime;

    [SerializeField] private BoxCollider2D checkNearPlayer;

    [SerializeField] private float outOfAttackTimeStart;
    private float outOfAttackTime;

    private void Start()
    {
        LifeController = new LifeController(maxLife);
        anim = GetComponent<Animator>();
        aIController = GetComponent<AIController>();
        LifeController.OnGetDamage += OnGetDamageHandler;
        LifeController.OnDead += OnDeadHandler;
        checkNearPlayer.enabled = true;
    }

    public enum EnemyStates
    {
        Idle,
        Patrolling,
        Damaged,
        Attacking,
        Persuing,
        Die
    }

    public EnemyStates CurrentState { get; private set; }


    public void ChangeState(EnemyStates newState)
    {
        CurrentState = newState;

        switch (CurrentState)
        {
            case EnemyStates.Idle:
                anim.SetBool("Idle", true);
                anim.SetBool("Move", false);
                checkNearPlayer.enabled = true;
               
                Debug.Log("Idle");
                break;

            case EnemyStates.Patrolling:

                anim.SetBool("Idle", false);
                anim.SetBool("Move", true);
                checkNearPlayer.enabled = false;
              
                Debug.Log("Patrol");
                break;

            case EnemyStates.Damaged:

                anim.SetBool ("Idle", false);
                anim.SetBool("Move", false);
                anim.SetTrigger("Damaged");
                checkNearPlayer.enabled = false;
                Debug.Log("Damaged");
                break;

            case EnemyStates.Attacking:

          
                if (Random.value <= 0.7f)
                {
                    anim.SetTrigger("Attack1");
                    aIController.Attack1();                 
                }

                else
                {
                    anim.SetTrigger("Attack2");
                    aIController.Attack2();
                }
                checkNearPlayer.enabled = false;
                Debug.Log("Attacking");
                break;

            case EnemyStates.Persuing:
              
                anim.SetBool("Idle", false);
                anim.SetBool("Move", true);
                checkNearPlayer.enabled = false;
             
                Debug.Log("Persue");
                break;

            case EnemyStates.Die:
                Debug.Log("Die");
                anim.SetTrigger("Death");
                checkNearPlayer.enabled = false;
             
                break;

            default:
                break;
        }

    }


    private void Update()
    {
        LifeController.Update();

        
       // Debug.Log($"out of attack {outOfAttackTime} with {outOfAttackTimeStart} as start");


        switch (CurrentState)
        {
            case EnemyStates.Idle:
                
                
                break;
            case EnemyStates.Patrolling:
          
                aIController.AIUpdate();

                break;
            case EnemyStates.Damaged:

                Debug.Log($"stun time {stunTime} with {stunTimeStart} as start");

                stunTime += Time.deltaTime;

                if (stunTime >= stunTimeStart)
                {
                    stunTime = 0;
                    ChangeState(EnemyStates.Idle);
                }
               

                break;
            case EnemyStates.Attacking:

                outOfAttackTime += Time.deltaTime;

                if (outOfAttackTime >= outOfAttackTimeStart)
                {
                    aIController.DeactivateHurtBox();
                    outOfAttackTime = 0;
                    ChangeState(EnemyStates.Idle);

                }
             
                break;
            case EnemyStates.Persuing:
              
                aIController.AIUpdate();

                break;
            case EnemyStates.Die:
               
                timerToDie -= Time.deltaTime;
                if (timerToDie <= 0)
                {
                    Die();
                }
                break;

            default:
                break;
        }

    }

    private void Die()
    {
        Destroy(this.gameObject);
    }


    private void OnGetDamageHandler(int currentLife, int damage)
    {
        if (currentLife > 0) ChangeState(EnemyStates.Damaged);
    }

    private void OnDeadHandler()
    {
        ChangeState(EnemyStates.Die);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null && checkNearPlayer.enabled == true)
        {
            ChangeState(EnemyStates.Persuing);
        }
    }

}
