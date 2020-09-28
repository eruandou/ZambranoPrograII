using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    [SerializeField] private float speed;
    private Enemy enemyParentComponent;
        
    [SerializeField] private int attack1Damage;
    [SerializeField] private int attack2Damage;

    [SerializeField] private Spores spores;

    [SerializeField] private int enemyLevel;

    [SerializeField] private BoxCollider2D hurtBox;

    [SerializeField] private float distanceToAttack;
    [SerializeField] private float maxFollowDistance;

    private SpriteRenderer sprRend;  

    private int selectedAttack;

  
    public int EnemyLevel => enemyLevel;

    
    public enum EnemyType
    {
        Mushroom,
        FlyingDemon,
        Skeleton
    }

    public EnemyType enemyType;






    private void Start()
    {
        enemyParentComponent = GetComponent<Enemy>();
        hurtBox = GetComponent<BoxCollider2D>();
        sprRend = GetComponent<SpriteRenderer>();
    }

    public void AIUpdate()
    {

        switch (enemyType)
        {
            case EnemyType.Mushroom:

                Vector2 direction = (Gamemanager.instance.enemiesController.playerRef.transform.position - transform.position).normalized;

                if (direction.x < 0) sprRend.flipX = true;
                else if (direction.x > 0) sprRend.flipX = false;

                transform.position += new Vector3(direction.x, direction.y,0) * speed * Time.deltaTime;
              
                if (Vector2.Distance (Gamemanager.instance.enemiesController.playerRef.transform.position,transform.position) <= distanceToAttack)
                {
                    enemyParentComponent.ChangeState(Enemy.EnemyStates.Attacking);
                }

                if (Vector2.Distance (Gamemanager.instance.enemiesController.playerRef.transform.position, transform.position) >= maxFollowDistance)
                {
                    enemyParentComponent.ChangeState(Enemy.EnemyStates.Idle);
                }

                break;
            case EnemyType.FlyingDemon:
                break;
            case EnemyType.Skeleton:
                break;
            default:
                break;
        }

        

    }

    public void Attack1()
    {
        selectedAttack = 1;
        hurtBox.enabled = true;
        
        switch (enemyType)
        {
            case EnemyType.Mushroom:

                
                break;
            case EnemyType.FlyingDemon:
                break;
            case EnemyType.Skeleton:
                break;
            default:
                break;
        }
    }


    public void Attack2()
    {
        selectedAttack = 2;
        hurtBox.enabled = false;
       

        switch (enemyType)
        {
            case EnemyType.Mushroom:

                float xo = transform.position.x -1;
                float yo = transform.position.y;

                float xd = 1;
                float yd = 1;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 position = new Vector2(xo + i * xd, yo + Mathf.Sin(i * (Mathf.PI / 2)));
                   Spores spore = Instantiate(spores, position, Quaternion.identity);
                    spore.InitialDirection = new Vector2(-1 * Mathf.Cos(i * (Mathf.PI) / 2), 1 * Mathf.Sin(i * (Mathf.PI) / 2));
                }

                break;
            case EnemyType.FlyingDemon:
                break;
            case EnemyType.Skeleton:
                break;
            default:
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && hurtBox.enabled == true)
        {
            if (selectedAttack == 1) player.lifeController.GetDamage(attack1Damage);
            else player.lifeController.GetDamage(attack2Damage);
        }
    }

    public void DeactivateHurtBox()
    {
        hurtBox.enabled = false;
    }


}
