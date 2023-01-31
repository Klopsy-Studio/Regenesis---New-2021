using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StopSound : MonoBehaviour
{
    public void StopSpecificSound(string soundToStop)
    {
        AudioManager.instance.Stop(soundToStop);
    }
}
