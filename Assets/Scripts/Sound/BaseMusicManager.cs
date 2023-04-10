using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMusicManager : MonoBehaviour
{
    AudioSource currentMusic;
    AudioSource musicToChange;
    [SerializeField] AudioSource baseMusic;
    [SerializeField] AudioSource barracksMusic;
    [SerializeField] AudioSource forgeMusic;
    [SerializeField] AudioSource shopMusic;

    [SerializeField] float targetVolume;

    bool turningMusicOn;
    bool turningMusicOff;
    [SerializeField] float time;
    [SerializeField] float changeSpeed;
    void Start()
    {
        baseMusic.volume = targetVolume;

        forgeMusic.volume = 0;
        barracksMusic.volume = 0;
        shopMusic.volume = 0;
    }

    void Update()
    {
        if (turningMusicOn)
        {
            time += Time.deltaTime;
            musicToChange.volume = Mathf.Lerp(musicToChange.volume, targetVolume, time/changeSpeed);

            if(time >= changeSpeed)
            {
                Debug.Log("Finished");

                musicToChange.volume = targetVolume;

                turningMusicOn = false;
                time = 0;
            }
        }

        if (turningMusicOff)
        {
            time += Time.deltaTime;
            musicToChange.volume = Mathf.Lerp(musicToChange.volume, 0, time / changeSpeed);

            if (time >= changeSpeed)
            {
                Debug.Log("Finished");

                musicToChange.volume = 0f;

                turningMusicOff = false;
                time = 0;
            }
        }
    }
    public void TurnOnMusic(AudioSource newMusic)
    {
        if (turningMusicOff)
        {
            turningMusicOff = false;
        }
        turningMusicOn = true;
        musicToChange = newMusic;
        time = 0;
    }

    public void TurnOffMusic(AudioSource newMusic)
    {
        if (turningMusicOn)
        {
            turningMusicOn = false;
        }
        turningMusicOff = true;
        musicToChange = newMusic;
        time = 0;
    }

}
