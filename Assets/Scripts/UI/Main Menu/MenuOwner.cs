using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuOwner : MonoBehaviour
{
    [SerializeField] List<Button> mainMenuButtons;
    [SerializeField] List<Button> optionsButtons;

    [SerializeField] Animator mainMenuAnimation;
    [SerializeField] Animator sceneTransition;

    [Header("Game Begin Animation")]
    [SerializeField] float sceneTransitionWait;
    [SerializeField] float optionsWait;
    private void Awake()
    {
        EnableMenuButtons();
    }

    private void Start()
    {
        StartCoroutine(GameBeginAnimation());
    }

    #region Generic Methods
    public void EnableButtons(List<Button> buttons)
    {
        foreach (Button b in buttons)
        {
            b.enabled = true;
        }
    }
    public void DisableButtons(List<Button> buttons)
    {
        foreach (Button b in buttons)
        {
            b.enabled = false;
        }
    }
    public void ResetButtons(List<MenuButton> buttons)
    {
        foreach (MenuButton b in buttons)
        {
            b.onExit.Invoke();
        }
    }
    #endregion

    #region Main Menu Buttons
    public void EnableMenuButtons()
    {
        EnableButtons(mainMenuButtons);
    }

    public void DisableMenuButtons()
    {
        DisableButtons(mainMenuButtons);
    }

    public void ResetMenuButtons()
    {
        //ResetButtons(mainMenuButtons);
    }

    #endregion

    #region Option Buttons
    public void EnableOptionsButtons()
    {
        //EnableButtons(optionsButtons);
    }

    public void DisableOptionButtons()
    {
        //DisableButtons(optionsButtons);
    }

    public void ResetOptionButtons()
    {
        //ResetButtons(optionsButtons);
    }

    #endregion

    public IEnumerator GameBeginAnimation()
    {
        sceneTransition.SetBool("fadeOut", true);
        yield return new WaitForSeconds(sceneTransitionWait);
        mainMenuAnimation.SetTrigger("titleAnimation");
    }

}
