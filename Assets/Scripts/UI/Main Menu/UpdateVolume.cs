using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UpdateVolume : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    
    public void ChangeVolume()
    {
        if(slider != null)
        {
            mixer.SetFloat("Volume", Mathf.Log10(slider.value) * 20);
        }
    }
}
