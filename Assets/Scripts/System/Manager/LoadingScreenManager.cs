using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] Animator sceneTransition;
    [SerializeField] string sceneToLoad;

    [SerializeField] float test;
    private void Awake()
    {
        sceneTransition.SetBool("fadeOut", true);
        sceneTransition.SetBool("fadeIn", false);
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        sceneToLoad = GameManager.instance.sceneToLoad;
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneToLoad);
        loading.allowSceneActivation = false;
        while (!loading.isDone)
        {
            test = loading.progress;

            if (loading.progress >= 0.9f)
            {
                yield return new WaitForSeconds(3f);
                sceneTransition.SetBool("fadeIn", true);
                sceneTransition.SetBool("fadeOut", false);
                yield return new WaitForSeconds(1f);

                loading.allowSceneActivation = true;

            }

            yield return null;
        }

        
    }
}
