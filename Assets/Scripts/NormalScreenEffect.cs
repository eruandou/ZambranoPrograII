using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class NormalScreenEffect : MonoBehaviour, IScreenEffect
{

    private PostProcessVolume effect;
    [SerializeField] private int trackToPlay;
    private bool active;
    private AudioSource audioSrc;
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
        effect = GetComponent<PostProcessVolume>();
        audioSrc = GetComponent<AudioSource>();
    }



    public void Activate()
    {
        effect.enabled = true;
        active = true;
        audioSrc.Play();
        Gamemanager.instance.musicPlayer.StartCrossFade(TrackToPlay, 1);
    }
    public void DeActivate()
    {
        effect.enabled = false;
        active = false;
        Gamemanager.instance.musicPlayer.BackToMainMusic();
    }

    
}
