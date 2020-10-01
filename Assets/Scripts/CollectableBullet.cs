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
            //Throw this in an event
            playerShootController.bulletsStack.Stack(bulletToCollect);
            playerShootController.CollectBulletSound();
            FindObjectOfType<BulletSpawner>().BulletDestroyed();
            Destroy(this.gameObject);
        }
    }
}
