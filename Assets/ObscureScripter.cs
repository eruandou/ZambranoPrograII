using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class ObscureScripter : MonoBehaviour
{
    private PostProcessVolume ppVol;
    private Vignette vignette;


    [Range(0, 1)] [SerializeField] private float intensity;
    [Range(0, 1)] [SerializeField] private float smoothness;

    private void Start()
    {
        ppVol = GetComponent<PostProcessVolume>();
        vignette = ppVol.profile.settings[0] as Vignette;
        vignette.intensity.value = intensity;
        vignette.smoothness.value = this.smoothness;
    }






}
