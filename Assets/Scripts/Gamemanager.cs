using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager instance;

    public UI UI;

    public EnemiesController enemiesController;

    public int lastAccessedLevel = 1;

    private TextAsset levelData;

    public int MaxEnemies { get; private set; }

    public float TimeLimit { get; private set; }

    public int ActualPoints { get; private set; }


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

    public void LoadUI(UI UI)
    {
        this.UI = UI;
    }

    public void LoadEnemiesController (EnemiesController eC)
    {
        this.enemiesController = eC;
    }

    public void GetPoints(int points)
    {
        ActualPoints += points;
        Debug.Log($"got {points} points and now I have {ActualPoints}");
        //play sound
        UI.UpdatePoints();
    }

    public void LoadScene(string levelToLoad)
    {
        SceneManager.LoadSceneAsync(levelToLoad);
    }

    public void LoadLevel (int levelToLoad)
    {
        string[] data = levelData.text.Split(new char[] {'\n' });

       string [] row = data[levelToLoad].Split(new char[] { ',' });

        int.TryParse(row[1], out int maxEnemies);
        MaxEnemies = maxEnemies;

        float.TryParse(row[2], out float timeLimit);
        TimeLimit = timeLimit;

        lastAccessedLevel = levelToLoad;

        LoadScene($"Level{levelToLoad}");
    }
    
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
        Debug.Log("Loaded scene");
    }

   



}
