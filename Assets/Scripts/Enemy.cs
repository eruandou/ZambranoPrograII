using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
 
    public LifeController LifeController { get; private set; }

    [SerializeField] private int maxLife;

    private Animator anim;
    private SpriteRenderer sprRend;
    private Rigidbody2D rb;   

    private float timerToDie;

    public bool isActiveFramesOnAttack2 ;

    [SerializeField] private int pointsToGive;

    public AIController AIController { get; private set; }

    [SerializeField] private float stunTimeStart;
    private float stunTime;

    [SerializeField] private BoxCollider2D checkNearPlayer;

    [SerializeField] private float outOfAttackTimeStart;
    private float outOfAttackTime;

    [Range(0, 100)] public int chanceToSpawnItem;



    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip grunt;
    private float timerToGrunt;

    private AudioSource audioSrc;

    public Color neutralColor;

    private void Start()
    {
        LifeController = new LifeController(maxLife);
        anim = GetComponent<Animator>();
        AIController = GetComponent<AIController>();
        LifeController.OnGetDamage += OnGetDamageHandler;
        LifeController.OnDead += OnDeadHandler;
        checkNearPlayer.enabled = true;
        audioSrc = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        sprRend = GetComponent<SpriteRenderer>();        
        timerToDie = dieSound.length;
        NewTimerToGrunt();
    }

    public enum EnemyStates
    {
        Idle,
        Patrolling,
        Damaged,
        Attacking,
        Persuing,
        Frozen,
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
                ChangeDetectionState(true);

                rb.constraints = RigidbodyConstraints2D.None;
                anim.enabled = true;
                sprRend.color = neutralColor;
                

                break;

            case EnemyStates.Patrolling:

                anim.SetBool("Idle", false);
                anim.SetBool("Move", true);
                ChangeDetectionState(false);


                break;

            case EnemyStates.Damaged:

                anim.SetBool("Idle", false);
                anim.SetBool("Move", false);
                anim.SetTrigger("Damaged");
                ChangeDetectionState(false);


                break;

            case EnemyStates.Attacking:


                if (Random.value <= 0.7f)
                {
                    anim.SetTrigger("Attack1");
                    AIController.Attack1();
                }

                else
                {
                    anim.SetTrigger("Attack2");
                    AIController.Attack2();
                }
                ChangeDetectionState(false);

                break;

            case EnemyStates.Persuing:

                anim.SetBool("Idle", false);
                anim.SetBool("Move", true);
                checkNearPlayer.enabled = false;
                audioSrc.clip = grunt;

               
                break;

            case EnemyStates.Die:

                anim.SetTrigger("Death");
                checkNearPlayer.enabled = false;
                audioSrc.clip = dieSound;
                audioSrc.pitch = 1;
                audioSrc.Play();

                break;
            case EnemyStates.Frozen:
                rb.constraints = RigidbodyConstraints2D.FreezeAll;               
                anim.enabled = false;
                sprRend.color = Color.blue;                
                ChangeDetectionState(false);

                break;
            default:
                break;
        }

    }


    private void ChangeDetectionState(bool isDetecting)
    {
        checkNearPlayer.enabled = isDetecting;
        AIController.detectPlayerBoxEnabled = isDetecting;
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

                AIController.AIUpdate();

                break;
            case EnemyStates.Damaged:



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
                    AIController.DeactivateHurtBox();
                    outOfAttackTime = 0;
                    ChangeState(EnemyStates.Idle);

                }

                break;
            case EnemyStates.Persuing:

                AIController.AIUpdate();
                timerToGrunt -= Time.deltaTime;
                if (timerToGrunt <= 0)
                {
                    audioSrc.pitch = Random.Range(0.7f, 1.1f);
                    audioSrc.Play();
                    NewTimerToGrunt();
                }


                break;
            case EnemyStates.Die:

                timerToDie -= Time.deltaTime;
                if (timerToDie <= 0)
                {
                    Die();
                }
                break;
            case EnemyStates.Frozen:

                break;
            default:
                break;
        }

    }

    private void Die()
    {       
        Gamemanager.instance.OnEnemyDie(pointsToGive);
        Gamemanager.instance.enemiesController.ItemToDrop(this.transform.position, chanceToSpawnItem);
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

    private void NewTimerToGrunt()
    {
        timerToGrunt = Random.Range(5f, 9f);
    }

}
