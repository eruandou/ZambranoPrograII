using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PotionDispatcher: MonoBehaviour
{

    private GameObject moveP, healP, freezeP;

    private static PotionDispatcher instance;
      

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

    public GameObject GetPotion(PotionRequired potionType)
    {
        switch (potionType)
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
