using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    [SerializeField] Animator unitAnimator;

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
        SetAnimation("death");
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
