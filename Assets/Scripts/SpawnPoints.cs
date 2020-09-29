using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{


    private Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();        
    }

    public void ActivatePortal()
    {
        StartCoroutine(ActivateAnimation());
    }


    private IEnumerator ActivateAnimation()
    {        
        anim.enabled = true;
        anim.SetBool("Appear", true);

        anim.SetBool("Dissapear", false);

        yield return new WaitForSeconds(0.7f);

        anim.SetBool("Appear", false);
        anim.SetBool("Dissapear", true);

        yield return new WaitForSeconds(0.7f);

        anim.enabled = false;

    }

}
