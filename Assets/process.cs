using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class process : MonoBehaviour
{

    public Shader shader;

    void Start()
    {       
        //Camera.main.RenderWithShader(shader, "GB");
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.RenderWithShader(shader, "GB");
    }
    
  
}
