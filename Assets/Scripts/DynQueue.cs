using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IDynQueue
{
    void Enqueue(Enemy enemy);

    void Dequeue();

    Enemy First();

    EnemyNode FirstNode();
    void InitializeQueue();

    bool IsQueueEmpty();
}


public class DynQueue 
{
    EnemyNode first;

    EnemyNode last;


    public DynQueue()
    {
        InitializeQueue();
    }





    public void Enqueue (Enemy newEnemy)
    {

        EnemyNode newNode = new EnemyNode(newEnemy);

        //If last isn't null, add the new node as next node
        if (last != null)
        {
           
            last.NextNode = newNode;
        }

      
        last = newNode;

        
        if (first == null)
        {
         
            first = last;
        }
    }


    public void Dequeue()
    {
       //Change first node for next node
        first = first.NextNode;

        //If there's no next node, the queue is empty
        if (first == null)
        {
            last = null;
        }
    }


    public bool IsQueueEmpty()
    {
        return (last == null);
    }

    public Enemy First()
    {
        return first.EnemyData;
    }

    public EnemyNode FirstNode()
    {
        return first;
    }

    public void InitializeQueue()
    {
        first = null;
        last = null;
    }


}
