using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNode 
{

    public ActivateableItems storedItem;

    public ItemNode nextNode;

    public int Index { get;  set; }



    public ItemNode (ActivateableItems newItem)
    {
        storedItem = newItem;        
    }






}
