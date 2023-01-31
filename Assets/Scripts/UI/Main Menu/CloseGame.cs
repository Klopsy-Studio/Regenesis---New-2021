using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CloseGame : MonoBehaviour
{
    [SerializeField] Animator sceneTransition;
    public void CloseApplication(float waitTime)
    {
        StartCoroutine(CloseGameSequence(waitTime));
    }

    IEnumerator CloseGameSequence(float waitTime)
    {
        sceneTransition.SetBool("fadeOut", false);
        sceneTransition.SetBool("fadeIn", true);
        yield return new WaitForSeconds(waitTime);

        Application.Quit();
    }
}
