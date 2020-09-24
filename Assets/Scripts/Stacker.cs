using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface Istacker
{

    void Stack(Bullet newBullet);

    void Unstack();

    Bullet First();

    bool IsStackEmpty();

    void InitializeStack();

}

public class Stacker : Istacker 
{

    BulletNode top;

    private int stackLimit = 10;
    public int StackedItems { get; private set; }

    public event Action <int> OnStack; 
    public event Action <int> OnUnStack; 







    public void Stack (Bullet newBullet)
    {
        //Check maxed stacked items

        if (StackedItems < stackLimit)
        {
            
            BulletNode newNode = new BulletNode(newBullet);

            newNode.nextNode = top;

            top = newNode;

            StackedItems++;

            OnStack?.Invoke(top.storedBullet.bulletNumber);
        }
    }

    public void Unstack()
    {
        top = top.nextNode;
        StackedItems--;

        OnUnStack?.Invoke(StackedItems);
        //Update UI Elements
    }

    public void InitializeStack()
    {
        top = null;       
    }


    public Bullet First()
    {
        return top.storedBullet;
    }

    public BulletNode FirstNode()
    {
        return top;
    }


    public bool IsStackEmpty()
    {
        return (top == null);
    }

    





}
