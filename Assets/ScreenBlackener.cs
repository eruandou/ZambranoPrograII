using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBlackener : MonoBehaviour
{

    private Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
        Gamemanager.instance.GetBlackenerReference(this);
    }

    public void CloseCurtains()
    {
        anim.SetTrigger("Close");
        Debug.Log("Triggered curtains");
    }
}
