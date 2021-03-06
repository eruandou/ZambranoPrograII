﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
 
    public LifeController LifeController { get; private set; }

    [SerializeField] private int maxLife;

    public event Action<Enemy> OnDie;

    private int chosenAttack = 1;

    private Animator anim;
    private SpriteRenderer sprRend;
    private Rigidbody2D rb;   

    private float timerToDie;        

    [SerializeField] private int pointsToGive, extraTime;
    public AIController AIController { get; private set; }

    [SerializeField] private float stunTimeStart;
    private float stunTime;

    [SerializeField] private BoxCollider2D checkNearPlayer,collisionBox;
    

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
                AIController.DeactivateHurtBox();
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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

                ChangeDetectionState(false);

                if (chosenAttack == 1)
                {
                    anim.SetTrigger("Attack1");                       
                }

                else
                {
                    anim.SetTrigger("Attack2");
                    AIController.Attack2();
                }

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
                collisionBox.enabled = false;
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

    public void ChangeNextAttack (int nextAttack)
    {
        chosenAttack = nextAttack;
    }

    private void ChangeDetectionState(bool isDetecting)
    {
        checkNearPlayer.enabled = isDetecting;
        AIController.detectPlayerBoxEnabled = isDetecting;
    }

    private void Update()
    {
        LifeController.Update();


      


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
        OnDie?.Invoke(this);
        Gamemanager.instance.OnEnemyDieHandler(pointsToGive, extraTime);
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
