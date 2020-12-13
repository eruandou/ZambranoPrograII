using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    private EnemiesController enemyCont;

    private Enemy closestEnemy;

    private Vector2 pointDirection;

    private bool triggeredMap;

    [SerializeField] private GameObject radarUi;
    [SerializeField] private RectTransform radarPointer;
    [SerializeField] private float borderForPointer;


    private void Start()
    {
        enemyCont = FindObjectOfType<EnemiesController>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            triggeredMap = !triggeredMap;
           
            if (triggeredMap)
            {
                closestEnemy = enemyCont.ReturnClosestEnemy();
               if (closestEnemy == null)
                {
                    triggeredMap = !triggeredMap;
                    return;
                }
            }
            radarUi.SetActive(triggeredMap);
        }

        if (closestEnemy != null && radarUi.activeSelf)
        {
            SetArrowRotation(closestEnemy);
        }

        
    }

    private void SetArrowRotation(Enemy enemy)
    {
        pointDirection = (enemy.transform.position - transform.position).normalized;

        float angle = (Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg) % 360;
        radarPointer.localEulerAngles = new Vector3(0, 0, angle);
    }

    /*
    private Vector2 CapPointerPosition (Vector2 pointerPos)
    {
        if (pointerPos.x < borderForPointer) pointerPos.x = borderForPointer;
        if (pointerPos.x > Screen.width - borderForPointer) pointerPos.x = Screen.width - borderForPointer;

        if (pointerPos.y < borderForPointer) pointerPos.y = borderForPointer;
        if (pointerPos.y > Screen.height - borderForPointer) pointerPos.y = Screen.height - borderForPointer;

        return pointerPos;
    }*/
}
