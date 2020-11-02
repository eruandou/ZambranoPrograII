using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private GameObject lowScoresPanel;

    [SerializeField] private PlayersFromHighScoreRow[] hallOfFame;
    [SerializeField] private PlayersFromHighScoreRow[] hallOfInfamy;

    [SerializeField] private GameObject warning;
    
    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();

        ToMainMenu();

        DataSaver.Load();

        PopulateHighscoreLists();

        
    }

    private void PopulateHighscoreLists()
    {
        if (!DataSaver.BSTHighscores.IsTreeEmpty())
        {
            List<HighscorePlayer> BestScorePlayers = new List<HighscorePlayer>();
            List<HighscorePlayer> WorstScorePlayers = new List<HighscorePlayer>();

            int topOfList;
            if (DataSaver.BSTHighscores.nodeCount >= 5) topOfList = 5;
            else topOfList = DataSaver.BSTHighscores.nodeCount;

            for (int i = 0; i < topOfList; i++)
            {
                hallOfFame[i].SetData(DataSaver.BSTHighscores.InReverseOrder(DataSaver.BSTHighscores.root, BestScorePlayers)[i]);
                hallOfInfamy[i].SetData(DataSaver.BSTHighscores.InOrder(DataSaver.BSTHighscores.root, WorstScorePlayers)[i]);
            }
            

        }

        else
        {
            Debug.Log("Tree is empty");
        }
    }

    public void ReUpdateHighscoreLists()
    {
        for (int i = 0; i < 5; i++)
        {
            hallOfFame[i].ResetData();
            hallOfInfamy[i].ResetData();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("TestLevel");
        if (Gamemanager.instance != null) Gamemanager.instance.OnGameStart();
    }

    public void ToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        creditsPanel.SetActive(false);
        highScorePanel.SetActive(false);
        lowScoresPanel.SetActive(false);
        ButtonPressed();

    }

    public void ToCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
        highScorePanel.SetActive(false);
        lowScoresPanel.SetActive(false);
        ButtonPressed();
    }

    public void ToHighscores()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
        highScorePanel.SetActive(true);
        ButtonPressed();
    }

    public void ToHallOfInfamy()
    {
        highScorePanel.SetActive(false);
        lowScoresPanel.SetActive(true);
        ButtonPressed();
    }

    public void ToHallOfFame()
    {
        highScorePanel.SetActive(true);
        lowScoresPanel.SetActive(false);
        ButtonPressed();
    }
    
    public void Exit()
    {
        Application.Quit();
        Debug.Log("Quitting app");
    }

    public void ResetScores()
    {
        highScorePanel.SetActive(false);
        lowScoresPanel.SetActive(false);
        warning.SetActive(true);
        warning.GetComponent<Warning>().warning.SetActive(true);
        ButtonPressed();
    }

    public void ButtonPressed()
    {
        audioSrc.Play();
    }

}
