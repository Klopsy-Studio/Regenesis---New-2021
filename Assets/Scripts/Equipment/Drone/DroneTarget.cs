using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneTarget : MenuButton
{
    public Image targetImage;
    public GameObject targetSetCheck;
    [SerializeField] PlayerUnit currentTarget;
    public PlayerUnit user;

    [SerializeField] int setTargetCost;

    public DroneManager owner;

    public void SetTarget(PlayerUnit target)
    {
        targetImage.sprite = target.timelineIcon;
        currentTarget = target;

        //Show different sprite if unit already has the target set on this unit

        if(currentTarget == user.droneUnit)
        {
            owner.SetCurrentTarget(this);
        }
    }

    public void PreviewCost()
    {
        if(user.actionsPerTurn >= owner.droneCost)
        {
            user.playerUI.PreviewActionCost(owner.droneCost);
        }
    }

    public void HighlightTarget()
    {
        currentTarget.unitSprite.color = new Color(currentTarget.unitSprite.color.r, currentTarget.unitSprite.color.g, currentTarget.unitSprite.color.b, 1f);
    }

    public void ReturnTarget()
    {
        currentTarget.unitSprite.color = new Color(currentTarget.unitSprite.color.r, currentTarget.unitSprite.color.g, currentTarget.unitSprite.color.b, 0.5f);
    }
    public void ReturnCost()
    {
        user.playerUI.ShowActionPoints();
    }
    public void AssignTarget()
    {
        if(user.actionsPerTurn >= owner.droneCost && user.droneUnit != currentTarget)
        {
            user.SetDrone(currentTarget, setTargetCost);
            owner.controller.CheckAbilities();
            owner.SetCurrentTarget(this);
            owner.CanChangeDroneTarget();
        }
    }
}
