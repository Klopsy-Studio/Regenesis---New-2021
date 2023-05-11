using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySound(string sound)
    {
        AudioManager.instance.Play(sound);
    }

    public void StopSound(string sound)
    {
        AudioManager.instance.Play(sound);
    }
}
