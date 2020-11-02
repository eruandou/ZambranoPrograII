using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode : MonoBehaviour
{

    private bool selectable;
    private bool unlocked;

    private bool complete;

    private Animator anim;
    private SpriteRenderer sprRend;
    private BoxCollider2D boxColl;

    private LevelSelectionManager lvlSelectManager;

    public int id;

    public enum CompletitionState
    {
        Complete,
        NotComplete
    }

    public CompletitionState CurrentState { get; private set; }

    private void Awake()
    {

        sprRend = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();
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
    }


    public void ChangeCompleteness(CompletitionState newState)
    {
        CurrentState = newState;

        switch (CurrentState)
        {
            case CompletitionState.Complete:
                anim.SetBool("Complete", true);
                break;
            case CompletitionState.NotComplete:
                anim.SetBool("Complete", false);
                break;
            default:
                break;
        }

    }












}
