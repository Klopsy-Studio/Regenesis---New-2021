using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using JetBrains.Annotations;
using System.Security.Cryptography;

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


    [SerializeField] PreviewTurnOrder previewTurnOrder;

    [SerializeField] List<TimelineIconUI> topLane = new List<TimelineIconUI>();
    List<TimelineIconUI> _topLane = new List<TimelineIconUI>();
    [SerializeField] List<TimelineIconUI> midLane = new List<TimelineIconUI>();
    List<TimelineIconUI> _midLane = new List<TimelineIconUI>();

    [SerializeField] List<TimelineIconUI> bottomLane = new List<TimelineIconUI>();
    List<TimelineIconUI> _bottomLane = new List<TimelineIconUI>();


    public void CallTimelinePreviewOrder()//Unity button 
    {
        if (battleController.enablePreview)
        {
            previewTurnOrder.CalculateOrder(battleController.timelineElements);
        }
    }

    public void CallTimelinePreviewOrderOnAbilitySelect(TimelineElements element, int actionCost)
    {
        if (battleController.enablePreview)
        {
            previewTurnOrder.gameObject.SetActive(true);
            previewTurnOrder.CalculateOrderOnAbilitySelect(battleController.timelineElements, element, actionCost);
        }
        
    }

    public void CallTimelinePreviewOrderOnItemSelect(TimelineElements element, TimelineElements _bomb)
    {
        if (battleController.enablePreview)
        {
            previewTurnOrder.CalculateOrderOnItemSelect(battleController.timelineElements, element, 2, _bomb);
        }
    }

    public void ExitPreviewTurnOrder()
    {
        if (battleController.enablePreview)
        {
            previewTurnOrder.ExitChanges();
        }
    }

   
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

            foreach (TimelineIconUI icon in topLane)
            {
                AssignIcons(icon, topLane);
            }

            foreach (TimelineIconUI icon in midLane)
            {
                AssignIcons(icon, midLane);
            }

            foreach (TimelineIconUI icon in bottomLane)
            {
                AssignIcons(icon, bottomLane);
            }
        }
    }

    public void SortList()
    {
        orderedIconsInTimeline = iconsInTimeline.OrderByDescending(x => x.element.timelineFill).ToList();
        topLane = _topLane.OrderByDescending(a => a.element.timelineFill).ToList();
        midLane = _midLane.OrderByDescending(b => b.element.timelineFill).ToList();
        bottomLane = _bottomLane.OrderByDescending(c => c.element.timelineFill).ToList();

    }

    public void AssignIcons(TimelineIconUI icon, List<TimelineIconUI> iconList)
    {
        int index = iconList.IndexOf(icon);

        if(index+1 < iconList.Count)
        {
            icon.prevIcon = iconList[index + 1];
        }
        else
        {
            icon.prevIcon = null;
        }

        if(index-1 >= 0)
        {
            icon.nextIcon = iconList[index - 1];
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
                    switch (a.lane)
                    {
                        case TimelineLane.Top:
                            topLane.Remove(a);
                            break;
                        case TimelineLane.Center:
                            midLane.Remove(a);
                            break;
                        case TimelineLane.Bottom:
                            bottomLane.Remove(a);
                            break;
                        default:
                            break;
                    }
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
                temp.lane = TimelineLane.Top;
                if (!_topLane.Contains(temp))
                {
                    _topLane.Add(temp);
                }

                temp.image.sprite = playerFrame;
                temp.element.iconTimeline = temp;
                temp.icon.sprite = battleController.timelineElements[i].timelineIcon;

                temp.downSupport.GetComponent<Image>().enabled = true;

                temp.downSupport.sprite = upSupport;
                temp.offset = 70;

                //temp.velocityText.gameObject.SetActive(true);

                temp.SetTimelineIconTextVelocity();
                //var timelineVelocity = (int)temp.element.timelineVelocity;
                //temp.velocityText.SetText(timelineVelocity.ToString());
            }
            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.EnemyUnit)
            {
                temp.lane = TimelineLane.Bottom;

                if (!_bottomLane.Contains(temp))
                {
                    _bottomLane.Add(temp);
                }

                temp.element.iconTimeline = temp;

                temp.image.sprite = enemyFrame;

                temp.icon.sprite = battleController.timelineElements[i].timelineIcon;

                temp.upSupport.GetComponent<Image>().enabled = true;
                temp.upSupport.sprite = downSupport;

                temp.offset = -70;

            }
            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.RealtimeEvents)
            {
                temp.lane = TimelineLane.Center;

                if (!_midLane.Contains(temp))
                {
                    _midLane.Add(temp);
                }

                temp.element.iconTimeline = temp;

                temp.image.sprite = eventFrame;
                temp.icon.sprite = eventIcon;

                temp.offset = 0;
            }
            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.Items)
            {
                temp.lane = TimelineLane.Center;

                if (!_bottomLane.Contains(temp))
                {
                    _bottomLane.Add(temp);
                }

                temp.element.iconTimeline = temp;

                temp.image.sprite = itemFrame;
                temp.icon.sprite = itemIcon;

                temp.offset = 0;
            }

            else if (battleController.timelineElements[i].timelineTypes == TimeLineTypes.PlayerDeath)
            {
                temp.lane = TimelineLane.Center;
                if (!_midLane.Contains(temp))
                {
                    _midLane.Add(temp);
                }
                temp.element.iconTimeline = temp;
                temp.upSupport.sprite = downSupport;
                temp.offset = 0;
            }

            else if(battleController.timelineElements[i].timelineTypes == TimeLineTypes.EnemyEvent)
            {
                temp.lane = TimelineLane.Center;

                if (!_bottomLane.Contains(temp))
                {
                    _bottomLane.Add(temp);
                }

                temp.element.iconTimeline = temp;
                temp.upSupport.GetComponent<Image>().enabled = true;

                temp.image.sprite = itemFrame;
                temp.offset = 0;

                temp.upSupport.gameObject.SetActive(true);
                temp.offset = -70;

            }

            else if(battleController.timelineElements[i].timelineTypes == TimeLineTypes.HunterEvent)
            {
                temp.lane = TimelineLane.Top;

                if (!_topLane.Contains(temp))
                {
                    _topLane.Add(temp);
                }

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
        if(battleController.timelineElements.Count > 0) 
        {
            for (int i = 0; i < battleController.timelineElements.Count; i++)
            {
                if(content.childCount >= i)
                {
                    if (content.GetChild(i) != null)
                    {
                        TimelineIconUI temp = content.GetChild(i).GetComponent<TimelineIconUI>();

                        if (temp.mouseOver)
                        {
                            selectedIcon = temp;
                            return true;
                        }
                    }
                }
            }

            selectedIcon = null;
            return false;
        }

        else
        {
            return false;
        }
        
    }
    public void ShowIconActing(TimelineElements element)
    {
        currentActorFrame.enabled = true;
        currentActorIcon.enabled = true;
        element.iconTimeline.isActing = true;

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
        element.iconTimeline.isActing = false;
    }
}
