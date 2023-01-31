using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSceneTransition : MonoBehaviour
{
    [SerializeField] Animator sceneTransition;
    void Start()
    {
        sceneTransition.SetBool("fadeOut", true);
        sceneTransition.SetBool("fadeIn", false);
    }

}
