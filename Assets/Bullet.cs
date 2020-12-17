using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] bool piercerBullet;

    [SerializeField] private float lifetime;
   
    public int bulletNumber;
    public Vector2 direction { get; set; }
    public int ExtraDamage { get; set; }






    private void Update()
    {
        transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) Destroy(gameObject);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            try
            {
                enemy.LifeController.GetDamage(damage);
            }
            catch (System.Exception)
            {
                throw;
            }
                  
        }

       if (!piercerBullet) Destroy(this.gameObject);

    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }




}
