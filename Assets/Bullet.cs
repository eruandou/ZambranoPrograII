using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    [SerializeField] private float speed;
    [SerializeField] private int damage;
    public int bulletNumber;
    public Vector2 direction { get; set; }
    public int ExtraDamage { get; set; }






    private void Update()
    {
        transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Damage enemies
        Destroy(this.gameObject);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }




}
