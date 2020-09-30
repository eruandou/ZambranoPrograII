using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class DataSaver
{
    private static SerializableListOfHighScores highscoreList;

    public static BST BSTHighscores = new BST();

    private const string SAVE_DATA_PATH = "/MyHighScores.json";

    
    public static void Save (int score, string nickname)
    {
        if (File.Exists (Application.dataPath + SAVE_DATA_PATH))
        {
            string json = File.ReadAllText(Application.dataPath + SAVE_DATA_PATH);

            highscoreList = JsonUtility.FromJson<SerializableListOfHighScores>(json);

            Debug.Log("There was a file");
        }
        else
        {
            highscoreList = new SerializableListOfHighScores();
            Debug.Log("There was NOT a file");
        }

        Debug.Log($"stored name is gonna be {nickname}");
        

        HighscorePlayer newHSPlayer = new HighscorePlayer(score, nickname);

        Debug.Log($"New player is {newHSPlayer.nickname} and has a score of {newHSPlayer.score}");
        highscoreList.myHighScoresList.Add(newHSPlayer);

        string modifiedJson = JsonUtility.ToJson(highscoreList);

        File.WriteAllText(Application.dataPath + SAVE_DATA_PATH, modifiedJson);

    }


    public static void Load()
    {

        Debug.Log("loading");
        if (File.Exists(Application.dataPath + SAVE_DATA_PATH))
        {
            string json = File.ReadAllText(Application.dataPath + SAVE_DATA_PATH);

            highscoreList = JsonUtility.FromJson<SerializableListOfHighScores>(json);

            Debug.Log("tried getting the list");
        }
        else
        {
            Debug.LogError("There's no highscore file created yet");            
        }


       
        BSTHighscores.InitializeTree();

        if (highscoreList != null)
        {
            foreach (HighscorePlayer HSP in highscoreList.myHighScoresList)
            {
                BSTHighscores.AddElement(ref BSTHighscores.root, HSP);
            }

        }
    }


    public static void ClearData()
    {
        if (File.Exists (Application.dataPath + SAVE_DATA_PATH))
        {
            highscoreList.myHighScoresList.Clear();
            File.Delete(Application.dataPath + SAVE_DATA_PATH);
        }
    }
}
