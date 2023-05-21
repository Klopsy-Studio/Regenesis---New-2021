using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class CinematicManager : MonoBehaviour
{
    [SerializeField] VideoPlayer player;
    [SerializeField] Animator transition;
    float videoLength;

    public bool skip;
    void Start()
    {
        videoLength = (float)player.clip.length;
        transition.SetBool("fadeOut", true);
    }

    // Update is called once per frame
    void Update()
    {
        videoLength -= Time.deltaTime;

        if (videoLength <= Time.deltaTime || skip)
        {
            GameManager.instance.sceneToLoad = "Tutorial";
            player.Stop();
            transition.SetBool("fadeOut", false);
            transition.SetBool("fadeIn", true);
            Invoke("Load", 1f);
        }
    }

    public void Load()
    {
        SceneManager.LoadScene("LoadingScreen");

    }

    public void SkipCutscene()
    {
        skip = true;
    }
}
