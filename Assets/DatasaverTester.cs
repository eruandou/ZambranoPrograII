using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatasaverTester : MonoBehaviour
{

    public string[] names;
  
    private void Awake()
    {
        /*
           for (int i = 0; i < 10; i++)
           {
               int points = Random.Range(0, 10000);
              string name = names[Random.Range(0, names.Length-1)];

               DataSaver.Save(points, name);
               Debug.Log("saving random");
           }
        */

        //DataSaver.Save(0, "juan");
        Debug.Log (DataSaver.BSTHighscores.nodeCount);
    }









}
