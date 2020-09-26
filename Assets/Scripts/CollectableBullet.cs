using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBullet : MonoBehaviour
{

    [SerializeField] private Bullet bulletToCollect;







    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerShootingController playerShootController = collision.GetComponent<PlayerShootingController>();

        if (playerShootController != null && playerShootController.ActiveFiring == false)
        {
            playerShootController.bulletsStack.Stack(bulletToCollect);
            Destroy(this.gameObject);
        }



    }











}
