using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderManagers : MonoBehaviour
{
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    void Start()
    {
        //sfxSlider.value = GameManager.instance.previousSfxSliderValue;
        //musicSlider.value = GameManager.instance.previousMusicSliderValue;
    }

    private void OnDestroy()
    {
        GameManager.instance.RecordSliderValues(sfxSlider.value, musicSlider.value);
    }
}
