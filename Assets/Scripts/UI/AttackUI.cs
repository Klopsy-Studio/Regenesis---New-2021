using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackUI : MonoBehaviour
{
    [SerializeField] GameObject attackElements;
    [SerializeField] Image unit1;
    [SerializeField] Image unit2;

    Animation unit1Animations;
    Animation unit2Animations;

    public bool duringAttack;
    public void SetUpAttack(Sprite attackingUnit, Sprite damagingUnit, AnimationClip attackAnimation, AnimationClip hurtAnimation)
    {
        attackElements.SetActive(true);

        //unit1.sprite = attackingUnit;
        //unit2.sprite = damagingUnit;

        //unit1Animations.clip = attackAnimation;
        //unit2Animations.clip = hurtAnimation;
    }

    public void ResetAttack()
    {
        attackElements.SetActive(false);
    }
    public IEnumerator Sequence()
    {
        //duringAttack = true;
        //unit1Animations.Play();

        //yield return new WaitForSeconds(unit1Animations.clip.averageDuration);

        //unit2Animations.Play();

        //yield return new WaitForSeconds(unit2Animations.clip.averageDuration);

        //ResetAttack();
        //duringAttack = false;      

        duringAttack = true;

        yield return new WaitForSeconds(2);
    }
}
