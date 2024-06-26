using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PopUpText : MonoBehaviour
{
    [Header("Damage References")]
    private TextMeshPro textMesh;
    private Animator textAnim;


    [Space]
    [Header("Buff Debuff References")]
    [SerializeField] SpriteRenderer statusIcon;
    [SerializeField] StatusIconsReferences references;
    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textAnim = GetComponent<Animator>();
    }
  
    public void SetUp(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textAnim.SetTrigger("dmg");
    }

    public void SetUpCritical(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textAnim.SetTrigger("crit");
        AudioManager.instance.Play("CriticalDamage");
    }

    public void SetUpDebuff(TypeOfModifier modifier)
    {
        textAnim.SetTrigger("buff");

        switch (modifier)
        {
            case TypeOfModifier.HunterMark:
                statusIcon.sprite = references.hunterMarkIcon;
                break;
            case TypeOfModifier.Defense:
                statusIcon.sprite = references.defenseDebuff;
                break;
            case TypeOfModifier.TimelineSpeed:
                statusIcon.sprite = references.slowedIcon;

                break;
            case TypeOfModifier.Damage:
                statusIcon.sprite = references.damageDebuff;
                break;
            case TypeOfModifier.Stun:
                statusIcon.sprite = references.stunnedIcon;
                break;
            case TypeOfModifier.SpiderMark:
                statusIcon.sprite = references.spiderMarkIcon;
                break;
            case TypeOfModifier.NearDeath:
                statusIcon.sprite = references.nearDeathIcon;
                break;
            default:
                break;
        }
    }


    public void SetUpBuff(TypeOfModifier modifier)
    {
        textAnim.SetTrigger("buff");

        switch (modifier)
        {
            case TypeOfModifier.Defense:
                statusIcon.sprite = references.defenseBuff;
                break;
            case TypeOfModifier.TimelineSpeed:
                statusIcon.sprite = references.hastedIcon;

                break;
            case TypeOfModifier.Damage:
                statusIcon.sprite = references.damageBuff;
                break;
            case TypeOfModifier.Bullseye:
                statusIcon.sprite = references.bullseyeIcon;
                break;
            case TypeOfModifier.PiercingSharpness:
                statusIcon.sprite = references.damageBuff;
                break;
            case TypeOfModifier.Antivirus:
                statusIcon.sprite = references.antivirusIcon;
                break;
            case TypeOfModifier.DroneUnit:
                statusIcon.sprite = references.droneIcon;
                break;
            case TypeOfModifier.SpikyArmor:
                statusIcon.sprite = references.spikyArmorIcon;
                break;
            case TypeOfModifier.MinionEvolve:
                statusIcon.sprite = references.evolveIcon;
                break;
            case TypeOfModifier.MinionFrenzy:
                statusIcon.sprite = references.frenzyIcon;
                break;
            default:
                break;
        }
    }

    public void SetUpRemoveDebuff(TypeOfModifier modifier)
    {
        textAnim.SetTrigger("debuff");

        switch (modifier)
        {
            case TypeOfModifier.HunterMark:
                statusIcon.sprite = references.hunterMarkIcon;
                break;
            case TypeOfModifier.Defense:
                statusIcon.sprite = references.defenseDebuff;
                break;
            case TypeOfModifier.TimelineSpeed:
                statusIcon.sprite = references.slowedIcon;

                break;
            case TypeOfModifier.Damage:
                statusIcon.sprite = references.damageDebuff;
                break;
            case TypeOfModifier.Stun:
                statusIcon.sprite = references.stunnedIcon;
                break;
            case TypeOfModifier.SpiderMark:
                statusIcon.sprite = references.spiderMarkIcon;
                break;
            default:
                break;
        }
    }

    public void SetUpRemoveBuff(TypeOfModifier modifier)
    {
        textAnim.SetTrigger("debuff");

        switch (modifier)
        {
            case TypeOfModifier.Defense:
                statusIcon.sprite = references.defenseBuff;
                break;
            case TypeOfModifier.TimelineSpeed:
                statusIcon.sprite = references.hastedIcon;

                break;
            case TypeOfModifier.Damage:
                statusIcon.sprite = references.damageBuff;
                break;
            case TypeOfModifier.Bullseye:
                statusIcon.sprite = references.bullseyeIcon;
                break;
            case TypeOfModifier.PiercingSharpness:
                statusIcon.sprite = references.damageBuff;
                break;
            case TypeOfModifier.Antivirus:
                statusIcon.sprite = references.defenseBuff;
                break;
            case TypeOfModifier.DroneUnit:
                statusIcon.sprite = references.droneIcon;
                break;
            case TypeOfModifier.SpikyArmor:
                statusIcon.sprite = references.spikyArmorIcon;
                break;
            default:
                break;
        }
    }
    public void DestroyPopUp()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
}
