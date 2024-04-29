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
        if (GameManager.instance.gameIsPaused)
            return;
        Time.timeScale = 0;
        GameManager.instance.gameIsPaused = true;
    }

    public void UnPauseGame()
    {
        if (!GameManager.instance.gameIsPaused)
            return;
        
        Time.timeScale = 1;
        GameManager.instance.gameIsPaused = false;

    }
}
