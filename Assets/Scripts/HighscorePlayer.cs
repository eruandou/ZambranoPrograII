using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class HighscorePlayer
{


    public int Score { get; set; }

    public string Nickname { get; set; }


    public HighscorePlayer(int highscore, string nickname)
    {
        this.Score = highscore;
        this.Nickname = nickname;
    }





}
