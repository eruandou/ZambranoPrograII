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

    public int chancesEnemy1;
    public int chancesEnemy2;
    public int chancesEnemy3;

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
        chancesEnemy1 = Gamemanager.instance.chanceToSpawnEnemy1;
        chancesEnemy2 = Gamemanager.instance.chanceToSpawnEnemy2;
        chancesEnemy3 = Gamemanager.instance.chanceToSpawnEnemy3;
        
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
        int maxChances = chancesEnemy1 + chancesEnemy2 + chancesEnemy3;
        int chance;
        int enemyToSpawn;
        int realChance2 = chancesEnemy1 + chancesEnemy2;
        int realChance3 = realChance2 + chancesEnemy3;

        for (int i = 0; i < spawnLimit; i++)
        {
           chance = Random.Range(0, maxChances);



            //Ask how to automate this or make it more generic

            if (chance < chancesEnemy1) enemyToSpawn = 1;
            else if (chance >= chancesEnemy1 && chance < realChance2) enemyToSpawn = 2;
            else  enemyToSpawn = 3;

            Debug.Log($"gonna spawn enemy {enemyToSpawn}");

            enemiesToSpawnQueue.Enqueue(enemiesDictionary[enemyToSpawn]);
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


        if (Random.value <= (float) spawnChance /100)
        {
            Instantiate(itemsToDrop[Random.Range(0, itemsToDrop.Length)], positionToSpawn, Quaternion.identity);           
        }
       
    }
}
