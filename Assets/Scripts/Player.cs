using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    [SerializeField] private float ShootCooldownStart;
    private float shootCooldown;

    private PlayerShootingController playerShootingController;

    private Vector2 lastDirection;

    public event Action<ActivateableItems> OnGetItem;   

    private void Start()
    {
        anim = GetComponent<Animator>();
        lifeController = new LifeController(maxLife);
        lifeController.OnGetDamage += OnGetDamageHandler;
        lifeController.OnGetHeal += OnGetHealHandler;
        lifeController.OnDead += OnDeadHandler;
        sprRend = GetComponent<SpriteRenderer>();
        playerShootingController = GetComponent<PlayerShootingController>();
    }



    private void Update()
    {
        CheckMovement();
        CheckShoot();
        CheckItemUsage();
        lifeController.Update();

    }


   public void GetItem(ActivateableItems newItem)
    {
        OnGetItem?.Invoke(newItem);
    }



    private void CheckShoot()
    {
        if (Input.GetKey(KeyCode.J) && shootCooldown >= ShootCooldownStart)
        {
            
            playerShootingController.Shoot(lastDirection.normalized);
            shootCooldown = 0;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            playerShootingController.CombineBullets();
        }
        shootCooldown += Time.deltaTime;





    }


    private void CheckItemUsage()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && Gamemanager.instance.UI.IHaveItems)
        {
            Gamemanager.instance.UI.ItemsToLeft();
            
        }

        if (Input.GetKeyDown(KeyCode.RightShift) && Gamemanager.instance.UI.IHaveItems)
        {
            Gamemanager.instance.UI.ItemsToRight();
           
        }

        if (Input.GetKeyDown(KeyCode.R) && Gamemanager.instance.UI.IHaveItems)
        {
            Gamemanager.instance.UI.AutomaticReorganizeInventory();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Gamemanager.instance.UI.UseItem(this);
        }



    }

    private void CheckMovement()
    {
        float moveX = Input.GetAxisRaw ("Horizontal");

        float moveXAbs = Mathf.Abs(moveX);

        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetAxisRaw("Horizontal") < 0) transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        else if (Input.GetAxisRaw("Horizontal") > 0) transform.rotation = Quaternion.LookRotation(Vector3.forward);

        transform.position += transform.right * speed * Time.deltaTime * moveXAbs + Vector3.up * moveY * speed * Time.deltaTime;

        if (moveXAbs != 0 || moveY != 0)
        {
            anim.SetBool("Walking", true);
            lastDirection = new Vector2(moveX, moveY);
        }
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

        Gamemanager.instance.enemiesController.OnPlayerDeadHandler();
        Gamemanager.instance.LoadGameOver();      
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

    public void ChangeSpeedItem(float newSpeed, float duration)
    {
        StartCoroutine(ChangeSpeed(newSpeed, duration));
    }

    private IEnumerator ChangeSpeed (float extraSpeedValue, float duration)
    {
        
        speed += extraSpeedValue;

        yield return new WaitForSeconds(3f);

        speed -= extraSpeedValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("I collisioned");
    }

}
