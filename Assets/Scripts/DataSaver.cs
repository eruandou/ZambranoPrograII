using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public static class DataSaver
{

    public static BST BSTHighscores = new BST();

    private static Database scoresDatabase;
    private static string databaseName = "PlayerScoresDataTable";



    public static void Save (int score, string nickname)
    {
       
        Debug.Log($"stored name is gonna be {nickname}");
        

        HighscorePlayer newHSPlayer = new HighscorePlayer(score, nickname);

        Debug.Log($"New player is {newHSPlayer.nickname} and has a score of {newHSPlayer.score}");
        
        if (scoresDatabase  == null)
        {
            LoadDataBase();
        }
          
        scoresDatabase.InsertPlayerIntoRanking(newHSPlayer);
    }
    
  
    private static void LoadDataBase()
    {
        scoresDatabase = new Database(databaseName);
        scoresDatabase.CreateScoresDataTable();
    }

    public static void Load()
    {

       
        BSTHighscores.InitializeTree();
        
        if (scoresDatabase == null) LoadDataBase();
      
        List<HighscorePlayer> highscoreList = scoresDatabase.GetPlayersRankingList();

        if (highscoreList != null)
        {
            foreach (HighscorePlayer HSP in highscoreList)
            {
                BSTHighscores.AddElement(ref BSTHighscores.root, HSP);
            }

        }
    }


    public static void ClearData()
    {     
        if (scoresDatabase == null) LoadDataBase();
    }
}
