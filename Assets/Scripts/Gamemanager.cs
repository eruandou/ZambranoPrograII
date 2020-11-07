﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public UI UI;

    public MusicPlayer musicPlayer;

    public EnemiesController enemiesController;

    public int lastAccessedLevel = 1;

    private TextAsset levelData;

    private const string LEVEL_SELECT_SCENE_PATH = "LevelSelect";

    public int MaxEnemies { get; private set; }

    private int killedEnemies;

    public float TimeLimit { get; private set; }

    public int ActualPoints { get; private set; }

    private List<int> levelsToUnlock = new List<int>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        levelData = Resources.Load<TextAsset>("LevelData");        
    }

    public void OnGameStart()
    {
        ActualPoints = 0;
        Debug.Log("On level load used");  
    }

    public void OnLevelLoad(int nodeID)
    {
        lastAccessedLevel = nodeID;
    }

    public void OnEnemyDie(int pointsToRecieve)
    {
        GetPoints(pointsToRecieve);
        killedEnemies++;
        if (killedEnemies >= MaxEnemies)
        {
            Win();
        }
    }
    

    public void Win()
    {     
           
        if (LevelsSaver.GetLevelCompletitionState(lastAccessedLevel) == (false))
        {
            //Check level as complete
            LevelsSaver.SaveNewUnlockedLevel(lastAccessedLevel, true);
            //Unlock next levels
            foreach (int levelID in levelsToUnlock )
            {
                LevelsSaver.SaveNewUnlockedLevel(levelID, false);
            }

        }



        //return to Hub

        LoadScene(LEVEL_SELECT_SCENE_PATH);
   
    }

 

    public void LoadUI(UI UI)
    {
        this.UI = UI;
    }
    
    public void LoadMusicPlayer (MusicPlayer musicPlayer)
    {
        this.musicPlayer = musicPlayer;
    }
    public void LoadEnemiesController (EnemiesController eC)
    {
        this.enemiesController = eC;
    }

    public void GetPoints(int points)
    {
        ActualPoints += points;
        //play sound
        UI.UpdatePoints();
    }

    public void LoadScene(string levelToLoad)
    {
        SceneManager.LoadSceneAsync(levelToLoad);
    }

    public void LoadLevel (int levelToLoad)
    {
        //Clear list from before the level
        levelsToUnlock.Clear();

        //Get data from the CSV

        string[] data = levelData.text.Split(new char[] { '\n' });

        string[] row = data[levelToLoad].Split(new char[] { ',' });

        //Pass that data into corresponding variables
        int.TryParse(row[1], out int maxEnemies);
        MaxEnemies = maxEnemies;

        float.TryParse(row[2], out float timeLimit);
        TimeLimit = timeLimit;

        //Add levels to unlock to list      


        //Separate the multiple level values this could have
        //in each cell, so that each value, individually,
        //gets added to the corresponding list
        string[] cell = row[3].Split(new char[] { ' ' });

        for (int i = 0; i < cell.Length; i++)
        {
            int.TryParse(cell[i], out int level);
            levelsToUnlock.Add(level);
        }      

        lastAccessedLevel = levelToLoad;
        
        //Load corresponding level
        LoadScene($"Level{levelToLoad}");
    }
    
    public void Lose()
    {
        LoadScene("GameOverScene");       
    }

   



}
