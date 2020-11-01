using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length;
    private float length2;
    private float start_pos;
    private float start_pos_2;
    public float parallax_effect;
    public float parallax_effect_2;    
    private Transform cam;
 
    void Start()
    {
        start_pos = transform.position.x;
        Vector2 bounds = GetComponent<SpriteRenderer>().bounds.size;
        length = bounds.x;
        length2 = bounds.y;       
        cam = Camera.main.transform;
        start_pos_2 = transform.position.y;
    }

    void FixedUpdate()
    {
        float temp = (cam.position.x * (1 - parallax_effect));
        float temp2 = (cam.position.y * (1 - parallax_effect_2));

        float distance = (cam.position.x * parallax_effect);
        float distance_2 = (cam.position.y * parallax_effect_2);

        transform.position = new Vector3(start_pos + distance, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, start_pos_2 + distance_2, transform.position.z);

        if (temp > start_pos + length)
        {
            start_pos += length;
        }
        else if (temp < start_pos - length)
        {
            start_pos -= length;
        }

        if (temp2 > start_pos_2 + length2)
        {
            start_pos_2 += length2;
        }
        else if (temp2 < start_pos_2 - length2)
        {
            start_pos_2 -= length2;
        }

    }
}