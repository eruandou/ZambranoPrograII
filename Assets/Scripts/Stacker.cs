using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [SerializeField] private int StackLimit;
    private int stackedItems;




    public void Stack (Bullet newBullet)
    {
        //Check maxed stacked items

        if (stackedItems < StackLimit)
        {

            BulletNode newNode = new BulletNode(newBullet);

            newNode.nextNode = top;

            top = newNode;
        }
    }

    public void Unstack()
    {
        top = top.nextNode;
    }

    public void InitializeStack()
    {
        top = null;
    }


    public Bullet First()
    {
        return top.storedBullet;
    }

    public bool IsStackEmpty()
    {
        return (top == null);
    }








}
