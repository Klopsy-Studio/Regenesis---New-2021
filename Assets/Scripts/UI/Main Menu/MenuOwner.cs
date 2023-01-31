using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuOwner : MonoBehaviour
{
    [SerializeField] List<MenuButton> mainMenuButtons;
    [SerializeField] List<MenuButton> optionsButtons;

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
    public void EnableButtons(List<MenuButton> buttons)
    {
        foreach (MenuButton b in buttons)
        {
            b.canBeSelected = true;
        }
    }
    public void DisableButtons(List<MenuButton> buttons)
    {
        foreach (MenuButton b in buttons)
        {
            b.canBeSelected = false;
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
        ResetButtons(mainMenuButtons);
    }

    #endregion

    #region Option Buttons
    public void EnableOptionsButtons()
    {
        EnableButtons(optionsButtons);
    }

    public void DisableOptionButtons()
    {
        DisableButtons(optionsButtons);
    }

    public void ResetOptionButtons()
    {
        ResetButtons(optionsButtons);
    }

    #endregion

    public IEnumerator GameBeginAnimation()
    {
        sceneTransition.SetBool("fadeOut", true);
        yield return new WaitForSeconds(sceneTransitionWait);
        mainMenuAnimation.SetBool("titleAnimation", true);
        yield return new WaitForSeconds(optionsWait);
        mainMenuAnimation.SetBool("optionsAppear", true);

    }

}
