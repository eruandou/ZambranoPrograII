using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    private EnemiesController enemyCont;

    private Enemy closestEnemy;

    private Vector2 pointDirection;

    [SerializeField] private GameObject radarUi;
    [SerializeField] private RectTransform radarPointer;




    private void Start()
    {
        enemyCont = FindObjectOfType<EnemiesController>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            radarUi.SetActive(true);
            closestEnemy = enemyCont.ReturnClosestEnemy();           
        }

        if (Input.GetKeyUp (KeyCode.Tab))
        {
            radarUi.SetActive(false);
        }

        if (closestEnemy != null)
        {
            float angle = Vector2.Angle(transform.position, closestEnemy.transform.position);

            radarPointer.localEulerAngles = new Vector3(0, 0, angle);
        
    }











}
