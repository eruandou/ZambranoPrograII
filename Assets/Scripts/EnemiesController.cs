using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{


    public Player playerRef;
    public DynQueue enemiesToSpawnQueue;

    [SerializeField] private Enemy enemy1;
    [SerializeField] private Enemy enemy2;
    [SerializeField] private Enemy enemy3;

    private Dictionary <int,Enemy> enemiesDictionary = new Dictionary <int, Enemy>();

    [SerializeField] private ActivateableItems[] itemsToDrop;


    private int spawnedEnemies;
    private int spawnLimit;

    [SerializeField] private float retrieveTimeStart;
    private float retrieveTime;

    public bool CanRetrieve => retrieveTime <= 0 && !enemiesToSpawnQueue.IsQueueEmpty();



    private void Start()
    {
        Gamemanager.instance.LoadEnemiesController(this);
        enemiesToSpawnQueue = new DynQueue();

        enemiesDictionary.Add(1, enemy1);
        enemiesDictionary.Add(2, enemy2);
        enemiesDictionary.Add(3, enemy3);

        spawnLimit = Gamemanager.instance.MaxEnemies;    
        
        GenerateNewBatchOfEnemies();


        enemiesToSpawnQueue.OnDequeue += OnDequeueHandler;

        playerRef = FindObjectOfType<Player>();
    }



    public void FreezeEnemiesActivator()
    {
        StartCoroutine(FreezeEnemies());
    }

    private IEnumerator FreezeEnemies()
    {

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {           
            enemy.ChangeState(Enemy.EnemyStates.Frozen);
        }

        yield return new WaitForSeconds(4);

        foreach (Enemy enemy in enemies)
        {           
            enemy.ChangeState(Enemy.EnemyStates.Idle);
        }
     
    }


    public void GenerateNewBatchOfEnemies()
    {
        for (int i = 0; i < spawnLimit; i++)
        {           
            enemiesToSpawnQueue.Enqueue(enemiesDictionary[Random.Range(1, 4)]);
            spawnedEnemies++;            
        }
    }

    private void Update()
    {
        retrieveTime -= Time.deltaTime;
    }

    private void OnDequeueHandler()
    {
        retrieveTime = retrieveTimeStart;
    }


    public int SpawnedEnemies()
    {
        return spawnedEnemies;
    }

    public void OnPlayerDeadHandler()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.enabled = false;
            enemy.GetComponent<AIController>().enabled = false;
        }
    }

    public void ItemToDrop(Vector2 positionToSpawn, int spawnChance)
    {
        Debug.Log($"random value is {Random.value}");

        if (Random.value <= (float) spawnChance /100)
        {
            Instantiate(itemsToDrop[Random.Range(0, itemsToDrop.Length)], positionToSpawn, Quaternion.identity);
            Debug.Log("Enemy dropped an item");
        }
       
    }
}
