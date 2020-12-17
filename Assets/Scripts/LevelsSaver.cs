using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class LevelsSaver
{

    private static (bool[], bool[]) levelStates; 

    private static Database levelsDatabase;
    private static string databaseName = "LevelStateTable";



    public static void SaveNewUnlockedLevel(int nodeID, bool isComplete)
    {

        if (levelsDatabase == null) LoadDataBase();      
        levelsDatabase.UpdateLevelStateValue(nodeID, 1, Convert.ToInt32(isComplete));

    }

    private static void LoadDataBase()
    {        
        levelsDatabase = new Database(databaseName);
        levelsDatabase.CreateLevelStateTable();
    }

    public static (bool [], bool []) Load()
    {
        if (levelsDatabase == null) LoadDataBase();
        levelStates = levelsDatabase.GetLevelsState();
        return levelStates;
    }


    public static bool GetLevelCompletitionState(int levelID)
    {
        if (levelID <= 0) return false;
        Load();
        return levelStates.Item2[levelID - 1];
    }

    public static void ClearData()
    {    
        levelsDatabase.ResetLevelStatesTable();
        Gamemanager.instance.secretItemsDatabase.ResetCollectedSecretItems();
    }

 


}
