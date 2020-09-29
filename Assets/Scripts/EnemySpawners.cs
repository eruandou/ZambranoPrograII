using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawners : MonoBehaviour
{    
    private SpawnPoints[] spawnPoints;
    private EnemiesController enemiesController;

    [SerializeField] private float internalSpawnTimeMax;
    [SerializeField] private float internalSpawnTimeMin;

    private float internalSpawnTime;


    private void Start()
    {
        GetNewSpawnTime();

        //Get enemies controller reference
        enemiesController = FindObjectOfType<EnemiesController>();
        //Get reference of all spawn points in the scene
        spawnPoints = FindObjectsOfType <SpawnPoints>();
    }


    private void Spawn()
    {
        //Instantiate dequeued item, in random spawnpoint
        SpawnPoints nextSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(enemiesController.enemiesToSpawnQueue.First(true), nextSpawnPoint.transform.position, Quaternion.identity);

        nextSpawnPoint.ActivatePortal();
        
    }



    private void Update()
    {

        internalSpawnTime -= Time.deltaTime;

        if (internalSpawnTime <= 0 && enemiesController.CanRetrieve)
        {
            Spawn();
            GetNewSpawnTime();
            
        }
        
    }


    private void GetNewSpawnTime()
    {
        internalSpawnTime = Random.Range(internalSpawnTimeMin, internalSpawnTimeMax);
    }





}
