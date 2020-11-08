using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public UI UI;

    public MusicPlayer musicPlayer;

    public int chanceToSpawnEnemy1, chanceToSpawnEnemy2, chanceToSpawnEnemy3;

    public EnemiesController enemiesController;

    public int lastAccessedLevel = 1;

    private TextAsset levelData;

    //private bool activeLevel;
    private float tempTime;

    private const string LEVEL_SELECT_SCENE_PATH = "LevelSelect";

    public int MaxEnemies { get; private set; }

    private int enemiesLeft;

    public int TimeLimit { get; private set; }

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



    public void OnEnemyDie(int pointsToRecieve)
    {
        GetPoints(pointsToRecieve);
        enemiesLeft--;
        UI.UpdateEnemiesLeft(enemiesLeft);
        if (enemiesLeft <= 0)
        {
            Win();
        }
    }



    public void Win()
    {
        this.enabled = false;

        if (LevelsSaver.GetLevelCompletitionState(lastAccessedLevel) == (false))
        {
            //Check level as complete
            LevelsSaver.SaveNewUnlockedLevel(lastAccessedLevel, true);
            //Unlock next levels
            foreach (int levelID in levelsToUnlock)
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

    public void LoadMusicPlayer(MusicPlayer musicPlayer)
    {
        this.musicPlayer = musicPlayer;
    }
    public void LoadEnemiesController(EnemiesController eC)
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

    public void LoadLevel(int levelToLoad)
    {
        //Clear list from before the level
        levelsToUnlock.Clear();
        //Get data from the CSV

        string[] data = levelData.text.Split(new char[] { '\n' });

        string[] row = data[levelToLoad].Split(new char[] { ',' });

        //Pass that data into corresponding variables
        int.TryParse(row[1], out int maxEnemies);
        MaxEnemies = maxEnemies;
        enemiesLeft = MaxEnemies;

        int.TryParse(row[2], out int timeLimit);
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

        int.TryParse(row[4], out chanceToSpawnEnemy1);
        int.TryParse(row[5], out chanceToSpawnEnemy2);
        int.TryParse(row[6], out chanceToSpawnEnemy3);

     

        lastAccessedLevel = levelToLoad;

        this.enabled = true;

        //Load corresponding level        
        LoadScene($"Level{levelToLoad}");
    }
       
    public void Lose()
    {
        this.enabled = false;
        LoadScene("GameOverScene");       
    }


    private void Update()
    {       
        tempTime += Time.deltaTime;
        if (tempTime >= 1)
        {
            tempTime = 0;
            TimeLimit--;
            Debug.Log(TimeLimit + " is time limit");
            if (TimeLimit <= 0)
            {
                Lose();
            }

            UI.UpdateTimeUI(TimeLimit);

        }
    }


}
