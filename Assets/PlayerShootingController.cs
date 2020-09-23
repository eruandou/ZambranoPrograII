using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
   
    private Bullet activeBullet;

    public Stacker bulletsStack;

    [SerializeField] private float timeBetweenBulletChange;

    private bool activeFiring;

    private void Start()
    {
        bulletsStack = new Stacker();
        bulletsStack.InitializeStack();
    }

    public void Shoot( Vector2 startPosition)
    {
        Instantiate(activeBullet, startPosition, Quaternion.identity);
    }

    public void CombineBullets()
    {
        if (!activeFiring)
        {
            //Combination code
            activeFiring = true;
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
    }


}
