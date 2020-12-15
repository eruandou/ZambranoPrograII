using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LevelsSaver
{
    private static SerializableArrayOfLevelNodes levelNodesState;

    private static (bool[], bool[]) levelStates;

    private const string UNLOCKED_LEVELS_DATA_PATH = "/unlockedLevels.json";

    private static Database levelsDatabase;
    private static string databaseName = "LevelStateTable";



    public static void SaveNewUnlockedLevel(int nodeID, bool isComplete)
    {/*
        if (File.Exists(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH))
        {
            string json = File.ReadAllText(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH);

            levelNodesState = JsonUtility.FromJson<SerializableArrayOfLevelNodes>(json);

            Debug.Log("There was a file");
        }
        else
        {
            levelNodesState = new SerializableArrayOfLevelNodes();
            
            Debug.Log("There was NOT a file");
        }
        */
        if (levelsDatabase == null) LoadDataBase();

        levelsDatabase.UpdateLevelStateValue(nodeID, true, isComplete);


        /*levelNodesState.unlockStates[nodeID - 1] = true;
        levelNodesState.completedStates[nodeID - 1] = isComplete;
        */
        /*
        string modifiedJson = JsonUtility.ToJson(levelNodesState);

        File.WriteAllText(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH, modifiedJson);
        */
    }

    private static void LoadDataBase()
    {        
        levelsDatabase = new Database(databaseName);
        levelsDatabase.CreateLevelStateTable();
    }

    public static (bool [], bool []) Load()
    {

        /*

        if (File.Exists(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH))
        {
            string json = File.ReadAllText(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH);

            levelNodesState = JsonUtility.FromJson<SerializableArrayOfLevelNodes>(json);
        }
        else
        {
            Debug.LogError("There's no nodes file created yet");
            ClearData();
        }
        */
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
        /*
        if (File.Exists(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH))
        {           
            File.Delete(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH);           
        }

        SaveNewUnlockedLevel(1, false);
        */

        levelsDatabase.ResetLevelStatesTable();
    }

 


}
