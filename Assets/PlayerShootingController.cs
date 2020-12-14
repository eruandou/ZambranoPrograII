using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{

    private Bullet activeBullet;

    public Stacker bulletsStack;

    [SerializeField] private Transform bulletSpawner;

    [SerializeField] private float timeBetweenBulletChange;

    private int extraDamage;
    private bool eightDirectionShooting;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private AudioSource audioSrc;

    [SerializeField] private AudioClip shootSound, combineSound, bulletCollect;

    private int bulletsToShoot = 1;

    private int value1Counter, value2Counter, value3Counter, value4Counter;
    
    public bool ActiveFiring { get; private set; }

    private ScreenEffectManager screenEffectManager;

    private void Awake()
    {
        bulletsStack = new Stacker();
        bulletsStack.InitializeStack();
        screenEffectManager = FindObjectOfType<ScreenEffectManager>();
    }

    public void Shoot(Vector2 direction)
    {
        if (activeBullet != null)
        {
            ResetAudioSource();

            Vector2 directionModifier;
            
            if (eightDirectionShooting)
            {
;
                for (int i = 0; i < 8; i++)
                {
                    Bullet bullet = Instantiate(activeBullet, bulletSpawner.position, Quaternion.identity);
                    float x = Mathf.Cos(i * (Mathf.PI / 4));
                    float y = Mathf.Sin(i * (Mathf.PI / 4));
                    bullet.direction = new Vector2(x, y);
                }
            }

            else
            {
                for (int i = 0; i < bulletsToShoot; i++)
                {
                    Bullet bullet = Instantiate(activeBullet, bulletSpawner.position, Quaternion.identity);
                    directionModifier = new Vector2(Mathf.Cos(i * (Mathf.PI / 4)), Mathf.Sin(i * (Mathf.PI / 4)));
                    bullet.direction = (direction + i * directionModifier).normalized;
                    bullet.ExtraDamage += extraDamage;
                }
            }
                 
           
            audioSrc.clip = shootSound;
            audioSrc.Play();


        }
    }

    public void CombineBullets()
    {
        if (!ActiveFiring && bulletsStack.StackedItems >= 3)
        {
            Bullet bullet1;
            Bullet bullet2;
            Bullet bullet3;

            audioSrc.clip = combineSound;
            ResetAudioSource();
            audioSrc.Play();

            //Automation THIS
            bullet1 = bulletsStack.First();
            bullet2 = bulletsStack.FirstNode().nextNode.storedBullet;
            bullet3 = bulletsStack.FirstNode().nextNode.nextNode.storedBullet;

            CombineBullets(bullet1, bullet2, bullet3);

            ActiveFiring = true;
            StartCoroutine(BulletChangeOverTime());
        }
    }

    public void CollectBulletSound()
    {
        audioSrc.clip = bulletCollect;
        audioSrc.volume = 0.5f;
        audioSrc.pitch = 0.8f;
        audioSrc.Play();
    }
    private IEnumerator BulletChangeOverTime()
    {
        //Preguntar alternativa para un For

        while (!bulletsStack.IsStackEmpty())
        {
            activeBullet = bulletsStack.First();
            yield return new WaitForSeconds(timeBetweenBulletChange);
            bulletsStack.Unstack();          
        }

        ResetFiring();
        

    }


    private void ResetFiring()
    {
        activeBullet = null;
        ActiveFiring = false;
        bulletsToShoot = 1;
        eightDirectionShooting = false;
        extraDamage = 0;
        Gamemanager.instance.UI.ClearList();
    }



    
    private void CombineBullets(Bullet bullet1, Bullet bullet2, Bullet bullet3)
    {
        int bullet1Value = bullet1.bulletNumber;
        int bullet2Value = bullet2.bulletNumber;
        int bullet3Value = bullet3.bulletNumber;

         value1Counter = 0;
         value2Counter = 0;
         value3Counter = 0;
         value4Counter = 0;

        //JSON read this

        switch (bullet1Value)
        {
            case 1:
                value1Counter++;
                break;
            case 2:
                value2Counter++;
                break;
            case 3:
                value3Counter++;
                break;
            case 4:
                value4Counter++;
                break;
            default:
                break;
        }

        switch (bullet2Value)
        {
            case 1:
                value1Counter++;
                break;
            case 2:
                value2Counter++;
                break;
            case 3:
                value3Counter++;
                break;
            case 4:
                value4Counter++;
                break;
            default:
                break;
        }

        switch (bullet3Value)
        {
            case 1:
                value1Counter++;
                break;
            case 2:
                value2Counter++;
                break;
            case 3:
                value3Counter++;
                break;
            case 4:
                value4Counter++;
                break;
            default:
                break;
        }


        if (value1Counter == 3)
        {
           PlusDamageBullet(1);
        }

        if (value2Counter == 3)
        {
            KillEverythingOnScreen();
        }

        if (value3Counter == 3)
        {
            PlusDamageBullet(2);
        }

        if (value4Counter == 3)
        {
            EightDirectionBullet();
        }



        if (value1Counter == 2)
        {
            if (value2Counter == 1)
            {
                GivePotion(PotionDispatcher.PotionRequired.move,1);
            }

            if (value3Counter == 1)
            {
                TwiBullets();
            }

            if (value4Counter == 1)
            {              
                GivePotion(PotionDispatcher.PotionRequired.freeze, 1);
            }
        }

        if (value2Counter == 2)
        {
            
            if (value1Counter == 1)
            {
                //Make it different from freeze potion
                FreezeEnemies();
            }

            if (value3Counter == 1)
            {
                GivePotion(PotionDispatcher.PotionRequired.move, 2);
            }

            if (value4Counter == 1)
            {
                GivePotion(PotionDispatcher.PotionRequired.freeze, 2);
            }


        }

        if (value3Counter == 2)
        {
            if (value1Counter == 1)
            {
                TriBullets();
            }

            if (value2Counter == 1)
            {
                GivePotion(PotionDispatcher.PotionRequired.heal, 2);
            }

            if (value4Counter == 1)
            {
                TwiBullets();
            }
        }

        if (value4Counter == 2)
        {

            if (value1Counter == 1)
            {
                Invulnerability();
            }

            if (value2Counter == 1)
            {
                GivePotion(PotionDispatcher.PotionRequired.heal, 1);
            }

            if (value3Counter == 1)
            {
                TriBullets();
            }

        }


        if (value1Counter == 1)
        {
            if (value2Counter ==1 && value3Counter == 1)
            {
                GameBoyFilter();
            }

            if (value2Counter ==1 && value4Counter == 1)
            {
                AstigmatismFilter();
            }

            if (value3Counter ==1 && value4Counter == 1)
            {
                LSDTripFilter();
            }
        }

        if (value2Counter == 1)
        {
           if (value3Counter == 1 && value4Counter == 1)
            {
                Drunk();
            }
        }
    }

    private void EightDirectionBullet()
    {
        eightDirectionShooting = true;        
    }



    private void PlusDamageBullet(int exDamage)
    {
        this.extraDamage = exDamage;       

        Debug.Log("Extra damage");
    }

    private void KillEverythingOnScreen()
    {
        Collider2D[] enemies = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(20, 20), 0, enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().LifeController.GetDamage(100);
        }

        Debug.Log("Kill in screen");
    }

    private void GivePotion(PotionDispatcher.PotionRequired potion, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (PotionDispatcher.Instance != null)
            {
                Instantiate(PotionDispatcher.Instance.GetPotion(potion),this.transform.position, Quaternion.identity);
            }
        }

    }

    private void TwiBullets()
    {
        bulletsToShoot = 2;
        Debug.Log("TwiBullets");
    }

   

    private void FreezeEnemies()
    {
       FindObjectOfType<EnemiesController>().FreezeEnemiesActivator();
        Debug.Log("Freeze enemies");
    }

    private void TriBullets()
    {
        bulletsToShoot = 3;
        Debug.Log("tribullet");
    }

    private IEnumerator Invulnerability()
    {
        Debug.Log("invulnerable");
        //Works, Give visual feedback

        LifeController lc = GetComponentInParent<Player>().lifeController;
        lc.ChangeInvincible(true);

        yield return new WaitForSeconds(5);

        lc.ChangeInvincible(false);

    }


    private void GameBoyFilter()
    {
        screenEffectManager.EnableFilter(0);
    }

    private void AstigmatismFilter()
    {
        screenEffectManager.EnableFilter(1);
    }

    private void LSDTripFilter()
    {
        screenEffectManager.EnableFilter(2);
    }   


    private void Drunk()
    {
        screenEffectManager.EnableFilter(3);
    }


    private void ResetAudioSource()
    {
        audioSrc.volume = 1;
        audioSrc.pitch = 1;
    }



}
