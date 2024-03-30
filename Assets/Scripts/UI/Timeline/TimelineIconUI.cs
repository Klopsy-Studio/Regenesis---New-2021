using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class TimelineIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Icon position variables")]
    public TimelineLane lane;
    public RectTransform groupPos;
    public TextMeshProUGUI velocityText;
    public RectTransform rectTransform;
    public Image icon;
    [Space]
    [Header("Icon supports")]
    public Image upSupport;
    public Image downSupport;
    public Image middleUpSupport;
    public Image middleDownSupport;
    [Space]
    [Header("Unit Speed")]
    public Image speedImageComponent;
    public Sprite[] speedSprites;
    [Space]
    [HideInInspector] public bool mouseOver;
    [HideInInspector] public bool selected;

    [Header("Icon animations")]
    public Animator iconAnimationsTimeline;
    [SerializeField] GameObject iconHighlight;
    public GameObject iconStunnedIndicator;
    [Space]

    [HideInInspector] public TimelineElements element;


    [HideInInspector] public int indexChild;
   

    public int offset;
    public int originalOffset;

    public float barSize;

    public RectTransform iconTransform;

    [HideInInspector] public TimelineIconUI prevIcon;
    [HideInInspector] public TimelineIconUI nextIcon;
    [HideInInspector] public TimelineUI owner;

    public float minDistance;
    public Vector2 previousPosition;
    public float maxDistance;

    [HideInInspector] public bool timelineEnabled = true;
    [SerializeField] bool allowExpandUnits;

    [HideInInspector] public bool isActing;

    [HideInInspector] public bool enableUpdate;

    private void Start()
    {
        this.gameObject.name = element.name;
    }
    
   
    public void EnableAppear()
    {
        iconAnimationsTimeline.SetTrigger("appear");
    }

    public void EnableDisappear()
    {
        iconAnimationsTimeline.SetTrigger("disappear");
    }

    private void OnEnable()
    {
        if(element != null)
        {
            iconAnimationsTimeline.SetFloat("character", element.timelineIconIndex);
        }
    }
    void Update()
    {
        if (timelineEnabled)
        {
            iconTransform.anchoredPosition = new Vector2(-barSize / 2 + element.GetActionBarPosition() * barSize, offset);
        }

        if (enableUpdate)
        {
            //UpdatePreviousIcon();
            UpdateNextIcon();
            UpdatePreviousIcon();
        }
       
    }

    public void SavePosition()
    {
        previousPosition = iconTransform.anchoredPosition = new Vector2(-barSize / 2 + element.GetActionBarPosition() * barSize, offset);
    }
    public void PutPreviousOnTop()
    {
        if (!isActing)
        {
            if (prevIcon != null)
            {
                if (element.timelineFill - prevIcon.element.timelineFill <= minDistance)
                {
                    prevIcon.SavePosition();
                    prevIcon.iconTransform.position = new Vector2(groupPos.position.x, groupPos.position.y);

                    switch (prevIcon.lane)
                    {
                        case TimelineLane.Top:
                            prevIcon.downSupport.gameObject.SetActive(false);
                            break;
                        case TimelineLane.Bottom:
                            prevIcon.upSupport.gameObject.SetActive(false);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
       
    }

    public void ResetPrevious()
    {
        if(prevIcon != null)
        {
            prevIcon.iconTransform.anchoredPosition = prevIcon.previousPosition;

            switch (prevIcon.lane)
            {
                case TimelineLane.Top:
                    downSupport.gameObject.SetActive(true);
                    break;
                case TimelineLane.Bottom:
                    upSupport.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        
    }
  
    public void SetTimelineIconTextVelocity()
    {
        if(velocityText != null)
        {
            var timelineVelocity = (int)element.TimelineVelocity;

            if(element.timelineVelocity == TimelineVelocity.Stun)
            {
                velocityText.SetText("");
            }

            else
            {
                velocityText.SetText(timelineVelocity.ToString());
            }

        }

    }

    public void UpdatePreviousIcon()
    {
        if (prevIcon == null)
            return;
        if (element.timelineFill - prevIcon.element.timelineFill >= maxDistance && element.timelineTypes == prevIcon.element.timelineTypes)
        {
            ReturnIcon(this);
        }

    }

    public void UpdateNextIcon()
    {
        if (nextIcon == null)
            return;

        if(nextIcon.element.timelineFill - element.timelineFill <= minDistance && element.timelineTypes == nextIcon.element.timelineTypes && element.fTimelineVelocity >= nextIcon.element.fTimelineVelocity)
        {
            PutIconUpwards(this);
        }
    }
    public void ReturnIcon(TimelineIconUI icon)
    {
        if(icon != null)
        {
            icon.offset = icon.originalOffset;
            icon.downSupport.gameObject.SetActive(true);
        }
    }

    public void PutIconUpwards(TimelineIconUI icon)
    {
        if(icon != null)
        {
            icon.offset = 115;
            icon.downSupport.gameObject.SetActive(false);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(owner.selectedIcon != null)
        {
            if(owner.selectedIcon != this)
            {
                if (owner.selectedIcon.selected)
                {
                    owner.selectedIcon.selected = false;
                }
            }
            
        }
        owner.selectedIcon = this;
        mouseOver = true;
    }

    public void Grow()
    {
        //iconAnimations.SetBool("isGrow", true);
    }

    public void Return()
    {
        //iconAnimations.SetBool("isGrow", false);
    }


    #region VFX
    public void ActivateIconHightlight()
    {
        iconHighlight.SetActive(true);
    }
    
    public void DeactivateIconHightlight()
    {
        iconHighlight.SetActive(false);
    }
    public void EnableStun()
    {
        iconStunnedIndicator.SetActive(true);
    }

    public void DisableStun()
    {
        iconStunnedIndicator.SetActive(false);
    }
    #endregion

    #region Unit Speed
    public void ChangeSpeedImageMode(bool value)
    {
        speedImageComponent.gameObject.SetActive(value);
    }

    public void SetUnitSpeed(int speedIndex)
    {
        if (speedIndex >= speedSprites.Length)
            return;

        speedImageComponent.sprite = speedSprites[speedIndex];
    }

    #endregion
}

public enum TimelineLane
{
    Top, Center, Bottom
}
