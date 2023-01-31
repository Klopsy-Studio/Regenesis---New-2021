using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] Animator sceeneTransition;
    


    public void SetTransition(string sceneToLoad)
    {
        GameManager.instance.sceneToLoad = sceneToLoad;
        sceeneTransition.SetTrigger("fadeIn");
        Invoke("SetLoadingScreen", 1.5f);
    }

    public void SetLoadingScreen()
    {
        SceneManager.LoadScene("LoadingScreen");
    }
}
