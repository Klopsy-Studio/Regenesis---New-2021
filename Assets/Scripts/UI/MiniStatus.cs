using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniStatus : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [Space(3)]
    [Header("Player Status References")]
    [SerializeField] GameObject playerStatus;
    [SerializeField] GameObject playerBuffsAndDebuffs;
    [SerializeField] TextMeshProUGUI playerPower;
    [SerializeField] TextMeshProUGUI playerCrit;
    [SerializeField] TextMeshProUGUI playerElement;
    [SerializeField] TextMeshProUGUI playerDef;
    [SerializeField] TextMeshProUGUI playerRes;
    [SerializeField] TextMeshProUGUI playerDeb;
    [SerializeField] Image playerPortrait;
    [Space(2)]
    [Header("Monster Status References")]

    [SerializeField] GameObject monsterStatus;
    [SerializeField] GameObject monsterBuffsAndDebuffs;
    [SerializeField] Image monsterPortrait;

    [Space(2)]
    [Header("Any event Status")]
    [SerializeField] GameObject anyEventStatus;
    [SerializeField] TextMeshProUGUI eventDescription;
    [SerializeField] Image eventIcon;




    [Header("Buffs and Debuffs Prefabs")]
    [SerializeField] GameObject battlecryIcon;
    [SerializeField] GameObject hunterMarkIcon;
    [SerializeField] GameObject damageMarkIcon;

    [SerializeField] GameObject slowedIcon;
    [SerializeField] GameObject stunnedIcon;


    GameObject currentStatus;


    public void DeactivateStatus()
    {
        parent.SetActive(false);
        ResetChilds(playerBuffsAndDebuffs);
        ResetChilds(monsterBuffsAndDebuffs);
        currentStatus.SetActive(false);
        currentStatus = null;
    }
    public void SetStatus(PlayerUnit element)
    {
        Debug.Log("Setting Unit");
        if(currentStatus != null)
        {
            if (currentStatus != playerStatus)
            {
                DeactivateStatus();

            }
        }
        parent.SetActive(true);

        playerStatus.SetActive(true);
        currentStatus = playerStatus;

        playerPortrait.sprite = element.timelineIcon;
        playerPower.SetText(element.power.ToString());
        playerCrit.SetText(element.criticalPercentage.ToString());
        playerElement.SetText(element.elementPower.ToString());
        playerDef.SetText(element.defense.ToString());
        playerRes.SetText(element.defenseElement.ToString());
        playerDeb.SetText("???");


        if (element.debuffModifiers.Count > 0)
        {
            foreach(Modifier m in element.debuffModifiers)
            {
                switch (m.modifierType)
                {
                    case TypeOfModifier.Critical:
                        Instantiate(hunterMarkIcon, playerBuffsAndDebuffs.transform);
                        break;
                    case TypeOfModifier.Defense:
                        Instantiate(battlecryIcon, playerBuffsAndDebuffs.transform);

                        break;
                    case TypeOfModifier.TimelineSpeed:
                        Instantiate(slowedIcon, playerBuffsAndDebuffs.transform);

                        break;
                    case TypeOfModifier.Damage:
                        Instantiate(damageMarkIcon, playerBuffsAndDebuffs.transform);
                        break;
                    default:
                        break;
                }
            }
        }

        if (element.buffModifiers.Count > 0)
        {
            foreach (Modifier m in element.buffModifiers)
            {
                switch (m.modifierType)
                {
                    case TypeOfModifier.Critical:
                        Instantiate(hunterMarkIcon, playerBuffsAndDebuffs.transform);
                        break;
                    case TypeOfModifier.Defense:
                        Instantiate(battlecryIcon, playerBuffsAndDebuffs.transform);

                        break;
                    case TypeOfModifier.TimelineSpeed:
                        Instantiate(slowedIcon, playerBuffsAndDebuffs.transform);

                        break;
                    case TypeOfModifier.Damage:
                        Instantiate(damageMarkIcon, playerBuffsAndDebuffs.transform);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public void SetStatus(EnemyUnit element)
    {
        Debug.Log("Setting Monster");

        if (currentStatus != null)
        {
            if (currentStatus != monsterStatus)
            {
                DeactivateStatus();
            }
        }
        parent.SetActive(true);
        
        monsterStatus.SetActive(true);
        currentStatus = monsterStatus;

        monsterPortrait.sprite = element.timelineIcon;

        if (element.debuffModifiers.Count > 0)
        {
            foreach (Modifier m in element.debuffModifiers)
            {
                switch (m.modifierType)
                {
                    case TypeOfModifier.Critical:
                        Instantiate(hunterMarkIcon, monsterBuffsAndDebuffs.transform);
                        break;
                    case TypeOfModifier.Defense:
                        Instantiate(battlecryIcon, monsterBuffsAndDebuffs.transform);

                        break;
                    case TypeOfModifier.TimelineSpeed:
                        Instantiate(slowedIcon, monsterBuffsAndDebuffs.transform);
                        break;
                    case TypeOfModifier.Damage:
                        Instantiate(damageMarkIcon, monsterBuffsAndDebuffs.transform);
                        break;
                    default:
                        break;
                }
            }
        }

        if (element.buffModifiers.Count > 0)
        {
            foreach (Modifier m in element.buffModifiers)
            {
                switch (m.modifierType)
                {
                    case TypeOfModifier.Critical:
                        Instantiate(hunterMarkIcon, monsterBuffsAndDebuffs.transform);
                        break;
                    case TypeOfModifier.Defense:
                        Instantiate(battlecryIcon, monsterBuffsAndDebuffs.transform);

                        break;
                    case TypeOfModifier.TimelineSpeed:
                        Instantiate(slowedIcon, monsterBuffsAndDebuffs.transform);

                        break;
                    case TypeOfModifier.Damage:
                        Instantiate(damageMarkIcon, monsterBuffsAndDebuffs.transform);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public void SetStatus(TimelineElements element)
    {
        Debug.Log("Setting Event");

        if (currentStatus != null)
        {
            if (currentStatus != anyEventStatus)
            {
                DeactivateStatus();
            }
        }
        parent.SetActive(true);

        anyEventStatus.SetActive(true);
        currentStatus = anyEventStatus;

        eventIcon.sprite = element.timelineIcon;
        eventDescription.SetText(element.eventDescription);

    }
    public void ResetChilds(GameObject item)
    {
        if(item.transform.childCount > 0)
        {
            foreach (GameObject icon in item.transform)
            {
                icon.SetActive(false);
            }
            item.transform.DetachChildren();
        }
        
    }

}
