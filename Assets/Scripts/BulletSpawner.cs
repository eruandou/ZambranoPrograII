using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //Bullet classes
    [SerializeField] private GameObject[] bullets;


    [SerializeField] private float maxSpawnTime, minSpawnTime;

    [SerializeField] private float maxY, maxX, minY, minX;

    private float nextSpawnTime;
    private Vector2 nextSpawnPosition;

    [SerializeField] private GameObject collectables;

    [SerializeField] private Vector2 offset;


    private int spawnedBullets;

    private void Start()
    {
       nextSpawnPosition = NewSpawnPosition();
       nextSpawnTime = NewSpawnTime();
        
        
    }



    private void Update()
    {
        if (nextSpawnTime <= 0)
        {
            
            if (spawnedBullets < 100)
            {
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(bullets[Random.Range(0, bullets.Length)], nextSpawnPosition + offset * i, Quaternion.identity, collectables.transform);
                    spawnedBullets++;
                }
            }

            nextSpawnTime = NewSpawnTime();
            nextSpawnPosition = NewSpawnPosition();
        }
        else
        {
            nextSpawnTime -= Time.deltaTime;
        }
    }



    private Vector2 NewSpawnPosition()
    {
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private float NewSpawnTime()
    {
        return Random.Range(minSpawnTime, maxSpawnTime);
    }

    public void BulletDestroyed()
    {
        spawnedBullets--;
    }

}
