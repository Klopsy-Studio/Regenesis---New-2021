using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pause : MonoBehaviour
{
    float originalTimeScale;

    private void Start()
    {
        originalTimeScale = Time.timeScale;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
    }
}
