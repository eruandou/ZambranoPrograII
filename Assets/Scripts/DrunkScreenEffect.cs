﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DrunkScreenEffect : MonoBehaviour, IScreenEffect
{

    private ColorGrading colorGradingDrunk;
    public PostProcessProfile ppProf;
    private PostProcessVolume ppVol;
    private bool active;
    private FloatParameter hueShift;
    [SerializeField] private float stepTime = 0.05f;


    [SerializeField] private int trackToPlay = 3;
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
        colorGradingDrunk = ppVol.profile.settings[2] as ColorGrading;
        hueShift = new FloatParameter();       
    }




    public void Activate()
    {
        ppVol.enabled = true;
        hueShift.value = 0;
        colorGradingDrunk.hueShift.value = hueShift.value;
        active = true;
        StartCoroutine(Gamemanager.instance.musicPlayer.CrossFade(TrackToPlay, 1));
        StartCoroutine(DrunkScreenProgretion());
    }

    private IEnumerator DrunkScreenProgretion()
    {

        while (active)
        {
            hueShift.value += 1;
            if (hueShift.value > 180) hueShift.value = -180;
            colorGradingDrunk.hueShift.value = hueShift.value;
            yield return new WaitForSeconds(stepTime);
        }
    }



    public void DeActivate()
    {
        ppVol.enabled = false;
        active = false;
        if (Gamemanager.instance.musicPlayer.CurrentAudioSource.volume != 1) StartCoroutine(Gamemanager.instance.musicPlayer.CrossFade(0, 1));
    }











}
