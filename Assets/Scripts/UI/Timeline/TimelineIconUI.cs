using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class TimelineIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI velocityText;
    public RectTransform rectTransform;
    public Image image;

    public Image icon;

    public Image upSupport;
    public Image downSupport;

    public bool mouseOver;

    public Animator iconAnimations;

    public TimelineElements element;


    public int indexChild;
   
    public GameObject stunnedIndicator;

    public int offset;
    public int originalOffset;

    public float barSize;

    public RectTransform iconTransform;

    public TimelineIconUI prevIcon;
    public TimelineIconUI nextIcon;
    public float minDistance;
    public float maxDistance;

    public void EnableStun()
    {
        stunnedIndicator.SetActive(true);
    }
    private void Start()
    {
        this.gameObject.name = element.name;
    }
    public void DisableStun()
    {
        stunnedIndicator.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    void Update()
    {
        iconTransform.anchoredPosition = new Vector2(-barSize / 2 + element.GetActionBarPosition() * barSize, offset);

        //Change the timeline type comparison for each timeline type, kai example
        //Has dicho que de esto te vas a acordar en dos meses jodete


        //UpdatePreviousIcon();
        UpdateNextIcon();
        UpdatePreviousIcon();
    }

  
    public void SetTimelineIconTextVelocity()
    {
        var timelineVelocity = (int)element.TimelineVelocity;
        Debug.Log("EL NUMERO DE LA VELOCIDAD ES " + timelineVelocity);
        velocityText.SetText(timelineVelocity.ToString());
        //Debug.Log("esta activado");
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
        Return();
    }
    public void Grow()
    {
        iconAnimations.SetBool("isGrow", true);
    }

    public void Return()
    {
        iconAnimations.SetBool("isGrow", false);
    }
}
