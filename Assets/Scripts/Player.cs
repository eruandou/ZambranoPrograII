using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;


    private Animator anim;
    public LifeController lifeController;
    [SerializeField] private int maxLife = 10;
    private SpriteRenderer sprRend;

    [SerializeField] private Color damagedColor;
    [SerializeField] private Color healedColor;
    [SerializeField] private Color normalColor;

    private int equippedBullet;
    




    private void Start()
    {
        anim = GetComponent<Animator>();
        lifeController = new LifeController(maxLife);
        lifeController.OnGetDamage += OnGetDamageHandler;
        lifeController.OnGetHeal += OnGetHealHandler;
        sprRend = GetComponent<SpriteRenderer>();
    }



    private void Update()
    {
        CheckMovement();
        CheckShoot();
    
    }






    private void CheckShoot()
    {







    }




    private void CheckMovement()
    {
        float moveX = Mathf.Abs(Input.GetAxis("Horizontal")) * speed * Time.deltaTime ;
        float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") < 0) transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        else if (Input.GetAxisRaw("Horizontal") > 0) transform.rotation = Quaternion.LookRotation(Vector3.forward);

        transform.position += transform.right * speed * Time.deltaTime * moveX + Vector3.up * moveY * speed * Time.deltaTime;

        if (moveX != 0 || moveY != 0) anim.SetBool("Walking", true);
        else anim.SetBool("Walking", false);
    }

    private void OnGetDamageHandler(int currentLife, int damage)
    {
        StartCoroutine(ChangeColor(damagedColor, 10));
    }

    private void OnGetHealHandler (int currentLife, int heal)
    {
        StartCoroutine(ChangeColor(healedColor, 10));
    }

    private void OnDeadHandler()
    {
        lifeController.OnDead -= OnDeadHandler;
        lifeController.OnGetHeal -= OnGetHealHandler;
        lifeController.OnGetDamage -= OnGetDamageHandler;

        Destroy(this);
    }




    private IEnumerator ChangeColor (Color newColor, int iterations, int i = 0)
    {
        sprRend.color = newColor;

        yield return new WaitForSeconds(0.05f);

        sprRend.color = normalColor;

        if (i < iterations)
        {
            i++;
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(ChangeColor(newColor, iterations, i));
        }
            
        
    }

}
