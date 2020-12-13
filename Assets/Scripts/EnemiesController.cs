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
    [SerializeField] private Enemy enemy4;
    [SerializeField] private Enemy enemy5;
    [SerializeField] private Enemy enemy6;

    public int chancesEnemy1;
    public int chancesEnemy2;
    public int chancesEnemy3;
    public int chancesEnemy4;
    public int chancesEnemy5;
    public int chancesEnemy6;



    private Dictionary <int,Enemy> enemiesDictionary = new Dictionary <int, Enemy>();

    public GameObject[] itemsToDrop;

    private List<Enemy> alreadySpawnedEnemies = new List<Enemy>();


    private int spawnedEnemies;
    private int spawnLimit;

    [SerializeField] private float retrieveTimeStart;
    private float retrieveTime;

    public bool CanRetrieve => retrieveTime <= 0 && !enemiesToSpawnQueue.IsQueueEmpty();

    public void SuscribeToEnemySpawner(EnemySpawners eSpawn)
    {
        eSpawn.OnEnemySpawn += OnEnemySpawnHandler;
    }

    private void Start()
    {
        Gamemanager.instance.LoadEnemiesController(this);
        enemiesToSpawnQueue = new DynQueue();

        enemiesDictionary.Add(1, enemy1);
        enemiesDictionary.Add(2, enemy2);
        enemiesDictionary.Add(3, enemy3);
        enemiesDictionary.Add(4, enemy4);
        enemiesDictionary.Add(5, enemy5);
        enemiesDictionary.Add(6, enemy6);

        spawnLimit = Gamemanager.instance.MaxEnemies;
        chancesEnemy1 = Gamemanager.instance.chanceToSpawnEnemy1;
        chancesEnemy2 = Gamemanager.instance.chanceToSpawnEnemy2;
        chancesEnemy3 = Gamemanager.instance.chanceToSpawnEnemy3;
        chancesEnemy4 = Gamemanager.instance.chanceToSpawnEnemy4;
        chancesEnemy5 = Gamemanager.instance.chanceToSpawnEnemy5;
        chancesEnemy6 = Gamemanager.instance.chanceToSpawnEnemy6;


        itemsToDrop = new GameObject[3];
        itemsToDrop[0] = PotionDispatcher.Instance.GetPotion(PotionDispatcher.PotionRequired.heal);
        itemsToDrop[1] = PotionDispatcher.Instance.GetPotion(PotionDispatcher.PotionRequired.freeze);
        itemsToDrop[2] = PotionDispatcher.Instance.GetPotion(PotionDispatcher.PotionRequired.move);

        
        GenerateNewBatchOfEnemies();


        enemiesToSpawnQueue.OnDequeue += OnDequeueHandler;

        playerRef = FindObjectOfType<Player>();
    }

    private void OnEnemySpawnHandler(Enemy enemy)
    {
        alreadySpawnedEnemies.Add(enemy);
        enemy.OnDie += OnEnemyDieHandler;
    }

    private void OnEnemyDieHandler (Enemy enemy)
    {
        alreadySpawnedEnemies.Remove(enemy);
    }

    public void FreezeEnemiesActivator()
    {
        StartCoroutine(FreezeEnemies());
    }

    private IEnumerator FreezeEnemies()
    {
        foreach (Enemy enemy in alreadySpawnedEnemies)
        {           
            enemy.ChangeState(Enemy.EnemyStates.Frozen);
        }

        yield return new WaitForSeconds(4);

        foreach (Enemy enemy in alreadySpawnedEnemies)
        {           
            enemy.ChangeState(Enemy.EnemyStates.Idle);
        }
     
    }

    public Enemy ReturnClosestEnemy()
    {
        Enemy closestEnemy = alreadySpawnedEnemies[0];

        foreach (Enemy enemy in alreadySpawnedEnemies)
        {
            if  (Vector2.Distance (enemy.transform.position, playerRef.transform.position) < Vector2.Distance(closestEnemy.transform.position, playerRef.transform.position))
            {
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }



    public void GenerateNewBatchOfEnemies()
    {
        int maxChances = chancesEnemy1 + chancesEnemy2 + chancesEnemy3 + chancesEnemy4 + chancesEnemy5 + chancesEnemy6;
        int chance;
        int enemyToSpawn;
        int realChance2 = chancesEnemy1 + chancesEnemy2;
        int realChance3 = realChance2 + chancesEnemy3;
        int realChance4 = realChance3 + chancesEnemy4;
        int realChance5 = realChance4 + chancesEnemy5;
        int realChance6 = realChance5 + chancesEnemy6;

        for (int i = 0; i < spawnLimit; i++)
        {
           chance = UnityEngine.Random.Range(0, maxChances);



            //Ask how to automate this or make it more generic

            //refactor

            if (chance < chancesEnemy1) enemyToSpawn = 1;
            else if (chance >= chancesEnemy1 && chance < realChance2) enemyToSpawn = 2;
            else if (chance >= realChance2 && chance < realChance3) enemyToSpawn = 3;
            else if (chance >= realChance3 && chance < realChance4) enemyToSpawn = 4;
            else if (chance >= realChance4 && chance < realChance5) enemyToSpawn = 5;
            else enemyToSpawn = 6;

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
