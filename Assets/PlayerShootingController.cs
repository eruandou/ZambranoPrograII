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

    private bool activeFiring;

    private void Awake()
    {
        bulletsStack = new Stacker();
        bulletsStack.InitializeStack();
    }

    public void Shoot(Vector2 direction)
    {
        if (activeBullet != null)
        {
           Bullet bullet = Instantiate(activeBullet, bulletSpawner.position,Quaternion.identity);
            bullet.direction = direction;
            bullet.ExtraDamage += extraDamage;


        }
    }

    public void CombineBullets()
    {
        if (!activeFiring && bulletsStack.StackedItems >= 3)
        {
            Bullet bullet1;
            Bullet bullet2;
            Bullet bullet3;

            //Automation THIS
            bullet1 = bulletsStack.First();
            bullet2 = bulletsStack.FirstNode().nextNode.storedBullet;
            bullet3 = bulletsStack.FirstNode().nextNode.nextNode.storedBullet;

            CombineBullets(bullet1, bullet2, bullet3);

            activeFiring = true;
            StartCoroutine(BulletChangeOverTime());
        }
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
        activeBullet = null;
    }



    private void CombineBullets(Bullet bullet1, Bullet bullet2, Bullet bullet3)
    {
        int bullet1Value = bullet1.bulletNumber;
        int bullet2Value = bullet2.bulletNumber;
        int bullet3Value = bullet3.bulletNumber;

        int value1Counter = 0;
        int value2Counter = 0;
        int value3Counter = 0;
        int value4Counter = 0;

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


        if (value1Counter == 4)
        {
            PlusDamageBullet(1);
        }

        if (value2Counter == 4)
        {
            KillEverythingOnScreen();
        }

        if (value3Counter == 4)
        {
            PlusDamageBullet(2);
        }

        if (value4Counter == 4)
        {
            EightDirectionBullet();
        }



        if (value1Counter == 2)
        {
            if (value2Counter == 1)
            {
                GiveMovePotion(1);
            }

            if (value3Counter == 1)
            {
                TwiBullets();
            }

            if (value4Counter == 1)
            {
                GiveFreezingPotion(1);
            }
        }

        if (value2Counter == 2)
        {
            
            if (value1Counter == 1)
            {
                FreezeEnemies();
            }

            if (value3Counter == 1)
            {
                GiveMovePotion(2);
            }

            if (value4Counter == 1)
            {
                GiveFreezingPotion(2);
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
                GiveHealPotion(2);
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
                Drunk();
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
                Filter1();
            }

            if (value2Counter ==1 && value4Counter == 1)
            {
                Filter2();
            }

            if (value3Counter ==1 && value4Counter == 1)
            {
                Filter3();
            }
        }

        if (value2Counter == 1)
        {
           if (value3Counter == 1 && value4Counter == 1)
            {
                Filter4();
            }
        }




    }


    private void EightDirectionBullet()
    {
        Debug.Log("Shooting eigth directional bullets");
    }

    private void PlusDamageBullet(int extraDamage)
    {
        Debug.Log($"I added {extraDamage} damage to bullets");
    }


    private void KillEverythingOnScreen()
    {
        Debug.Log("Kill everything on screen");
    }

    private void GiveMovePotion(int amount)
    {
        Debug.Log($"Give {amount} move potions");
    }

    private void TwiBullets()
    {
        Debug.Log("Shooting TwiBullets");
    }

    private void GiveFreezingPotion(int amount)
    {
        Debug.Log($"Give {amount} freezing potions");
    }

    private void FreezeEnemies()
    {
        Debug.Log("Freeze enemies");
    }

    private void TriBullets()
    {
        Debug.Log("Shooting Tribullets");
    }

    private void Invulnerability()
    {
        Debug.Log("I'm invulnerable");
    }


    private void Filter1()
    {
        Debug.Log("Filter 1");
    }

    private void Filter2()
    {
        Debug.Log("Filter 2");
    }

    private void Filter3()
    {
        Debug.Log("Filter 3");
    }

    private void Filter4()
    {
        Debug.Log("Filter 4");
    }

    private void GiveHealPotion (int amount)
    {
        Debug.Log($"Give {amount} healing potions");
    }

    private void Drunk()
    {
        Debug.Log("Drunk effect");
    }



}
