using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public UI UI;

    public MusicPlayer musicPlayer;

    public int SecretItemCount { get; private set; }

    private ScreenBlackener screenBlackener;

    private Database dbLevels;
    public Database secretItemsDatabase;

    public int chanceToSpawnEnemy1, chanceToSpawnEnemy2, chanceToSpawnEnemy3,chanceToSpawnEnemy4,
        chanceToSpawnEnemy5,chanceToSpawnEnemy6, chanceToSpawnEnemy7, chanceToSpawnEnemy8, chanceToSpawnEnemy9;

    public EnemiesController enemiesController;

    public int lastAccessedLevel = 1;

    private List<LevelData> allLevelsData;

    //private bool activeLevel;
    private float tempTime;

    private const string LEVEL_SELECT_SCENE_PATH = "LevelSelect";
    private const string SECRET_ENDING_SCENE_PATH = "SecretEnding";
    private const string NORMAL_ENDING_SCENE_PATH = "NormalEnding";

    private const string LEVELDATA_DATABASE_NAME = "LevelDataTable";
    private const string SECRET_ITEM_COLLECTED_DATABASE_NAME = "SecretItemCollectedDataTable";

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

        dbLevels = new Database(LEVELDATA_DATABASE_NAME);
        secretItemsDatabase = new Database(SECRET_ITEM_COLLECTED_DATABASE_NAME);

        dbLevels.CreateLevelDataTable();


        allLevelsData = dbLevels.ReadLevelData();
    }

    public void OnGameStart()
    {
        ActualPoints = 0;
        Debug.Log("On level load used");
    }

    public void GetBlackenerReference (ScreenBlackener screenBlackenerInstance)
    {
        this.screenBlackener = screenBlackenerInstance;
    }

    public void OnEnemyDieHandler(int pointsToRecieve, int extraTime)
    {
        GetPoints(pointsToRecieve);
        enemiesLeft--;
        TimeLimit += extraTime;
        UI.UpdateTimeUI(TimeLimit);
        UI.UpdateEnemiesLeft(enemiesLeft);
        if (enemiesLeft <= 0)
        {
            Win();
        }
    }



    public void Win()
    {
        this.enabled = false;

        if (LevelsSaver.GetLevelCompletitionState(lastAccessedLevel) == false)
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

        bool[] secretItemStates = secretItemsDatabase.GetSecretItemState();

        if (lastAccessedLevel == 11)
        {
            if (secretItemStates[0] && secretItemStates[1] && secretItemStates[2])
            {
              StartCoroutine(LoadScene(SECRET_ENDING_SCENE_PATH));
            }
            else
            {
                StartCoroutine(LoadScene(NORMAL_ENDING_SCENE_PATH));
            }

            LevelsSaver.ClearData();


            return;
            
        }

        

        StartCoroutine(LoadScene(LEVEL_SELECT_SCENE_PATH));

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

    public IEnumerator LoadScene(string levelToLoad)
    {
        screenBlackener.CloseCurtains();

        yield return new WaitForSeconds(1);

        Debug.Log("Loading level");
        SceneManager.LoadSceneAsync(levelToLoad);
    }

    

    public void LoadLevel(int levelToLoad)
    {
        //Clear list from before the level
        levelsToUnlock.Clear();
        //Get data from the database

        LevelData levelDataToLoad = allLevelsData[levelToLoad];

      
        //Pass that data into corresponding variables

        this.MaxEnemies = levelDataToLoad.MaxEnemies;
        this.enemiesLeft = MaxEnemies;
      

        this.TimeLimit = levelDataToLoad.TimeLimit;
      

        //Add levels to unlock to list      
        

        levelsToUnlock.Add(levelDataToLoad.UnlockLevel1);
        levelsToUnlock.Add(levelDataToLoad.UnlockLevel2);

        //Add enemy spawn rate chances to current level manager
        this.chanceToSpawnEnemy1 = levelDataToLoad.Mushroom1Chance;
        this.chanceToSpawnEnemy2 = levelDataToLoad.Mushroom2Chance;
        this.chanceToSpawnEnemy3 = levelDataToLoad.Mushroom3Chance;
        this.chanceToSpawnEnemy4 = levelDataToLoad.Skeleton1Chance;
        this.chanceToSpawnEnemy5 = levelDataToLoad.Skeleton2Chance;
        this.chanceToSpawnEnemy6 = levelDataToLoad.Skeleton3Chance;
        this.chanceToSpawnEnemy7 = levelDataToLoad.FlyingDemon1Chance;
        this.chanceToSpawnEnemy8 = levelDataToLoad.FlyingDemon2Chance;
        this.chanceToSpawnEnemy9 = levelDataToLoad.FlyingDemon3Chance;


        lastAccessedLevel = levelToLoad;

        this.enabled = true;

        //Load corresponding level        
        StartCoroutine(LoadScene($"Level{levelToLoad}"));
    }
       
    public void Lose()
    {
        this.enabled = false;
        StartCoroutine(LoadScene("GameOverScene"));       
    }


    private void Update()
    {       
        tempTime += Time.deltaTime;

        if (tempTime >= 1)
        {
            tempTime = 0;
            TimeLimit--;
            if (TimeLimit <= 0)
            {
                Lose();
            }

            UI.UpdateTimeUI(TimeLimit);
        }
    }


    public void GetSecretItem(int itemNumber)
    {  
        secretItemsDatabase.UpdateSecretItemsCollected(itemNumber);
    }

}
