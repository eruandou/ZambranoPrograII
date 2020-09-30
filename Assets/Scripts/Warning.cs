using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour
{
    [SerializeField] private GameObject clearDone;
    public GameObject warning;
    [SerializeField] private GameObject highscores;
    [SerializeField] private GameObject lowScores;
    [SerializeField] private MainMenu mainMenu;




    private void Start()
    {
        warning.SetActive(true);
    }

    private void OnEnable()
    {
     if (warning.activeSelf == false)
        {
            warning.SetActive(true);
        }    
    }


    private void Update()
    {
        if (warning.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DataSaver.ClearData();
                Debug.Log("Data cleared");
                warning.SetActive(false);
                clearDone.SetActive(true);
                mainMenu.ReUpdateHighscoreLists();
                
                StartCoroutine(OutOfWarning());
        }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                warning.SetActive(false);
                highscores.SetActive(true);
            }
        }
    }

    private IEnumerator OutOfWarning()
    {
        yield return new WaitForSeconds(1);
        clearDone.SetActive(false);
        highscores.SetActive(true);        
    }
    
}
