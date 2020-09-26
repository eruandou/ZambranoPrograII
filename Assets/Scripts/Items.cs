﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{

    public enum PotionType
    {
        Heal,
        Freeze,
        Speed
    }

    public PotionType potionType;

    private int healingPoints = 2,
                speedBoost = 5;


    private float timeEnemiesAreFrozen = 8,
                  timeToNormalSpeed = 5;


    public void ActivateItem(Player player)
    {
        switch (potionType)
        {
            case PotionType.Heal:


                player.lifeController.GetHeal(healingPoints);                
                break;


            case PotionType.Freeze:

                break;
            case PotionType.Speed:

                player.ChangeSpeedItem(speedBoost, timeToNormalSpeed);

                break;
            default:
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.GetItem(this);
            //Destroy(this.gameObject);
        }
    }


    public void MoveToActive(Vector3 newPosition)
    {
        this.transform.position = Camera.main.transform.position + newPosition;
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }










}
