using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializableArrayOfLevelNodes 
{
    public bool[] unlockStates;
    public bool[] completedStates;

    public SerializableArrayOfLevelNodes()
    {
        unlockStates = new bool[11];
        completedStates = new bool[11];       
    }
    
       public void Reset()
    {
        for (int i = 1; i < unlockStates.Length; i++)
        {
            unlockStates[i] = false;
            completedStates[i] = false;
        }

        unlockStates[0] = true;
    }

}
