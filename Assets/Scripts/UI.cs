using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    [SerializeField] private TextMeshProUGUI pointsText;
 

    private void Start()
    {
        Gamemanager.instance.LoadUI(this);

        bulletDictionary.Add(1, bullet1);
        bulletDictionary.Add(2, bullet2);
        bulletDictionary.Add(3, bullet3);
        bulletDictionary.Add(4, bullet4);

        stackPosition = new List<Vector3>();
        player = FindObjectOfType<Player>();
        playerShoot = player.GetComponent<PlayerShootingController>();

        UpdatePoints();



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

    private void OnGetItemHandler(ActivateableItems newItem)
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

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.F1)) DynArrayToArray();
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
            ActivateableItems itemOnCurrentSpot = itemsArray.Retrieve(currentSelectedItem);

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

    public void UpdatePoints()
    {
        pointsText.text = $"Points : {Gamemanager.instance.ActualPoints}";
    }


    //Quicksort related

    private ItemNode[] DynArrayToArray()
    {
        ItemNode currentNode = itemsArray.root;
        ItemNode[] itemarray = new ItemNode[itemsArray.HeldItems];

        for (int i = 0; i < itemsArray.HeldItems; i++)
        {
            itemarray[i] = currentNode;
            currentNode = currentNode.nextNode;
        }


        Debug.Log("Disorganized list");

        for (int i = 0; i < itemsArray.HeldItems; i++)
        {
            Debug.Log($"{itemarray[i].storedItem.potionType}");
        }

        quickSort(itemarray, 0, 2);


        for (int i = 0; i < itemsArray.HeldItems; i++)
        {
            Debug.Log($"{itemarray[i].storedItem.potionType}");
        }

        return itemarray;
    }


    public int Partition(ItemNode[] arr, int left, int right)
    {
        int pivot;
        int aux = (left + right) / 2;
        pivot = arr[aux].storedItem.potionTypeValue;



        while (true)
        {
            while (arr[left].storedItem.potionTypeValue < pivot)
            {
                left++;
            }
            while (arr[right].storedItem.potionTypeValue > pivot)
            {
                right--;
            }
            if (left < right)
            {
                int temp = arr[right].storedItem.potionTypeValue;
                arr[right] = arr[left];
                arr[left].storedItem.potionTypeValue = temp;
            }
            else
            {
                // este es el valor que devuelvo como proxima posicion de
                // la particion en el siguiente paso del algoritmo
                return right;
            }
        }

    }
    public void quickSort(ItemNode[] arr, int left, int right)
    {
        int pivot;
        if (left < right)
        {
            pivot = Partition(arr, left, right);
            if (pivot > 1)
            {
                // mitad del lado izquierdo del vector
                quickSort(arr, left, pivot - 1);
            }
            if (pivot + 1 < right)
            {
                // mitad del lado derecho del vector
                quickSort(arr, pivot + 1, right);
            }
        }
    }

}

