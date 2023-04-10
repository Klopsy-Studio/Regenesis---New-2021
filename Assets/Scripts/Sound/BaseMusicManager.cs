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

    bool updatingMusic;
    [SerializeField] float time;
    [SerializeField] float changeSpeed;
    void Start()
    {
        baseMusic.volume = targetVolume;
        currentMusic = baseMusic;

        forgeMusic.volume = 0;
        barracksMusic.volume = 0;
        shopMusic.volume = 0;
    }

    void Update()
    {
        if (updatingMusic)
        {
            time += Time.deltaTime;

            currentMusic.volume = Mathf.Lerp(currentMusic.volume, 0, time/changeSpeed);

            musicToChange.volume = Mathf.Lerp(musicToChange.volume, targetVolume, time/changeSpeed);

            if(time >= changeSpeed)
            {
                Debug.Log("Finished");

                currentMusic.volume = 0;
                musicToChange.volume = targetVolume;

                currentMusic = musicToChange;
                updatingMusic = false;
                time = 0;
            }
        }
    }
    public void ChangeMusic(AudioSource newMusic)
    {
        updatingMusic = true;
        musicToChange = newMusic;
        time = 0;
    }

}
