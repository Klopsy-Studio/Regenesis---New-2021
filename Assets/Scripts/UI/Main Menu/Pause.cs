using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;

    }
}
