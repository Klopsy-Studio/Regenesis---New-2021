using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Button Checks")]
    public bool test;
    public bool canBeSelected;
    public bool selected;
    [Space]

    [Header("Variables")]
    [SerializeField] Color defaultTextColor;
    [SerializeField] Color highlightTextColor;
    [SerializeField] float appearPosition;
    [SerializeField] float buttonSpeed = 2;


    [Header("Toggle Images")]
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite toggledSprite;
    bool toggleIndex = false;

    float originalPosition;
    [Space]
    [Header("References")]
    [SerializeField] Text buttonText;
    [SerializeField] Image buttonImage;
    [SerializeField] Animator buttonAnimations;
    [SerializeField] RectTransform buttonRect;

    public UnityEvent action;
    public UnityEvent onHover;
    public UnityEvent onExit;
    public UnityEvent onUp;

    bool appear = false;
    bool hide = false;


    bool toggledPosition = true; //By default is opened


    float currentTime;

    void Start()
    {
        if(buttonRect != null)
        {
            originalPosition = buttonRect.localPosition.x;
        }
    }

    public void OnNewGameClicked()
    {
        //create a new game - initialize game data
        DataPersistenceManager.instance.NewGame();
        //load the gameplay scene - whick will in turn save the game because of 
        //OnSceneUnloaded() in the DataPersistenceManager
    }

    void Update()
    {
        if (appear)
        {
            buttonRect.localPosition = new Vector3(Mathf.Lerp(buttonRect.localPosition.x, appearPosition, currentTime), buttonRect.localPosition.y, buttonRect.localPosition.z);
            currentTime += Time.deltaTime * buttonSpeed;

            if(originalPosition < appearPosition)
            {
                if (buttonRect.localPosition.x >= appearPosition)
                {
                    buttonRect.localPosition = new Vector3(appearPosition, buttonRect.localPosition.y, buttonRect.localPosition.z);
                    currentTime = 0;
                    appear = false;
                }
            }

            else
            {
                if (buttonRect.localPosition.x <= appearPosition)
                {
                    buttonRect.localPosition = new Vector3(appearPosition, buttonRect.localPosition.y, buttonRect.localPosition.z);
                    currentTime = 0;
                    appear = false;
                }
            }
            
        }

        if (hide)
        {
            
            buttonRect.localPosition = new Vector3(Mathf.Lerp(buttonRect.localPosition.x, originalPosition, currentTime), buttonRect.localPosition.y, buttonRect.localPosition.z);
            currentTime += Time.deltaTime * buttonSpeed;


            if(appearPosition < originalPosition)
            {
                if (buttonRect.localPosition.x >= originalPosition)
                {
                    buttonRect.localPosition = new Vector3(originalPosition, buttonRect.localPosition.y, buttonRect.localPosition.z);
                    currentTime = 0;
                    hide = false;
                }
            }

            else
            {
                if (buttonRect.localPosition.x <= originalPosition)
                {
                    buttonRect.localPosition = new Vector3(originalPosition, buttonRect.localPosition.y, buttonRect.localPosition.z);
                    currentTime = 0;
                    hide = false;
                }
            }
            
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (canBeSelected && selected)
        {
            if (action != null)
            {
               action.Invoke();
            }
        }
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (canBeSelected && selected)
        {
            if(onUp != null)
            {
                onUp.Invoke();
            }
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //if (canBeSelected && selected)
        //{
        //    if (action != null)
        //    {
        //        action.Invoke();
        //    }
        //}
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            onHover.Invoke();
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            onExit.Invoke();
        }
    }

    public virtual void HighlightText()
    {
        buttonText.color = highlightTextColor;
    }

    public virtual void DefaultText()
    {
        buttonText.color = defaultTextColor;
    }

    public virtual void ChangeButtonImage(Sprite newSprite)
    {
        buttonImage.sprite = newSprite;
    }
    public void Selected()
    {
        selected = true;
    }

    public void UnSelected()
    {
        selected = false;
    }

    public void ActivateButton()
    {
        canBeSelected = true;
    }

    public void DeactivateButton()
    {
        canBeSelected = false;
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }


    public void PlayAnimation(string trigger)
    {
        if(buttonAnimations != null)
        {
            buttonAnimations.SetTrigger(trigger);
        }
    }


    public void MakeButtonAppear()
    {
        appear = true;
        hide = false;
        currentTime = 0;
    }

    public void MakeButtonHide()
    {
        hide = true;

        appear = false;
        currentTime = 0;
    }


    public void SetButtonPosition(Vector3 newPosition)
    {
        buttonRect.localPosition = newPosition;
    }

    public void SetDefaultPosition()
    {
        buttonRect.localPosition = new Vector3(originalPosition, buttonRect.localPosition.y, buttonRect.localPosition.z);
    }

    public void ToggleSprite()
    {
        toggleIndex = !toggleIndex;

        if (toggleIndex)
        {
            buttonImage.sprite = toggledSprite;
        }
        else
        {
            buttonImage.sprite = defaultSprite;
        }
    }
    public void SetDefaultSprite()
    {
        buttonImage.sprite = defaultSprite;
        toggleIndex = false;
    }


    public void TogglePosition()
    {
        toggledPosition = !toggledPosition;

        if (toggledPosition)
        {
            MakeButtonAppear();
        }
        else
        {
            MakeButtonHide();
        }
    }
}
