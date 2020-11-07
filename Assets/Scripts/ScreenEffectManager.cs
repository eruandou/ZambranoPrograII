using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ScreenEffectManager : MonoBehaviour
{

    private IScreenEffect[] filters;
    //public GameObject[] filterObjects;  
    [SerializeField] private float timeToDisableEffect;
    private bool canEnableAnotherFilter;


    private void Start()
    {
        filters = new IScreenEffect[4];
        filters[0] = GameObject.Find("GBLookFilter").GetComponent<NormalScreenEffect>();
        filters[1] = GameObject.Find("Astigmatism").GetComponent<NormalScreenEffect>();
        filters[2] = FindObjectOfType<LSDScreenEffect>();
        filters[3] = FindObjectOfType<DrunkScreenEffect>();

        for (int i = 0; i < filters.Length; i++)
        {
            Debug.Log($"{filters[i].TrackToPlay} is the track to play");
        }



        for (int i = 0; i < filters.Length; i++)
        {
            filters[i].DeActivate();
        }
    }


    public void EnableFilter(int filterToEnable)
    {
        if (!canEnableAnotherFilter) return;

        for (int i = 0; i < filters.Length; i++)
        {
            if (i == filterToEnable)
            {
                filters[i].Activate();
                StartCoroutine(DisableEffectAfterTime(filterToEnable));
                canEnableAnotherFilter = false;
                Debug.Log($"Enabled filter {i}");              
            }
            else
            {
                filters[i].DeActivate();
                Debug.Log($"Disabled filter {i}");
            }
        }
    }

    private IEnumerator DisableEffectAfterTime(int filterToDisableLater)
    {
        yield return new WaitForSeconds(timeToDisableEffect);

        canEnableAnotherFilter = true;
        filters[filterToDisableLater].DeActivate();

    }
  




}
