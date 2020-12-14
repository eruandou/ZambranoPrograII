using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Vector2 direction;
    private bool followPlayer;
    private Player playerRef;

    [SerializeField] private int damage;
    [SerializeField] private CircleCollider2D collider;

    [SerializeField] private float speed;
    [SerializeField] private float lifeTimeStart;
    private float lifeTime;
    [SerializeField] private float timeToDestroy;
    private bool taggedToBeDestroyed;

    private Animator anim;




    private void Start()
    {
        anim = GetComponent<Animator>();       
        if (followPlayer)
        {
            playerRef = Gamemanager.instance.enemiesController.playerRef;
        }

   
    }



    private void Update()
    {
        if (followPlayer)
        {
            direction = (playerRef.transform.position - transform.position).normalized;
        }

        MoveBullet(direction);

        if (!taggedToBeDestroyed)
        {
            lifeTime += Time.deltaTime;
            if (lifeTime >= lifeTimeStart)
            {
                DestroyBullet();
            }
        }
       

      else
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ChangeDirection (Vector2 newDirection)
    {
        direction = newDirection;
    }
    private void MoveBullet(Vector2 dir)
    {
        transform.position += new Vector3(dir.x, dir.y, 0) * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.lifeController.GetDamage(damage);
            DestroyBullet();
        }
    }

    public void SetChaser (bool follow)
    {
        followPlayer = follow;
    }

    private void DestroyBullet()
    {
        anim.SetTrigger("Hit");
        collider.enabled = false;
        lifeTime = timeToDestroy;
        taggedToBeDestroyed = true;

    }


}
