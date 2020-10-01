using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDynArray
{
    void Add(ActivateableItems item);

    void Remove(int index);

    ActivateableItems Retrieve(int index);

    ItemNode RetrieveNode(int index);
    ItemNode PreviousNode(int index);

    void Initialize();

    bool IsArrayEmpty();


}




public class DynArray : IDynArray
{

    public ItemNode root;
    public int HeldItems { get; private set; }
    public int ItemLimit { get; private set; } = 10;

    private ItemNode emptyNode;


    public DynArray()
    {
        Initialize();
        emptyNode = new ItemNode(null);
    }

    public void Add (ActivateableItems newItem)
    {
        
        
        ItemNode newNode = new ItemNode(newItem);


        if (root == null)
        {
            root = newNode;
            root.Index = 0;
          
        }

        else
        {
            ItemNode currentNode = root;           
            while (currentNode.nextNode != null)
            {
                currentNode = currentNode.nextNode;        
            }

            currentNode.nextNode = newNode;
            newNode.Index = currentNode.Index + 1;

        }


        HeldItems++;
    }




    public ActivateableItems Retrieve(int index)
    {
      
        if (IsArrayEmpty())
        {
            return emptyNode.storedItem;
        }


        ItemNode currentNode = root;


        while (currentNode.Index != index)
        {
            currentNode = currentNode.nextNode;
        }

        return currentNode.storedItem;

        

    }

    public ItemNode RetrieveNode(int index)
    {
        ItemNode currentNode = root;

        while (currentNode.Index != index)
        {
            currentNode = currentNode.nextNode;
        }

        return currentNode;
    }



    public ItemNode PreviousNode(int index)
    {
        ItemNode currentNode = root;

        if (index == 0)
        {
            while (currentNode.Index != HeldItems)
            {
                currentNode = currentNode.nextNode;
            }
            
        }

        else
        {
            while (currentNode.Index != index - 1)
            {
                currentNode = currentNode.nextNode;
            }

        }

        return currentNode;

    }




    public void Remove(int index)
    {

        if (HeldItems >=1)
        {
            if (index != 0)
            {
                ItemNode currentNode = root;
                /*
                while (currentNode.Index < index - 1)
                {
                    currentNode = currentNode.nextNode;
                }

                currentNode.nextNode = currentNode.nextNode.nextNode;

                while (currentNode != null)
                {
                    currentNode = currentNode.nextNode;
                    currentNode.Index -= 1;
                }
                */


                while (currentNode.Index < index)
                {
                    currentNode = currentNode.nextNode;
                }

                PreviousNode(index).nextNode = currentNode.nextNode;
                ActivateableItems itemToDelete = currentNode.storedItem;
                itemToDelete.DestroyItem();
                currentNode = currentNode.nextNode;

                

                while (currentNode != null)
                {
                    currentNode.Index--;
                    currentNode = currentNode.nextNode;
                }



            }

            else
            {
                root.storedItem.DestroyItem();
                root = root.nextNode;
                ItemNode currentNode = root;

                while (currentNode != null)
                {
                    currentNode.Index -= 1;
                    currentNode = currentNode.nextNode;
                }
            }


            HeldItems--;
        }
            
        

       
    }

    public bool IsArrayEmpty()
    {
        return (root == null);
    }

    public void Initialize()
    {
        root = null;
        HeldItems = 0;
    }
    








}
