using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNode 
{

    public Items storedItem;

    public ItemNode nextNode;

    public int Index { get;  set; }



    public ItemNode (Items newItem)
    {
        storedItem = newItem;        
    }






}
