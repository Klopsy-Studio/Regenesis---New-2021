using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Pause : MonoBehaviour
{
    float originalTimeScale;
    public bool gameIsPaused;

    private void Start()
    {
        originalTimeScale = Time.timeScale;
    }
    public void PauseGame()
    {
        if (GameManager.instance.gameIsPaused)
            return;
        Time.timeScale = 0;

        if (Camera.main.TryGetComponent<AudioLowPassFilter>(out AudioLowPassFilter lowPass))
            lowPass.enabled = true;
        GameManager.instance.gameIsPaused = true;
    }

    public void UnPauseGame()
    {
        if (!GameManager.instance.gameIsPaused)
            return;
        
        if (Camera.main.TryGetComponent<AudioLowPassFilter>(out AudioLowPassFilter lowPass))
            lowPass.enabled = false;
        Time.timeScale = 1;
        GameManager.instance.gameIsPaused = false;

    }
}
