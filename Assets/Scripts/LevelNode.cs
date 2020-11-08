using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode : MonoBehaviour
{

    private bool selectable;
    private bool unlocked;

    private Animator anim;
    private SpriteRenderer sprRend;
    private BoxCollider2D boxColl; 

    private LevelSelectionManager lvlSelectManager;
    private GameObject connectionToPreviousNode;
    

    public int id;  

 

    private void Awake()
    {

        sprRend = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();

        try
        {
            connectionToPreviousNode = GetComponentInChildren<ParticleSystem>().gameObject;
            connectionToPreviousNode.SetActive(false);
        }
        catch (System.Exception)
        {
          
        }
    }



    public void ChangeSelectableness(bool isSelectable)
    {
        selectable = isSelectable;
    }

    public void UnlockLevel(bool isUnlocked)
    {
        unlocked = isUnlocked;
        if (unlocked)
        {
            anim = GetComponent<Animator>();
            lvlSelectManager = FindObjectOfType<LevelSelectionManager>();
            ChangeSelectableness(true);
            if (connectionToPreviousNode != null) connectionToPreviousNode.SetActive(true);
             
            
        }
        else
        {
            sprRend.enabled = false;
            boxColl.enabled = false;
        }
    }

    public bool IsUnlocked()
    {
        return unlocked;
    }

    private void OnMouseDown()
    {       
        if (selectable)
        {
            ChangeSelectableness(false);
            lvlSelectManager.SelectDestination(this);
        }
        else
        {
           if (lvlSelectManager.CanSelectNewNode() && this == lvlSelectManager.WhatIsDestinyNode())
            {
               Gamemanager.instance.LoadLevel (id);                
            }
        }
    }

    
    public void ChangeCompleteness(bool isComplete)
    {
       if (!unlocked) return;

       if (isComplete)
        {
            anim.SetBool("Complete", true);
        }
        else
        {
            anim.SetBool("Complete", false);

        }                
    }

}
