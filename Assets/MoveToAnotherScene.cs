using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToAnotherScene : MonoBehaviour
{
    [SerializeField] Animator sceneTransition;
    [SerializeField] AudioSource[] sources;

    [SerializeField] float timer;
    [SerializeField] float speed;

    [SerializeField] string sceneToMove;

    public void GoToNewScene()
    {
        StartCoroutine(MoveToScene());
    }
    public IEnumerator MoveToScene()
    {
        GameManager.instance.sceneToLoad = sceneToMove;

        sceneTransition.SetBool("fadeInPause", true);

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            for (int i = 0; i < sources.Length; i++)
            {
                FadeInAudioSource(sources[i]);
            }

            yield return null;
        }

        SceneManager.LoadScene("LoadingScreen");

    }

    public void FadeInAudioSource(AudioSource source)
    {
        source.volume -= Time.deltaTime * speed;
    }

    public void ChangeSceneToLoad(string scene)
    {
        sceneToMove = scene;
        Debug.Log(sceneToMove);
        
    }
}
