using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{

    [SerializeField] private GameObject bulletEnemy1;
    [SerializeField] private GameObject bulletEnemy2;
    [SerializeField] private GameObject bulletEnemy3;
    [SerializeField] private GameObject bulletPlayer1;
    [SerializeField] private GameObject bulletPlayer2;
    [SerializeField] private GameObject bulletPlayer3;
    [SerializeField] private GameObject bulletPlayer4;







    public void Shoot(bool isEnemy, int bulletNumber, Vector2 startPosition)
    {
        GameObject bulletToSpawn;

        if (isEnemy)
        {
            switch (bulletNumber)
            {
                case 1:
                    bulletToSpawn = bulletEnemy1;
                    break;
                case 2:
                    bulletToSpawn = bulletEnemy2;
                    break;
                case 3:
                    bulletToSpawn = bulletEnemy3;
                    break;


                default:
                    bulletToSpawn = bulletEnemy1;
                    break;                  
            }

        }

        else
        {
            switch (bulletNumber)
            {

                case 1:
                    bulletToSpawn = bulletPlayer1;
                    break;
                case 2:
                    bulletToSpawn = bulletPlayer2;
                    break;
                case 3:
                    bulletToSpawn = bulletPlayer3;
                    break;
                case 4:
                    bulletToSpawn = bulletPlayer4;
                    break;
                default:
                    bulletToSpawn = bulletPlayer1;
                    break;
            }
        }

        GameObject bullet = Instantiate(bulletToSpawn, startPosition, Quaternion.identity);


    }









}
