using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum StatusMode
{
    Big, Small
}
public class UnitStatus : MonoBehaviour
{
    [Header("Status Modes")]
    [SerializeField] GameObject bigUnitUI;
    [SerializeField] GameObject smallUnitUI;
    [Space]
    [Header("Unit Status References")]

    PlayerUnit currentUnit;
    [SerializeField] Animator statusModes;
    [SerializeField] Text unitName;
    [SerializeField] Slider unitHealthBig;
    [SerializeField] Slider unitHealthSmall;

    public Image unitPortrait;
    [SerializeField] Image unitWeapon;


    [Header("Titles")]
    [HideInInspector] public bool updatingValue;
    [Header("UI Animations Variables")]
    [SerializeField] float speed;

    [Header("Status Mode")]
    [SerializeField] StatusMode uiStatus;
    public void SetUnit(PlayerUnit unit)
    {
        //unitName.text = unit.unitName;
        currentUnit = unit;
        unitHealthBig.maxValue = unit.maxHealth;
        unitHealthBig.value = unit.health;

        unitPortrait.sprite = unit.timelineIcon;

        unitWeapon.sprite = unit.weapon.weaponIcon;

        unitName.text = unit.unitName;

        unit.status = this;
    }

    void UpdateSliderValue(Slider currentSlider, int value)
    {
        currentSlider.value = value;
    }


    public void HealthAnimation(int target)
    {
        //ChangeToBig();
        StartCoroutine(SliderValueAnimation(unitHealthSmall, target));
    }

    //public void SharpnessAnimation(int target)
    //{
    //    StartCoroutine(SliderValueAnimation(unitSharpness, target));
    //}

    IEnumerator SliderValueAnimation(Slider s, int targetValue)
    {
        updatingValue = true;

        if(s.value >= targetValue)
        {
            while (s.value >= targetValue)
            {
                s.value -= Time.deltaTime * speed;
                yield return null;

                if (s.value <= 0)
                {
                    break;
                }
            }
        }
        else
        {
            while (s.value <= targetValue)
            {
                s.value += Time.deltaTime * speed;
                yield return null;

                if (s.value >= s.maxValue)
                {
                    break;
                }
            }
        }
        s.value = targetValue;

        Invoke("ChangeToSmall", 1f);
        updatingValue = false;
    }
         
    public void ChangeToBig()
    {
        if(uiStatus != StatusMode.Big)
        {
            unitHealthBig.value = currentUnit.health;
            statusModes.SetTrigger("big");
            uiStatus = StatusMode.Big;
        }
    }

    public void ChangeToSmall()
    {
        if (uiStatus != StatusMode.Small)
        {
            unitHealthSmall.value = currentUnit.health;
            statusModes.SetTrigger("small");
            uiStatus = StatusMode.Small;
        }
    }

}
