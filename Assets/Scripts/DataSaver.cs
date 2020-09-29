using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class DataSaver
{   
    private static Dictionary<int, HighscorePlayer> playerHighscoresDictionary = new Dictionary<int, HighscorePlayer>();

    private static BST BSTHighScores;

    public static void SavePlayers(int score, string nickname)
    {

        //Get previous data

        BinaryFormatter formatter = new BinaryFormatter();

        string dataPath = Application.persistentDataPath + "/players.dat";

       if (!File.Exists(dataPath))
        {
            FileStream stream = new FileStream(dataPath, FileMode.Create);
            formatter.Serialize(stream, playerHighscoresDictionary);
            stream.Close();
        }


        FileStream newStream = new FileStream(dataPath, FileMode.Open);
        
        playerHighscoresDictionary = formatter.Deserialize(newStream) as Dictionary<int, HighscorePlayer>;


        //Create and add new data

        HighscorePlayer newHighScorePlayer = new HighscorePlayer(score, nickname);

        playerHighscoresDictionary.Add(playerHighscoresDictionary.Count, newHighScorePlayer);

        //Write file

        formatter.Serialize(newStream, playerHighscoresDictionary);
        
        newStream.Close();
    }


    public static void LoadPlayers()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string dataPath = Application.persistentDataPath + "/players.dat";

        FileStream newStream = new FileStream(dataPath, FileMode.Open);

        BSTHighScores.InitializeTree();

        if (newStream != null)
        {
            playerHighscoresDictionary = formatter.Deserialize(newStream) as Dictionary<int, HighscorePlayer>;

            for (int i = 0; i < playerHighscoresDictionary.Count; i++)
            {
                HighscorePlayer playerToAdd = playerHighscoresDictionary[i];

                BSTHighScores.AddElement(ref BSTHighScores.root, playerToAdd);

                Debug.Log($"element added is {playerToAdd.Nickname} with a score of {playerToAdd.Score} and is at position {i}");
            }

        }
        else
        {
            Debug.Log("No highscore data found yet");
        }
        
    }





}
