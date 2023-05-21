using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuContinueButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] MenuOwner menuOwner;
    [SerializeField] MenuButton menuButton;
    [SerializeField] GameStartSequence gameStartSequence;

    [SerializeField] Color defaultTextColor;
    [SerializeField] Color highlightTextColor;
    [SerializeField] Color disableColor;
    [SerializeField] CampCursorUIInteraction interaction;
    [SerializeField] Text text;


    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            text.color = disableColor;
            menuButton.canBeSelected = false;
            interaction.canSelect = false;

            Debug.Log("No save data");
        }

        else
        {
            text.color = defaultTextColor;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            return;
        }
        menuOwner.DisableMenuButtons();
        OnContinueGameClicked();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            return;
        }

        text.color = highlightTextColor;

    }

    public void OnContinueGameClicked() //Unity Buttons
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            Debug.LogWarning("NO SAVE DATA");

            return;
        }

        DataPersistenceManager.instance.SaveGame();
        gameStartSequence.BeginGameStartSequence();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            return;
        }
        text.color = defaultTextColor;
    }
}
