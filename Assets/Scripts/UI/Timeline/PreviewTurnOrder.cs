using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PreviewTurnOrder : MonoBehaviour
{
    List<TimelineElements> previewTimelineList = new List<TimelineElements>();

    [SerializeField] GameObject iconPrefab;
    [SerializeField] Transform contentTransform;
    List<GameObject> iconsList = new List<GameObject>();
    [SerializeField] int numberOfTotalIcons;

    private void Start()
    {
        for (int i = 0; i < numberOfTotalIcons; i++)
        {
            var obj = Instantiate(iconPrefab, Vector3.zero, Quaternion.identity, contentTransform);
            iconsList.Add(obj);
            obj.gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
    public void CalculateOrder(List<TimelineElements> _timelineElementsList)
    {
        previewTimelineList.Clear();

        //Reset previewTime just in case
        foreach (var elements in _timelineElementsList)
        {
            elements.previewTime = 0;
        }

        //Set previewTime
        foreach (var elements in _timelineElementsList)
        {
            elements.previewTime = UniformLinearMotion_Time(elements);
            previewTimelineList.Add(elements);
        }

        //sortedPreviewList = previewTimelineList.OrderBy(o => o.previewTime).ToList();
        CompareTime comparer = new CompareTime();
        previewTimelineList.Sort(comparer);

        //for (int i = 0; i < previewTimelineList.Count; i++)
        //{
        //    Debug.Log("orden es " + i + " " + previewTimelineList[i].name);
        //}

        ShowIconOrder();
       
    }

    public void CalculateOrderOnAbilitySelect(List<TimelineElements> _timelineElementsList, TimelineElements _element, int actionCost)
    {
        previewTimelineList.Clear();

        //Reset previewTime just in case
        foreach (var elements in _timelineElementsList)
        {
            elements.previewTime = 0;
        }

        //Set previewTime
        foreach (var element in _timelineElementsList)
        {
            if (element == _element)
            {
                int previewUnitActionsPerTurn = element.actionsPerTurn - actionCost;
                float previewVelocity = SetPreviewVelocity(previewUnitActionsPerTurn);
                element.previewTime = UniformLineaMotion_Time_UnitAction(previewVelocity);
                
            }
            else
            {
                element.previewTime = UniformLinearMotion_Time(element);
               
            }
            previewTimelineList.Add(element);

        }

        CompareTime comparer = new CompareTime();
        previewTimelineList.Sort(comparer);

        for (int i = 0; i < previewTimelineList.Count; i++)
        {
            Debug.Log("orden es " + i + " " + previewTimelineList[i].name);
        }



        ShowIconOrder(_element);
    }

    public void CalculateOrderOnItemSelect(List<TimelineElements> _timelineElementsList, TimelineElements _elementUnit, int actionCost, TimelineElements _elementItem)
    {
        Debug.Log("esta llamando");
        foreach (var icon in iconsList)
        {
            var iconImage = icon.GetComponent<Image>();
            iconImage.color = Color.white;
            icon.gameObject.SetActive(false);
        }
        previewTimelineList.Clear();

        //Reset previewTime just in case
        foreach (var elements in _timelineElementsList)
        {
            elements.previewTime = 0;
        }

        //Set previewTime
        foreach (var element in _timelineElementsList)
        {
            if (element == _elementUnit)
            {
                int previewUnitActionsPerTurn = element.actionsPerTurn - actionCost;
                float previewVelocity = SetPreviewVelocity(previewUnitActionsPerTurn);
                element.previewTime = UniformLineaMotion_Time_UnitAction(previewVelocity);

            }
            else if(element == _elementItem)
            {
                Debug.Log("AAAAAAA");
                _elementItem.previewTime = UniformLineaMotion_Time_UnitAction(_elementItem.fTimelineVelocity);

            }
            else
            {
                element.previewTime = UniformLinearMotion_Time(element);

            }
            previewTimelineList.Add(element);

        }

        _elementItem.previewTime = UniformLineaMotion_Time_UnitAction(30);
        previewTimelineList.Add(_elementItem);
        CompareTime comparer = new CompareTime();
        previewTimelineList.Sort(comparer);

        //for (int i = 0; i < previewTimelineList.Count; i++)
        //{
        //    Debug.Log("orden es " + i + " " + previewTimelineList[i].name);
        //}

        //previewTimelineList.Add(_elementItem);
        Debug.Log("lista cantidad: " + previewTimelineList.Count);
        //ShowIconOrder(_elementUnit);

        ShowIconOrder(_elementUnit, _elementItem);
      
    }


    public void ShowIconOrder()
    {
        for (int i = 0; i < previewTimelineList.Count; i++)
        {
            iconsList[i].gameObject.SetActive(true);
            iconsList[i].GetComponentInChildren<TextMeshProUGUI>().SetText(previewTimelineList[i].name);

        }
    }

    public void ShowIconOrder(TimelineElements element, TimelineElements item = null)
    {
        for (int i = 0; i < previewTimelineList.Count; i++)
        {
            iconsList[i].gameObject.SetActive(true);
            iconsList[i].GetComponentInChildren<TextMeshProUGUI>().SetText(previewTimelineList[i].name);
            if(previewTimelineList[i] == element)
            {
                var iconImage = iconsList[i].GetComponent<Image>();
                iconImage.color = Color.red;
                //iconImage.color = new Color(iconImage.color.r, iconImage.color.g, iconImage.color.b, 0.5f);
            }
            else if (previewTimelineList[i] == item)
            {
                Debug.Log("entra a show icon order");
                var iconImage = iconsList[i].GetComponent<Image>();
                iconImage.color = Color.blue;
            }

        }
    }


    float UniformLinearMotion_Time(TimelineElements elements)
    {
        float finalPos = 100;
        float initPos = elements.timelineFill;
        float velocity = elements.fTimelineVelocity;

        float time = (finalPos - initPos) / velocity;

        return time;
    }

    float UniformLineaMotion_Time_UnitAction(float _velocity)
    {


        float finalPos = 100;
        float initPos = 0;
        float velocity = _velocity;

        float time = (finalPos - initPos) / velocity;

        return time;
    }

    public float SetPreviewVelocity(int previewActions)
    {
        float previewVelocity =0;
        //SetTimelineVelocityText();
        switch (previewActions)
        {
            case 0:
                previewVelocity = 9;
                break;
            case 1:
                previewVelocity = 12;
                break;
            case 2:
                previewVelocity = 15;
                break;
            case 3:
                previewVelocity = 18;
                break;
            case 4:
                previewVelocity = 21;
                break;
            case 5:
                previewVelocity = 24;
                break;
            default:
                break;
        }

        return previewVelocity;
    }

    public void ExitChanges()
    {
        foreach (var icon in iconsList)
        {
            var iconImage = icon.GetComponent<Image>();
            iconImage.color = Color.white;
        }
        this.gameObject.SetActive(false);
    }

}



// C# program to demonstrate the concept of 
// List<T>.Sort(IComparer <T>) method
//https://www.geeksforgeeks.org/how-to-sort-list-in-c-sharp-set-1/#m2




class CompareTime : IComparer<TimelineElements>
{
    public int Compare(TimelineElements x, TimelineElements y)
    {
        return x.previewTime.CompareTo(y.previewTime);
    }
}

