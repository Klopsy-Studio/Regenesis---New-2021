using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public bool allowSound = true;
    public void PlaySound(string sound)
    {
        if (allowSound)
        {
            AudioManager.instance.Play(sound);

        }
    }

    public void StopSound(string sound)
    {
        AudioManager.instance.Play(sound);
    }
}
