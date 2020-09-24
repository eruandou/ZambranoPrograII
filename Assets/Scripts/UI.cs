using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private float offsetY;

    [SerializeField] private GameObject bullet1, bullet2, bullet3, bullet4;

    private Dictionary<int, GameObject> bulletDictionary = new Dictionary<int, GameObject>();

    [SerializeField] private Vector3 position0;

    private List<Vector3> stackPosition;

    [SerializeField] private PlayerShootingController playerShoot;

    List<GameObject> bulletsSpawned = new List<GameObject>();
 

    private void Start()
    {


        bulletDictionary.Add(1, bullet1);
        bulletDictionary.Add(2, bullet2);
        bulletDictionary.Add(3, bullet3);
        bulletDictionary.Add(4, bullet4);
        

        stackPosition = new List<Vector3>();

        for (int i = 0; i < 10; i++)
        {
            Vector3 position = new Vector3(position0.x, position0.y + i * offsetY, position0.z);
            stackPosition.Add(position);
            Debug.Log($"Stack positions are {stackPosition[i]}");            
        }

        playerShoot.bulletsStack.OnStack += OnStackHandler;
        playerShoot.bulletsStack.OnUnStack += OnUnstackhandler;

    }




  


    private void OnStackHandler(int newBulletType)
    {
        UpdateUI(false, newBulletType);
    }

    private void OnUnstackhandler (int positionclear)
    {
        UpdateUI(true,0);
    }

    public void UpdateUI(bool isErase, int newBulletType)
    {
       if (!isErase)
        {
            GameObject bulletToAdd = bulletDictionary[newBulletType];
            GameObject instancedBullet = Instantiate(bulletToAdd, stackPosition[playerShoot.bulletsStack.StackedItems - 1], Quaternion.identity);
            instancedBullet.transform.SetParent(Camera.main.transform,false);
            bulletsSpawned.Add(instancedBullet);
          

        }

        else
        {
            Destroy(bulletsSpawned[playerShoot.bulletsStack.StackedItems]);
            Debug.Log("Erasing");
        }







    }
   











}
