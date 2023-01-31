using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // To reproduce a sound: Play("The name of the sound");

    public static AudioManager instance;

    [SerializeField] AudioMixerGroup mixer;
    public Sound[] sounds;
    private float volume = 0.5f;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        #endregion

        volume = PlayerPrefs.GetFloat("Volume", 0.5f); // For storing the General volume value

        // Set-up
        foreach (Sound sound in sounds)
        {
            // Create a GameObject with an AudioSource component dor each Sound asset
            GameObject audioSource = new GameObject("Audio source" + sound.name);
            audioSource.transform.SetParent(transform);
            sound.source = audioSource.AddComponent<AudioSource>();
            
            // Sound properties
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume * volume; // IMPORTANT: sound.volume is the Audio clip's own volume value
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.outputAudioMixerGroup= mixer;
        }
    }

    // Logic for storing new general Volume value
    public float VolumeSave
    {
        get
        {
            return volume;
        }

        set
        {
            value = Mathf.Clamp(value, 0, 1);
            PlayerPrefs.SetFloat("Volume", value);
            volume = value;

            foreach (Sound sound in sounds)
            {
                sound.source.volume = sound.volume * volume;
            }
        }
    }

    // This is for the *eventual* Volume slider in a hypothetical Settings menu
    public float Volume
    {
        get
        {
            return volume;
        }

        set
        {
            value = Mathf.Clamp(value, 0, 1);
            volume = value;
        }
    }

    // Play method
    public void Play (string name)
    {
        Sound newSound = Array.Find(sounds, sound => sound.name == name);
        newSound.source.Play();
    }

    public void Stop(string name)
    {
        Sound newSound = Array.Find(sounds, sound => sound.name == name);
        newSound.source.Stop();
    }
}
