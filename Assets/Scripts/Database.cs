using System.Data;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Database
{
    private string connectionPath;
    private IDbConnection dbConn;
    public Database(string databaseName)
    {
        connectionPath = "URI=file:" + Application.dataPath + $"/{databaseName}.s3db";
        MakeConnection();
    }
    private void MakeConnection()
    {
        dbConn = new SqliteConnection(connectionPath);
    }

    private void PostQuery(string query)
    {
        try
        {
            dbConn.Open();

            IDbCommand command = dbConn.CreateCommand();
            command.CommandText = query;
            command.ExecuteReader();

            command.Dispose();
            command = null;

        }
        catch (Exception e)
        {
            Debug.Log($"Error has ocurred : {e}");
        }
        finally
        {
            dbConn.Close();
        }
    }

    public void UpdateLevelStateValue(int level, bool unlocked, bool completed)
    {
        string query = string.Format("UPDATE LevelStateTable SET Unlocked = {0}, Completed = {1} WHERE id = {2}", unlocked, completed, level);
        PostQuery(query);
    }

    //Tested, works
    public void ResetScoresDataTable()
    {
        string query = "DROP TABLE IF EXISTS PlayerScoresDataTable";
        PostQuery(query);
        CreateScoresDataTable();
    }

    public void ResetLevelStatesTable()
    {
        string query = "DROP TABLE IF EXISTS LevelStateTable";
        PostQuery(query);
        CreateLevelStateTable();

        for (int i = 1; i < 12; i++)
        {
            InsertLevelState(i, false, false);
        }

        UpdateLevelStateValue(1, true, false);
    }

    public (bool[], bool []) GetLevelsState()
    {
        bool[] unlockedState = new bool[11];
        bool[] completedState = new bool[11];
        

        try
        {
            dbConn.Open();

            IDbCommand dbCommand = dbConn.CreateCommand();

            string consultQuery = "SELECT * FROM LevelStateTable";

            dbCommand.CommandText = consultQuery;

            IDataReader reader = dbCommand.ExecuteReader();

            int index = 0;
            while (reader.Read())
            {
                unlockedState[index] = reader.GetBoolean(1);
                completedState[index] = reader.GetBoolean(2);
                index++;
            }

                reader.Close();
            reader = null;
            dbCommand.Dispose();
            dbCommand = null;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            dbConn.Close();
        }

        return (unlockedState,completedState);


    }

    //Tested, works
    public void InsertPlayerIntoRanking(HighscorePlayer hsP)
    {
       string query = string.Format ("INSERT INTO PlayerScoresDataTable" +
            "(Score,Nickname)" + 
            "VALUES ({0},'{1}')",
            hsP.score,
            hsP.nickname);

        PostQuery(query);     
    }

    public void InsertLevelState (int levelId, bool unlocked, bool completed)
    {
        string query = string.Format("INSERT INTO LevelStateTable" +
            "(Id,Unlocked,Completed)" +
            "VALUES ({0},{1}, {2})",
            levelId,
            unlocked,
            completed);

        PostQuery(query);
            

    }

    //Create tables (level data, scores data)

    public void CreateScoresDataTable()
    {
        string query =
            "CREATE TABLE IF NOT EXISTS PlayerScoresDataTable ( " +
                "Id INTEGER PRIMARY KEY NOT NULL, " +
                "Score INTEGER NOT NULL, " +
                "Nickname VARCHAR(10) NOT NULL)";
        PostQuery(query);

    }

    public void CreateLevelStateTable()
    {
        string query =
            "CREATE TABLE IF NOT EXISTS LevelStateTable ( " +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Unlocked BOOL NOT NULL, " +
                "Completed BOOL NOT NULL)";

        PostQuery(query);



        
    }


    public void CreateLevelDataTable()
    {
        /*
       string query = "CREATE TABLE IF NOT EXISTS LevelDataTable ( " +
            "LevelId INTEGER PRIMARY KEY  AUTOINCREMENT, " +
            "MaxEnemies INTEGER NOT NULL, " +
            "TimeLimit INTEGER NOT NULL, " +
            "LevelsItUnlocks INTEGER NOT NULL, " +
            "Mushroom1Chance INTEGER NOT NULL, " +
            "Mushroom2Chance INTEGER NOT NULL, " +
            "Mushroom3Chance INTEGER NOT NULL, " +
            "Skeleton1Chance INTEGER NOT NULL, " +
            "Skeleton2Chance INTEGER NOT NULL, " +
            "Skeleton3Chance INTEGER NOT NULL, " +
            "FlyingDemon1Chance INTEGER NOT NULL, " +
            "FlyingDemon2Chance INTEGER NOT NULL, " +
            "FlyingDemon3Chance INTEGER NOT NULL, ";
        */

        string query =
           "CREATE TABLE IF NOT EXISTS LevelDataTable ( " +
               "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               "MaxEnemies INTEGER NOT NULL, " +
               "TimeLimit INTEGER NOT NULL, " +
               "LevelsItUnlocks1 INTEGER NOT NULL, " +
               "LevelsItUnlocks2 INTEGER NOT NULL, " +
               "Mushroom1Chance INTEGER NOT NULL, " +
               "Mushroom2Chance INTEGER NOT NULL, " +
               "Mushroom3Chance INTEGER NOT NULL, " +
               "Skeleton1Chance INTEGER NOT NULL, " +
               "Skeleton2Chance INTEGER NOT NULL, " +
               "Skeleton3Chance INTEGER NOT NULL, " +
               "FlyingDemon1Chance INTEGER NOT NULL, " +
               "FlyingDemon2Chance INTEGER NOT NULL, " +
               "FlyingDemon3Chance INTEGER NOT NULL)";             
        PostQuery(query);
    }





    public List<HighscorePlayer> GetPlayersRankingList()
    {
        List <HighscorePlayer> listOfPlayers = new List<HighscorePlayer>();

        try
        {
            dbConn.Open();

            IDbCommand dbCommand = dbConn.CreateCommand();

            string consultQuery = "SELECT * FROM PlayerScoresDataTable";

            dbCommand.CommandText = consultQuery;

            IDataReader reader = dbCommand.ExecuteReader();

            while (reader.Read())
            {
                int score = reader.GetInt32(1);
                string nickname = reader.GetString(2);

                HighscorePlayer p = new HighscorePlayer(score, nickname);               
                listOfPlayers.Add(p);
            }

            reader.Close();
            reader = null;
            dbCommand.Dispose();
            dbCommand = null;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            dbConn.Close();
        }

        return listOfPlayers;
    }

    public List<LevelData> ReadLevelData()
    {
        //List<HighscorePlayer> listOfPlayers = new List<HighscorePlayer>();
        List<LevelData> levelData = new List<LevelData>();
        LevelData blank = new LevelData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        levelData.Add(blank);

        try
        {
            dbConn.Open();

            IDbCommand dbCommand = dbConn.CreateCommand();

            string consultQuery = "SELECT * FROM LevelDataTable";

            dbCommand.CommandText = consultQuery;

            IDataReader reader = dbCommand.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int maxEnemies = reader.GetInt32(1);
                int timeLimit = reader.GetInt32(2);
                int levelToUnlock1 = reader.GetInt32(3);
                int levelToUnlock2 = reader.GetInt32(4);
                int mushroom1Chance = reader.GetInt32(5);
                int mushroom2Chance = reader.GetInt32(6);
                int mushroom3Chance = reader.GetInt32(7);
                int Skeleton1Chance = reader.GetInt32(8);
                int skeleton2Chance = reader.GetInt32(9);
                int skeleton3Chance = reader.GetInt32(10);
                int flyingDemon1Chance = reader.GetInt32(11);
                int flyingDemon2Chance = reader.GetInt32(12);
                int flyingDemon3Chance = reader.GetInt32(13);

                LevelData level = new LevelData(id, maxEnemies, timeLimit, levelToUnlock1, levelToUnlock2, mushroom1Chance, mushroom2Chance, mushroom3Chance, Skeleton1Chance,
                                                skeleton2Chance, skeleton3Chance, flyingDemon1Chance, flyingDemon2Chance, flyingDemon3Chance);

                levelData.Insert(id,level);
            }

            reader.Close();
            reader = null;
            dbCommand.Dispose();
            dbCommand = null;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            dbConn.Close();
        }

        return levelData;
    }


















}