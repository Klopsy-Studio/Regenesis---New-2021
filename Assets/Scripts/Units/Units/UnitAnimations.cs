using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    public Animator unitAnimator;

    public void SetCharacter(float value)
    {
        unitAnimator.SetFloat("Character", value);
    }
    public void SetWeapon(float value)
    {
        unitAnimator.SetFloat("Weapon", value);
    }
    public void SetAnimation(string animationToCall)
    {
        unitAnimator.SetTrigger(animationToCall);
    }
    public void SetAnimation(string animationToCall, bool state)
    {
        unitAnimator.SetBool(animationToCall, state);

    }
    public void SetIdle()
    {
        SetAnimation("idle");
    }

    public void ResetAllowDeath()
    {
        unitAnimator.SetBool("allowDeath", true);
    }
    public void SetCombatIdle()
    {
        SetAnimation("combatIdle");
    }

    public void SetNearDeath()
    {
        SetAnimation("nearDeath");
    }

    public void SetDeath()
    {
        unitAnimator.SetBool("nearDeath", false);
        unitAnimator.SetBool("death", true);

    }


    public void SetAttackHammer()
    {
        SetAnimation("attackHammer");
    }

    public void SetAttackSlingshot()
    {
        SetAnimation("attackSlingshot");
    }

    public void SetPush()
    {
        SetAnimation("push");
    }

    public void SetDamage()
    {
        SetAnimation("damage");
    }


    public void SetPrepareThrow()
    {
        SetAnimation("prepareThrow");
    }

    public void SetThrow()
    {
        SetAnimation("throw");
    }

    public void SetInject()
    {
        SetAnimation("inject");
    }
}
