using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniStatus : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] BattleController controller;
    [Space(3)]
    [Header("Player Status References")]
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] Image playerWeapon;
    [SerializeField] GameObject playerStatus;
    [SerializeField] GameObject playerBuffsAndDebuffs;
    [SerializeField] Image playerPortrait;
    [SerializeField] Slider playerHealth;
    [Space(2)]
    [Header("Monster Status References")]

    [SerializeField] GameObject monsterStatus;
    [SerializeField] GameObject monsterBuffsAndDebuffs;
    [SerializeField] Image monsterPortrait;
    [SerializeField] TextMeshProUGUI monsterName;

    [Space(2)]
    [Header("Any event Status")]
    [SerializeField] GameObject anyEventStatus;
    [SerializeField] TextMeshProUGUI eventDescription;
    [SerializeField] Image eventIcon;




    [Header("Buffs and Debuffs Prefabs")]
    [SerializeField] GameObject iconPrefab;

    [SerializeField] GameObject defenseBuff;
    [SerializeField] GameObject defenseDebuff;
    [SerializeField] GameObject slowedIcon;
    [SerializeField] GameObject hastedIcon;
    [SerializeField] GameObject damageBuff;
    [SerializeField] GameObject damageDebuff;
    [SerializeField] GameObject spiderMarkIcon;
    [SerializeField] GameObject hunterMarkIcon;
    [SerializeField] GameObject bullseyeIcon;
    [SerializeField] GameObject stunnedIcon;
    [SerializeField] GameObject piercingIcon;


    [Space]
    [Header("Weapon sprites")]
    [SerializeField] Sprite hammerSprite;
    [SerializeField] Sprite bowSprite;
    [SerializeField] Sprite gunbladeSprite;
    [SerializeField] Sprite droneSprite;

    GameObject currentStatus;


    public void DeactivateStatus()
    {
        if (controller.enableMiniStatus)
        {
            parent.SetActive(false);
            ResetChilds(playerBuffsAndDebuffs);
            ResetChilds(monsterBuffsAndDebuffs);
            currentStatus.SetActive(false);
            currentStatus = null;
        } 
    }
    public void SetStatus(PlayerUnit element)
    {
        if (controller.enableMiniStatus)
        {
            if (currentStatus != null)
            {
                if (currentStatus != playerStatus)
                {
                    DeactivateStatus();
                }
            }
            parent.SetActive(true);
            playerStatus.SetActive(true);
            currentStatus = playerStatus;

            playerPortrait.sprite = element.unitPortrait;
            playerName.SetText(element.unitName);
            ResetChilds(playerBuffsAndDebuffs);
            SetAlteredEffects(element, playerBuffsAndDebuffs.transform);

            switch (element.weapon.EquipmentType)
            {
                case KitType.Hammer:
                    playerWeapon.sprite = hammerSprite;
                    break;
                case KitType.Bow:
                    playerWeapon.sprite = bowSprite;
                    break;
                case KitType.Gunblade:
                    playerWeapon.sprite = gunbladeSprite;
                    break;
                case KitType.Drone:
                    playerWeapon.sprite = droneSprite;
                    break;
                default:
                    break;
            }
        }
        
    }
    public void SetStatus(EnemyUnit element)
    {
        if (controller.enableMiniStatus)
        {
            Debug.Log("Setting Monster");

            if (currentStatus != null)
            {
                if (currentStatus != monsterStatus)
                {
                    DeactivateStatus();
                }
            }

            ResetChilds(monsterBuffsAndDebuffs);

            parent.SetActive(true);

            monsterName.SetText(element.unitName);
            monsterStatus.SetActive(true);
            currentStatus = monsterStatus;

            monsterPortrait.sprite = element.unitPortrait;

            SetAlteredEffects(element, monsterBuffsAndDebuffs.transform);

        }

    }
    public void SetStatus(TimelineElements element)
    {
        if (controller.enableMiniStatus)
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
        

    }

    public void SpawnIcon(GameObject o, Transform parent)
    {
        Instantiate(o, parent);
    }


    public void SetAlteredEffects(Unit target, Transform targetParent)
    {
        if (target.debuffModifiers.Count > 0)
        {
            foreach (Modifier m in target.debuffModifiers)
            {
                switch (m.modifierType)
                {
                    case TypeOfModifier.HunterMark:
                        SpawnIcon(hunterMarkIcon, targetParent);
                        break;
                    case TypeOfModifier.Defense:
                        SpawnIcon(defenseDebuff, targetParent);
                        break;
                    case TypeOfModifier.TimelineSpeed:
                        SpawnIcon(slowedIcon, targetParent);

                        break;
                    case TypeOfModifier.Damage:
                        SpawnIcon(damageDebuff, targetParent);
                        break;
                    case TypeOfModifier.Stun:
                        SpawnIcon(stunnedIcon, targetParent);
                        break;
                    case TypeOfModifier.SpiderMark:
                        SpawnIcon(spiderMarkIcon, targetParent);
                        break;
                    default:
                        break;
                }
            }
        }

        if (target.buffModifiers.Count > 0)
        {
            foreach (Modifier m in target.buffModifiers)
            {
                switch (m.modifierType)
                {
                    case TypeOfModifier.HunterMark:
                        SpawnIcon(hunterMarkIcon, targetParent);
                        break;
                    case TypeOfModifier.Defense:
                        SpawnIcon(defenseBuff, targetParent);
                        break;
                    case TypeOfModifier.TimelineSpeed:
                        SpawnIcon(hastedIcon, targetParent);

                        break;
                    case TypeOfModifier.Damage:
                        SpawnIcon(damageBuff, targetParent);
                        break;
                    case TypeOfModifier.Stun:
                        SpawnIcon(stunnedIcon, targetParent);
                        break;
                    case TypeOfModifier.SpiderMark:
                        SpawnIcon(spiderMarkIcon, targetParent);
                        break;
                    case TypeOfModifier.Bullseye:
                        SpawnIcon(bullseyeIcon, targetParent);
                        break;
                    case TypeOfModifier.PiercingSharpness:
                        SpawnIcon(piercingIcon, targetParent);
                        break;
                    case TypeOfModifier.Antivirus:
                        SpawnIcon(defenseBuff, targetParent);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public void ResetChilds(GameObject item)
    {
        if(item.transform.childCount > 0)
        {
            Transform parent = item.transform;

            foreach (Transform icon in parent)
            {
                icon.gameObject.SetActive(false);
            }
            item.transform.DetachChildren();
        }
        
    }

}
