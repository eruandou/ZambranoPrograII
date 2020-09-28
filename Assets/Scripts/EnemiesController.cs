using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{


    public Player playerRef;



    private void Start()
    {
        playerRef = FindObjectOfType<Player>();
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
            enemy.enabled = false;
            enemy.GetComponent<Animator>().enabled = false;
            enemy.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        yield return new WaitForSeconds(4);

        Debug.Log("Defrosting enemies");
        foreach (Enemy enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            enemy.enabled = true;
            enemy.GetComponent<Animator>().enabled = true;
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
        }
        Debug.Log("Coroutine ended succesfully");
    }






}
