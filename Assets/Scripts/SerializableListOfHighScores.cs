using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public  class SerializableListOfHighScores
{
    public List<HighscorePlayer> myHighScoresList;

    public SerializableListOfHighScores()
    {
        myHighScoresList = new List<HighscorePlayer>();
    }


}
