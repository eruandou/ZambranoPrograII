using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoxSurpirse : MonoBehaviour
{

    private EnemiesController enemiesCont;
    private Enemy fakeEnemyThis;
    private destructableTile dTile;

    // Start is called before the first frame update
    void Start()
    {
        enemiesCont = FindObjectOfType<EnemiesController>();
        fakeEnemyThis = GetComponent<Enemy>();
        enemiesCont.OnEnemySpawnHandler(fakeEnemyThis);
        dTile = GetComponent<destructableTile>();
        dTile.OnDestroyTile += () =>
        {
            enemiesCont.OnEnemyDieHandler(fakeEnemyThis);         
        };        
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

}
