using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructableTile : MonoBehaviour
{

    [SerializeField] private GameObject colletableSecretItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();

        if (bullet != null)
        {
            Instantiate(colletableSecretItem, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }






}
