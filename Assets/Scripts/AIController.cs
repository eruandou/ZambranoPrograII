using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    [SerializeField] private float speed;
    private Enemy enemyParentComponent;
        
    [SerializeField] private int attack1Damage;
    [SerializeField] private int attack2Damage;

    [SerializeField] private GameObject Projectile;

    [SerializeField] private int enemyLevel;

    [SerializeField] private BoxCollider2D hurtBox;

    [SerializeField] private float distanceToAttack;
    [SerializeField] private float maxFollowDistance;

    private EnemiesController enemiesCont;

    private SpriteRenderer sprRend;
    public bool detectPlayerBoxEnabled;

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
        sprRend = GetComponent<SpriteRenderer>();
        enemiesCont = FindObjectOfType<EnemiesController>();
    }

    public void AIUpdate()
    {
        Vector2 direction = (enemiesCont.playerRef.transform.position - transform.position).normalized;

        if (direction.x < 0) sprRend.flipX = true;
        else if (direction.x > 0) sprRend.flipX = false;

        transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;

        if (Vector2.Distance(enemiesCont.playerRef.transform.position, transform.position) <= distanceToAttack)
        {
            enemyParentComponent.ChangeState(Enemy.EnemyStates.Attacking);
        }

        if (Vector2.Distance(enemiesCont.playerRef.transform.position, transform.position) >= maxFollowDistance)
        {
            enemyParentComponent.ChangeState(Enemy.EnemyStates.Idle);
        }

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

    public void Attack1()
    {
        selectedAttack = 1;
        hurtBox.enabled = true;
    }


    public void Attack2()
    {
        selectedAttack = 2;
        
       

        switch (enemyType)
        {
            case EnemyType.Mushroom:

                hurtBox.enabled = false;

                float xo = transform.position.x -1;
                float yo = transform.position.y;

                float xd = 1;

                for (int i = 0; i < 3; i++)
                {
                    Vector2 position = new Vector2(xo + i * xd, yo + Mathf.Sin(i * (Mathf.PI / 2)));
                    GameObject spore = Instantiate(Projectile, position, Quaternion.identity);
                    spore.GetComponent <Spores>().InitialDirection = new Vector2(-1 * Mathf.Cos(i * (Mathf.PI) / 2), 1 * Mathf.Sin(i * (Mathf.PI) / 2));
                }

                break;
            case EnemyType.FlyingDemon:

                hurtBox.enabled = true;
                StartCoroutine(SpawnBulletsWithDelays(0.6f, 3));


                break;
            case EnemyType.Skeleton:

                hurtBox.enabled = true;


                break;
            default:
                break;
        }
    }

    private IEnumerator SpawnBulletsWithDelays(float delayBetweenBullets, int bulletAmount) 
    {
        bool setChaser = EnemyLevel >= 2;

        for (int i = 0; i < bulletAmount; i++)
        {

            GameObject newBullet = Instantiate(Projectile, transform.position, Quaternion.identity);
            EnemyBullet bulletRef = newBullet.GetComponent<EnemyBullet>();
            bulletRef.SetChaser(setChaser);
            bulletRef.ChangeDirection((enemiesCont.playerRef.transform.position - transform.position).normalized);          
            
            yield return new WaitForSeconds(delayBetweenBullets);
            Debug.Log("Deployed bullet " + i);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null && hurtBox.enabled == true && !detectPlayerBoxEnabled)
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
