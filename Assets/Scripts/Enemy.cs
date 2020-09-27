using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
 
    public LifeController LifeController { get; private set; }

    [SerializeField] private int maxLife;

    private Animator anim;

    private void Start()
    {
        LifeController = new LifeController(maxLife);
        anim = GetComponent<Animator>();
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




    public void ChangeState (EnemyStates newState)
    {
        CurrentState = newState;

        switch (CurrentState)
        {
            case EnemyStates.Idle:
                anim.SetBool("Idle", true);
                anim.SetBool("Move", false);
                break;
            case EnemyStates.Patrolling:
                anim.SetBool("Idle", false);
                anim.SetBool("Move", true);
                break;
            case EnemyStates.Damaged:
                anim.SetTrigger("Damaged");
                break;
            case EnemyStates.Attacking:

                if (Random.value <= 0.7f) anim.SetTrigger("Attack1");
                else anim.SetTrigger("Attack2");
                break;
            case EnemyStates.Persuing:
                anim.SetBool("Idle", false);
                anim.SetBool("Move", true);
                break;
            case EnemyStates.Die:
                anim.SetTrigger("Death");
                break;
            default:
                break;
        }

    }



    private void Update()
    {
        //debug

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ChangeState(EnemyStates.Idle);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            ChangeState(EnemyStates.Patrolling);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ChangeState(EnemyStates.Persuing);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            ChangeState(EnemyStates.Damaged);
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            ChangeState(EnemyStates.Attacking);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            ChangeState(EnemyStates.Die);
        }

    }
}
