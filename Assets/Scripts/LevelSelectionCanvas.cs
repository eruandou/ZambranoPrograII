using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionCanvas : MonoBehaviour
{

    [SerializeField] private GameObject clearPanel, confirmPanel;




    private void Start()
    {
        ToMain();
    }








    public void ToConfirm()
    {
        clearPanel.SetActive(false);
        confirmPanel.SetActive(true);
    }

    public void ToMain()
    {
        clearPanel.SetActive(true);
        confirmPanel.SetActive(false);
    }


    public void ClearData()
    {
        LevelsSaver.ClearData();
        ToMain();
        Gamemanager.instance.LoadScene("LevelSelect");
    }



}
