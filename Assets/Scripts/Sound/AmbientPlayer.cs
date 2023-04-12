using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;


    float time;
    float timeOffset;

    bool playing;

    [SerializeField] float minTimeOffset;
    [SerializeField] float maxTimeOffset;

    void Start()
    {
        if(source != null)
        {
            source.Play();
            time = source.clip.length;
            playing = true;
        }

        else
        {
            Debug.Log("Source is null");
        }
        
    }

    void Update()
    {
        if(source != null)
        {
            if (playing)
            {
                time -= Time.deltaTime;

                if (time <= 0)
                {
                    playing = false;
                    time = source.clip.length;
                    timeOffset = Random.Range(minTimeOffset, maxTimeOffset);
                }
            }
            else
            {
                timeOffset -= Time.deltaTime;
                if (timeOffset <= 0)
                {
                    source.Play();
                    timeOffset = 0;
                    playing = true;
                }
            }
        }
        
        
    }
}
