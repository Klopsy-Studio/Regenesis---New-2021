using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TimelineUI : MonoBehaviour
{
    [SerializeField] BattleController battleController;
    [SerializeField] GameObject iconPrefab;
    [SerializeField] RectTransform content;
    [SerializeField] RectTransform removedChildren;
    float barSize;


    [Header("Icon prefabs")]
    [SerializeField] Sprite playerFrame;
    [SerializeField] Sprite enemyFrame;
    [SerializeField] Sprite eventFrame;

    [SerializeField] Sprite eventIcon;
    [SerializeField] Sprite itemFrame;
    [SerializeField] Sprite itemIcon;

    [SerializeField] Sprite upSupport;
    [SerializeField] Sprite downSupport;

    [SerializeField] Sprite eventSupport;

    public int offset;


    public TimelineIconUI selectedIcon;


    public Image currentActorFrame;
    public Image currentActorIcon;

    List<TimelineIconUI> iconsInTimeline = new List<TimelineIconUI>();
    public List<TimelineIconUI> orderedIconsInTimeline;

    public bool isActive;
    //The bar size. Dependant on size delta. Only works for a static scale object as delta isn't mesured the same way with different anchors.
    private void Start()
    {
        barSize = content.sizeDelta.x;
    }

    //Not Ideal. Would be better to avoid GetComponent entirely. Simplest solution for a 45 minutes project
    private void Update()
    {
       
        if (isActive) 
        {
            BalanceAmountOf(iconPrefab, content, battleController.timelineElements.Count);
            SortList();

            foreach (TimelineIconUI icon in iconsInTimeline)
            {
                AssignIcons(icon);
            }
        }
    }

    public void SortList()
    {
        orderedIconsInTimeline = iconsInTimeline.OrderByDescending(x => x.element.timelineFill).ToList();
    }

    public void AssignIcons(TimelineIconUI icon)
    {
        int index = orderedIconsInTimeline.IndexOf(icon);

        if(index+1 < orderedIconsInTimeline.Count)
        {
            icon.prevIcon = orderedIconsInTimeline[index + 1];
        }
        else
        {
            icon.prevIcon = null;
        }

        if(index-1 >= 0)
        {
            icon.nextIcon = orderedIconsInTimeline[index - 1];
        }
        else
        {
            icon.nextIcon = null;
        }

    }

    //Avoid creating or destroying more than necessary
    private bool BalanceAmountOf(GameObject prefab, Transform content, int amount)
    {
        if (content.childCount > amount)
        {
            for (int i = 0; i < content.childCount; i++)
            {
                var a = content.GetChild(i).GetComponent<TimelineIconUI>();

                if (!a.element.elementEnabled)
                {
                    a.transform.parent = removedChildren;
                    a.gameObject.SetActive(false);
                    iconsInTimeline.Remove(a);
                }
            }

            return true;
        }

        if (content.childCount < amount)
        {
            int amountToAdd = amount - content.childCount;
            for (int i = 0; i < amountToAdd; i++)
            {
                TimelineIconUI test = Instantiate(prefab, content).GetComponent<TimelineIconUI>();
                iconsInTimeline.Add(test);
            }
            SetIcons();
            return true;
        }

        return false;
    }

    public void SetIcons()
    {
        TimelineIconUI temp;
        for (int i = 0; i < battleController.timelineElements.Count; i++)
        {
            temp = iconsInTimeline[i];
            temp.element = battleController.timelineElements[i];

            if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.PlayerUnit)
            {
               
                temp.image.sprite = playerFrame;
                temp.element.iconTimeline = temp;
                temp.icon.sprite = battleController.timelineElements[i].timelineIcon;

                temp.downSupport.GetComponent<Image>().enabled = true;

                temp.downSupport.sprite = upSupport;
                temp.offset = 70;

                temp.velocityText.gameObject.SetActive(true);

                temp.SetTimelineIconTextVelocity();
                //var timelineVelocity = (int)temp.element.timelineVelocity;
                //temp.velocityText.SetText(timelineVelocity.ToString());
            }
            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.EnemyUnit)
            {
                temp.element.iconTimeline = temp;

                temp.image.sprite = enemyFrame;

                temp.icon.sprite = battleController.timelineElements[i].timelineIcon;

                temp.upSupport.GetComponent<Image>().enabled = true;
                temp.upSupport.sprite = downSupport;

                temp.offset = -70;

            }
            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.RealtimeEvents)
            {
                temp.element.iconTimeline = temp;

                temp.image.sprite = eventFrame;
                temp.icon.sprite = eventIcon;

                temp.offset = 0;
            }
            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.Items)
            {
                temp.element.iconTimeline = temp;

                temp.image.sprite = itemFrame;
                temp.icon.sprite = itemIcon;

                temp.offset = 0;
            }

            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.PlayerDeath)
            {
                temp.element.iconTimeline = temp;
                temp.upSupport.sprite = downSupport;
                temp.offset = 0;
            }

            else if(battleController.timelineElements[i].timelineTypes == TimeLineTypes.EnemyEvent)
            {
                temp.element.iconTimeline = temp;
                temp.upSupport.GetComponent<Image>().enabled = true;

                temp.image.sprite = itemFrame;
                temp.offset = 0;

                temp.upSupport.gameObject.SetActive(true);
                temp.offset = -70;

            }

            else if(battleController.timelineElements[i].timelineTypes == TimeLineTypes.HunterEvent)
            {
                temp.image.sprite = playerFrame;
                temp.element.iconTimeline = temp;
                temp.icon.sprite = battleController.timelineElements[i].timelineIcon;

                temp.downSupport.GetComponent<Image>().enabled = true;
                temp.downSupport.sprite = upSupport;
                temp.offset = 70;
            }
            temp.barSize = content.sizeDelta.x;
            temp.originalOffset = temp.offset;
            temp.icon.sprite = battleController.timelineElements[i].timelineIcon;
            temp.image.SetNativeSize();

        }
    }
    public bool CheckMouse()
    {
        for (int i = 0; i < battleController.timelineElements.Count; i++)
        {
            TimelineIconUI temp = content.GetChild(i).GetComponent<TimelineIconUI>();

            if (temp.mouseOver)
            {
                selectedIcon = temp;
                return true;
            }
        }

        selectedIcon = null;
        return false;
    }
    public void ShowIconActing(TimelineElements element)
    {
        currentActorFrame.enabled = true;
        currentActorIcon.enabled = true;

        if(element.iconTimeline.prevIcon != null)
        {
            element.iconTimeline.ReturnIcon(element.iconTimeline.prevIcon);
        }
        switch (element.timelineTypes)
        {
            case TimeLineTypes.Null:
                break;
            case TimeLineTypes.PlayerUnit:
                currentActorFrame.sprite = playerFrame;
                break;
            case TimeLineTypes.EnemyUnit:
                currentActorFrame.sprite = enemyFrame;
                break;
            case TimeLineTypes.RealtimeEvents:
                currentActorFrame.sprite = eventFrame;
                break;
            case TimeLineTypes.Items:
                currentActorFrame.sprite = eventFrame;
                break;
            case TimeLineTypes.PlayerDeath:
                currentActorFrame.sprite = itemFrame;
                break;
            default:
                break;
        }

        currentActorIcon.sprite = element.timelineIcon;
    }

    public void HideIconActing()
    {
        currentActorFrame.enabled = false;
        currentActorIcon.enabled = false;
    }
    public void HideTimelineIcon(TimelineElements element)
    {
        element.iconTimeline.gameObject.SetActive(false);
    }

    public void ShowTimelineIcon(TimelineElements element)
    {
        element.iconTimeline.gameObject.SetActive(true);
    }
}
