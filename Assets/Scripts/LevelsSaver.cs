using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LevelsSaver
{
    private static SerializableArrayOfLevelNodes levelNodesState;

    private const string UNLOCKED_LEVELS_DATA_PATH = "/unlockedLevels.json";

    public static void SaveNewUnlockedLevel(int nodeID, bool isComplete)
    {
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

        levelNodesState.unlockStates[nodeID - 1] = true;
        levelNodesState.completedStates[nodeID - 1] = isComplete;

        string modifiedJson = JsonUtility.ToJson(levelNodesState);

        File.WriteAllText(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH, modifiedJson);

    }

    
    public static (bool [], bool []) Load()
    {

        Debug.Log("loading");
        if (File.Exists(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH))
        {
            string json = File.ReadAllText(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH);

            levelNodesState = JsonUtility.FromJson<SerializableArrayOfLevelNodes>(json);

            Debug.Log("tried getting the list");
        }
        else
        {
            Debug.LogError("There's no nodes file created yet");
            ClearData();
        }

        return (levelNodesState.unlockStates,levelNodesState.completedStates);
      
    }


    public static void ClearData()
    {
        if (File.Exists(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH))
        {           
            File.Delete(Application.dataPath + UNLOCKED_LEVELS_DATA_PATH);           
        }

        SaveNewUnlockedLevel(1, false);

    }

 


}
