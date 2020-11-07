using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PotionDispatcher
{

    private GameObject moveP, healP, freezeP;

    //[SerializeField] private GameObject movePotion, HealPotion, FreezePotion;


    public static PotionDispatcher instance;

    //Create singleton
    public static PotionDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PotionDispatcher();
            }
            return instance;
        }
       private set
        {
            instance = value;
        }
    }

    public enum PotionRequired
    {
        move,
        heal,
        freeze
    }


    public PotionDispatcher()
    {
        freezeP = Resources.Load("Prefabs/Items/Freeze Potion") as GameObject;
        healP = Resources.Load("Prefabs/Items/Heal Potion") as GameObject;
        moveP = Resources.Load("Prefabs/Items/Move Potion") as GameObject;
    }

    public GameObject GetPotion(PotionRequired potionToGiveBack)
    {
        switch (potionToGiveBack)
        {
            case PotionRequired.move:
                return moveP;

            case PotionRequired.heal:                
                return healP;

            case PotionRequired.freeze:               
                return freezeP;

            default:             
                return moveP;
        }
    }





}
