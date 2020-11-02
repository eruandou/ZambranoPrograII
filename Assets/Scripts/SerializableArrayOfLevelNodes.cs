using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SerializableArrayOfLevelNodes 
{
    public bool[] unlockStates;

    public SerializableArrayOfLevelNodes()
    {
        unlockStates = new bool[11];
        for (int i = 1; i < unlockStates.Length; i++)
        {
            unlockStates[i] = false;
        }

        unlockStates[0] = true;
    }
    
       

}
