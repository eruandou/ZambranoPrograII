using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSecretItem : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {


        var player = collision.GetComponent<Player>();

        if (player != null)
        {
            Gamemanager.instance.GetSecretItem();
            Destroy(gameObject); 
        }
    }
}
