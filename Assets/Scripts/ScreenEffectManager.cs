using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ScreenEffectManager : MonoBehaviour
{

    private IScreenEffect[] filters;
    [SerializeField] private float timeToDisableEffect;
    private bool canEnableAnotherFilter;


    private void Start()
    {
        filters = new IScreenEffect[4];
        filters[0] = GameObject.Find("GBLookFilter").GetComponent<NormalScreenEffect>();
        filters[1] = GameObject.Find("Astigmatism").GetComponent<NormalScreenEffect>();
        filters[2] = FindObjectOfType<DrunkScreen>();
        filters[3] = FindObjectOfType<LSDScreenEffect>();

        canEnableAnotherFilter = true;

        for (int i = 0; i < filters.Length; i++)
        {
            filters[i].DeActivate();
        }
    }


    public void EnableFilter(int filterToEnable)
    {
        if (!canEnableAnotherFilter || Gamemanager.instance.musicPlayer.OnTransition ) return;

        for (int i = 0; i < filters.Length; i++)
        {
            if (i == filterToEnable)
            {
                filters[i].Activate();
                StartCoroutine(DisableEffectAfterTime(filterToEnable));
            }
            else
            {
                filters[i].DeActivate();
            }
        }
        canEnableAnotherFilter = false;
    }

    private IEnumerator DisableEffectAfterTime(int filterToDisableLater)
    {
        yield return new WaitForSeconds(timeToDisableEffect);

        canEnableAnotherFilter = true;
        filters[filterToDisableLater].DeActivate();

    }
  




}
