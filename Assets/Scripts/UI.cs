using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private float offsetYBullets;

    [SerializeField] private GameObject bullet1, bullet2, bullet3, bullet4;

    private Dictionary<int, GameObject> bulletDictionary = new Dictionary<int, GameObject>();

    [SerializeField] private Vector3 position0Bullets;

    [SerializeField] private Vector3 position0Items;

    private List<Vector3> stackPosition;

    private PlayerShootingController playerShoot;

    List<GameObject> bulletsSpawned = new List<GameObject>();

    [SerializeField] private Player player;

    private DynArray itemsArray = new DynArray();

    public DynArray ItemsArray => itemsArray;

    private int currentSelectedItem = -1;   
 

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
            Vector3 position = new Vector3(position0Bullets.x, position0Bullets.y + i * offsetYBullets, position0Bullets.z);
            stackPosition.Add(position);                    
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
        if (itemsArray.HeldItems > itemsArray.ItemLimit)
        {
            return;
        }

        itemsArray.Add(newItem);
        Debug.Log("Got item");
        //if (itemsArray.HeldItems <=1)
        currentSelectedItem++;
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
   


    private void UpdateItemUI()
    {
        if (itemsArray.Retrieve(currentSelectedItem) != null)
        {
            Items itemOnCurrentSpot = itemsArray.Retrieve(currentSelectedItem);

            if (itemOnCurrentSpot.transform.parent != Camera.main.transform)
            {
               itemOnCurrentSpot.transform.SetParent(Camera.main.transform);
            }

            itemOnCurrentSpot.MoveToActive(position0Items);
           

        }

        //Set extra items as inactive

        ItemNode currentNode = itemsArray.root;
        while (currentNode != null)
        {
            if (currentNode.Index != currentSelectedItem) currentNode.storedItem.Deactivate();
            currentNode = currentNode.nextNode;
        }





    }


    public void ItemsToLeft()
    {

        if (currentSelectedItem == 0)
        {
            currentSelectedItem = itemsArray.HeldItems;
        }

        currentSelectedItem--;

        Debug.Log($"{currentSelectedItem} is the current selected item");

        UpdateItemUI();
    }

    public void ItemsToRight()
    {
        currentSelectedItem++;

        if (currentSelectedItem == itemsArray.HeldItems)
        {
            currentSelectedItem = 0;
        }

        


        Debug.Log($"{currentSelectedItem} is the current selected item");
        UpdateItemUI();
    }

    public void UseItem(Player player)
    {
        itemsArray.Retrieve(currentSelectedItem).ActivateItem(player);

        itemsArray.Remove(currentSelectedItem);

        if (currentSelectedItem == itemsArray.HeldItems)
        {
            currentSelectedItem = itemsArray.HeldItems - 1;
        }
        UpdateItemUI();



    }

}
