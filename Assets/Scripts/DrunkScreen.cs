using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DrunkScreen : MonoBehaviour, IScreenEffect 
{
    private LensDistortion lens;
    private PostProcessVolume ppVol;
    [SerializeField] private float stepXModifier;
    [SerializeField] private float stepYModifier;
    private bool active;
    private FloatParameter x;
    private FloatParameter y;
    [SerializeField] private float stepTime = 0.05f;
    private bool leftX, leftY;
    private AudioSource audioSrc;


    [SerializeField] private int trackToPlay = 2;
    public int TrackToPlay
    {
        get
        {
          return trackToPlay;
        }

        set
        {
            trackToPlay = value;
        }

    }
    


    private void Awake()
    {
        ppVol = GetComponent<PostProcessVolume>();
        lens = ppVol.profile.settings[0] as LensDistortion;      
        x = new FloatParameter();
        y = new FloatParameter();
        x.value = lens.intensityX;
        y.value = lens.intensityY;
        audioSrc = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        ppVol.enabled = true;
        active = true;
        audioSrc.Play();
        Gamemanager.instance.musicPlayer.StartCrossFade(TrackToPlay, 1);
        StartCoroutine(MakeScreenGoCrazy());
    }

    private IEnumerator MakeScreenGoCrazy()
    {

        while (active)
        {
           

            if (leftX)
            {
                x.value -= Time.deltaTime * stepXModifier;
                
            }
            else
            {
                x.value += Time.deltaTime * stepXModifier;
            }

           if (x.value <= 0 || x.value >= 1)
            {
                leftX = !leftX;
            }

            if (leftY)
            {
                y.value -= Time.deltaTime * stepYModifier;

            }
            else
            {
                y.value += Time.deltaTime * stepYModifier;
            }

            if (y.value <= 0 || y.value >= 1)
            {
                leftY = !leftY;
            }
            

            lens.intensityX.value = x.value;
            lens.intensityY.value = y.value;

            Debug.Log($"x is {x.value} and y is {y.value}");

            yield return new WaitForSeconds(stepTime);
        }





        
    }


    public void DeActivate()
    {
        ppVol.enabled = false;
        active = false;
        Gamemanager.instance.musicPlayer.BackToMainMusic();
    }









}
