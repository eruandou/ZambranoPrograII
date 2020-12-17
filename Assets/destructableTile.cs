using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructableTile : MonoBehaviour
{

    [SerializeField] private GameObject colletableSecretItem;
    [SerializeField] private int collectableSecretItemNumber;

    public event Action OnDestroyTile;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();

        if (bullet != null)
        {
            OnDestroyTile?.Invoke();
            var c = Instantiate(colletableSecretItem, this.transform.position, Quaternion.identity);
            c.GetComponent<CollectableSecretItem>().GetItemNumber(collectableSecretItemNumber);
            Destroy(gameObject);
        }
    }






}
