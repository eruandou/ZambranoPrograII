using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersFromHighScoreRow : MonoBehaviour
{
    public Text Nickname;    
    public Text Score;

    public void SetData(HighscorePlayer player)
    {
        Nickname.text = player.nickname;
        Score.text = player.score.ToString();
    }

    public void ResetData()
    {
        Nickname.text = "---";
        Score.text = "---";
    }
}
