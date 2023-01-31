using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartSequence : MonoBehaviour
{
    [SerializeField] Animator sceneTransition;
    [SerializeField] string sceneToLoad;
    public void BeginGameStartSequence()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        sceneTransition.SetBool("fadeIn", true);
        sceneTransition.SetBool("fadeOut", false);

        GameManager.instance.sceneToLoad = sceneToLoad;
        AsyncOperation loading = SceneManager.LoadSceneAsync("LoadingScreen");
        loading.allowSceneActivation = false;
        while (!loading.isDone)
        {
            if(loading.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                loading.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
