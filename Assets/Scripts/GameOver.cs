using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public  class GameOver : MonoBehaviour
{

    private string nickEntered;

    [SerializeField] private Text text;

    public void Save()
    {
        DataSaver.SavePlayers(Gamemanager.instance.ActualPoints, nickEntered);
        
    }


    public void Load()
    {
        DataSaver.LoadPlayers();
    }

    public void ChangeNameEntered()
    {
        nickEntered = text.text;
        Debug.Log($"new name is {nickEntered}");
    }

}
