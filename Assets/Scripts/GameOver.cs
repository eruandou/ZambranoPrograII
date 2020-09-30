using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver: MonoBehaviour
{


    [SerializeField] private Text nick;
    public void Save()
    {
        DataSaver.Save(Gamemanager.instance.ActualPoints, nick.text);
        SceneManager.LoadScene("MainMenu");
    }

  
}
