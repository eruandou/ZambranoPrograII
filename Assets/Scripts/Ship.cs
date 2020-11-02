using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    private int roadIndex;
    [SerializeField] private float speed;
    private Vector2 currentGoal;
    [SerializeField] private float rotationTime;
    private float starTime;
    

    private LevelSelectionManager lvlSelectionManager;


    void Start()
    {
        lvlSelectionManager = FindObjectOfType<LevelSelectionManager>();        
    }


    public void GetNewRoad(List <Vector2> roadConnection)
    {
        roadIndex = 1;
        starTime = Time.time;       
        StartCoroutine(WalkRoad(roadConnection));
    }


    private IEnumerator WalkRoad (List <Vector2> roadConnection)
    {
        currentGoal = roadConnection[roadIndex];
        Vector2 rotationNeeded = currentGoal - (Vector2)transform.position;
        transform.up = rotationNeeded;

        while ((Vector2)transform.position != currentGoal)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentGoal, speed * Time.deltaTime);
            transform.up = Vector2.Lerp(this.transform.up, rotationNeeded, (Time.time - starTime) / rotationTime);

            if ((Vector2)transform.position == currentGoal)
            {
                roadIndex++;             

                try
                {
                    currentGoal = roadConnection[roadIndex];
                    rotationNeeded = currentGoal - (Vector2)transform.position;
                    starTime = Time.time;
                }
                catch (Exception)
                {
                    lvlSelectionManager.ReachedDestination();                    
                }
            }
           

            yield return new WaitForEndOfFrame();
        }





    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
