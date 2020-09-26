using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private float offsetYBullets;

    [SerializeField] private GameObject bullet1, bullet2, bullet3, bullet4;

    private Dictionary<int, GameObject> bulletDictionary = new Dictionary<int, GameObject>();

    [SerializeField] private Vector3 position0;

    private List<Vector3> stackPosition;

    private PlayerShootingController playerShoot;

    List<GameObject> bulletsSpawned = new List<GameObject>();

    [SerializeField] private Player player;

    private DynArray itemsArray = new DynArray();

    private int currentSelectedItem = 0;

    [SerializeField] private Transform item1, item2, item3;

 

    private void Start()
    {


        bulletDictionary.Add(1, bullet1);
        bulletDictionary.Add(2, bullet2);
        bulletDictionary.Add(3, bullet3);
        bulletDictionary.Add(4, bullet4);

        stackPosition = new List<Vector3>();

        playerShoot = player.GetComponent<PlayerShootingController>();



        for (int i = 0; i < 10; i++)
        {
            Vector3 position = new Vector3(position0.x, position0.y + i * offsetYBullets, position0.z);
            stackPosition.Add(position);
            Debug.Log($"Stack positions are {stackPosition[i]}");            
        }

        playerShoot.bulletsStack.OnStack += OnStackHandler;
        playerShoot.bulletsStack.OnUnStack += OnUnstackhandler;
        player.OnGetItem += OnGetItemHandler;

    }




  


    private void OnStackHandler(int newBulletType)
    {
        UpdateBulletUI(false, newBulletType);
    }

    private void OnUnstackhandler (int positionclear)
    {
        UpdateBulletUI(true,0);
    }

    private void OnGetItemHandler(Items newItem)
    {
        itemsArray.Add(newItem);
        UpdateItemUI();
    }


    public void ClearList()
    {
        bulletsSpawned.Clear();
    }
    


    public void UpdateBulletUI(bool isErase, int newBulletType = 0)
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
   


    public void UpdateItemUI()
    {
       
    }




    private void Update()
    {
        ItemsInput();   
    }



    private void ItemsInput()
    {


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ItemsToLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            ItemsToRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseItem();
        }
    }

    private void ItemsToLeft()
    {
        currentSelectedItem--;
        if (currentSelectedItem < 0)
        {
            currentSelectedItem = itemsArray.HeldItems;
        }
    }

    private void ItemsToRight()
    {
        currentSelectedItem++;
        if (currentSelectedItem > itemsArray.HeldItems)
        {
            currentSelectedItem = 0;
        }
    }

    private void UseItem()
    {
        //Item effect
        Debug.Log("Item in effect");
        //Remove item from list
        itemsArray.Remove(currentSelectedItem);

        if (currentSelectedItem > itemsArray.HeldItems)
        {
            currentSelectedItem = itemsArray.HeldItems;
        }


        
        Debug.Log("Using item");
    }

}
