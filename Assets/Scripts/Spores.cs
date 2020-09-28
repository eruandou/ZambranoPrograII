using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spores : MonoBehaviour
{

    [SerializeField] private float fallingSpeed;

    [SerializeField] private int damage;

    [SerializeField] private float timerToDie;

    [SerializeField] private float timeToFall;

    private Animator anim;

    public Vector2 InitialDirection { get; set; }





    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position += new Vector3(InitialDirection.x, InitialDirection.y, 0) * fallingSpeed * Time.deltaTime;

        if (timeToFall <= 0)
        {
            transform.position += new Vector3(0, -fallingSpeed * Time.deltaTime, 0);

            timerToDie -= Time.deltaTime;

            if (timerToDie <= 0)
            {
                Die();
            }
        }

        else
        {
            timeToFall -= Time.deltaTime;
        }

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.lifeController.GetDamage(damage);
        }


        Die();

    }

    private void Die()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        anim.SetTrigger("Die");

        yield return new WaitForSeconds(0.2f);

        Destroy(this.gameObject);
    }

}
