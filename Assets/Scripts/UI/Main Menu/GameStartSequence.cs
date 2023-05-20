using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartSequence : MonoBehaviour
{
    [SerializeField] Animator sceneTransition;
    [SerializeField] string sceneToLoad;
    [SerializeField] float timeToWait;
    [SerializeField] AudioSource music;

    bool musicCheck = false;
    public void BeginGameStartSequence()
    {
        StartCoroutine(GameStart());
    }

    private void Update()
    {
        if (musicCheck)
        {
            music.volume -= Time.deltaTime*0.1f;
        }
    }
    IEnumerator GameStart()
    {
        musicCheck = true;
        sceneTransition.SetBool("fadeIn", true);
        sceneTransition.SetBool("fadeOut", false);

        GameManager.instance.sceneToLoad = sceneToLoad;
        AsyncOperation loading = SceneManager.LoadSceneAsync("LoadingScreen");
        loading.allowSceneActivation = false;
        while (!loading.isDone)
        {
            
            if(loading.progress >= 0.9f)
            {
                yield return new WaitForSeconds(timeToWait);
                loading.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
