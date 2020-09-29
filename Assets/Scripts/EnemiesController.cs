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


    private int spawnedEnemies;
    [SerializeField] private int spawnLimit;

    [SerializeField] private float retrieveTimeStart;
    private float retrieveTime;

    public bool CanRetrieve => retrieveTime <= 0 && !enemiesToSpawnQueue.IsQueueEmpty();



    private void Start()
    {
        playerRef = FindObjectOfType<Player>();
        enemiesToSpawnQueue = new DynQueue();

        enemiesDictionary.Add(1, enemy1);
        enemiesDictionary.Add(2, enemy2);
        enemiesDictionary.Add(3, enemy3);

        GenerateNewBatchOfEnemies();

        enemiesToSpawnQueue.OnDequeue += OnDequeueHandler;


    }



    public void FreezeEnemiesActivator()
    {
        StartCoroutine(FreezeEnemies());
    }

    private IEnumerator FreezeEnemies()
    {

        Debug.Log("Freezing enemies");
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            enemy.ChangeState(Enemy.EnemyStates.Frozen);
            enemy.GetComponent<Animator>().enabled = false;
            enemy.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        yield return new WaitForSeconds(4);

        Debug.Log("Defrosting enemies");
        foreach (Enemy enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            enemy.ChangeState(Enemy.EnemyStates.Idle);
            enemy.GetComponent<Animator>().enabled = true;
            enemy.GetComponent<SpriteRenderer>().color = enemy.neutralColor;
        }
        Debug.Log("Coroutine ended succesfully");
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
}
