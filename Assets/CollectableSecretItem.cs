using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSecretItem : MonoBehaviour
{
     private int itemNumber;

   


    public void GetItemNumber (int number)
    {
        this.itemNumber = number;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {


        var player = collision.GetComponent<Player>();

        if (player != null)
        {
          
            Gamemanager.instance.GetSecretItem(itemNumber);
            Destroy(gameObject); 
        }
    }
}
