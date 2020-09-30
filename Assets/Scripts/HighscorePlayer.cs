using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class HighscorePlayer
{


    public int score;

    public string nickname;

    public HighscorePlayer(int highscore, string nickname)
    {
        this.score = highscore;
        this.nickname = nickname;
    }





}
