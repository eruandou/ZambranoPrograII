using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISet
{
    void InitializeSet();
    bool IsSetEmpty();
    void Add(int x);
    int Pick();
    void Remove(int x);
    bool Belongs(int x);
}

public class Set: ISet
{

    int[] vertex;
    int position;


    public Set()
    {
        InitializeSet();
    }




    public void InitializeSet()
    {
        vertex = new int[100];
        position = 0;
    }
    public bool IsSetEmpty()
    {
        return position == 0;
    }

    public void Add(int newVertex)
    {
        if (!this.Belongs(newVertex))
        {
            vertex[position] = newVertex;
            position++;
        }
    }


    public int Pick()
    {
        return vertex[position - 1];
    }

   

    public void Remove(int vertexToRemove)
    {
        int i = 0;
        while (i < position && vertex[i] != vertexToRemove)
        {
            i++;
        }
        if (i < position)
        {
            vertex[i] = vertex[position - 1];
            position--;
        }
    }
    public bool Belongs(int x)
    {
        int i = 0;
        while (i < position && vertex[i] != x)
        {
            i++;
        }
        return (i < position);
    }

















}


