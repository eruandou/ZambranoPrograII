using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class NormalScreenEffect : MonoBehaviour, IScreenEffect
{

    private PostProcessVolume effect;
    [SerializeField] private int trackToPlay;
    private bool active;

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
    }



    public void Activate()
    {
        effect.enabled = true;
        active = true;
        StartCoroutine(Gamemanager.instance.musicPlayer.CrossFade(TrackToPlay, 1));
    }
    public void DeActivate()
    {
        effect.enabled = false;
        active = false;
        if (Gamemanager.instance.musicPlayer.CurrentAudioSource.volume != 1) StartCoroutine(Gamemanager.instance.musicPlayer.CrossFade(0, 1));
    }

    
}
