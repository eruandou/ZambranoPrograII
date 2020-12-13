using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource [] audioSrc;
    [SerializeField] private AudioClip[] musicToPlay;

    private AudioSource currentAudioSource;
    private AudioSource nextAudioSource;

    public bool OnTransition { get; private set; }
  
    [SerializeField] private float stepTime;
    [SerializeField] private float volumeChangeOverTime;



    private void Start()
    {
        //Assign multi clips to audiosources
        for (int i = 0; i < audioSrc.Length; i++)
        {
            audioSrc[i].volume = 0;
            audioSrc[i].playOnAwake = false;
            audioSrc[i].loop = true;
            audioSrc[i].clip = musicToPlay[i];
            audioSrc[i].Play();
        }
        audioSrc[0].volume = 1;
        PlayMusic();
        //Add reference to game manager
        Gamemanager.instance.LoadMusicPlayer(this);        
    }

    public void PlayMusic()
    {
        audioSrc[0].Play();
        currentAudioSource = audioSrc[0];
    }

    public void PlayMusic (int trackToPlay)
    {
        audioSrc[trackToPlay].Play();
        currentAudioSource = audioSrc[trackToPlay];
    }

    public void StopMusic()
    {
        audioSrc[0].Stop();
    }

    public void StopMusic(int trackToStop)
    {
        audioSrc[trackToStop].Stop();
    }

    public void StartCrossFade(int newTrackToPlay, float newTargetedVolume)
    {
        if (audioSrc[newTrackToPlay] == currentAudioSource) return;

        StartCoroutine(CrossFade(newTrackToPlay, newTargetedVolume));
    }

    public IEnumerator CrossFade(int newTrackToPlay, float newTargetedVolume)
    {
        OnTransition = true;

        //Check new track to play and check volume = 0
        nextAudioSource = audioSrc[newTrackToPlay];
        nextAudioSource.volume = 0;

        if (nextAudioSource == currentAudioSource)
        {
            
        }

        //With a while loop, decrease volume of old audio and increment the new audio
        while (nextAudioSource.volume < newTargetedVolume && currentAudioSource.volume >= 0)
        {
            currentAudioSource.volume -= volumeChangeOverTime;
            nextAudioSource.volume += volumeChangeOverTime;
            yield return new WaitForSeconds(stepTime);
        }

       
        //Make sure the old audio is = 0

        currentAudioSource.volume = 0;

        //Make the new audio become the current audio source    
        
        currentAudioSource = nextAudioSource;

        nextAudioSource = null;

        //Better use percentages later

        OnTransition = false;


    }

    public void BackToMainMusic()
    {
        StartCrossFade(0, 1);
    }



}
