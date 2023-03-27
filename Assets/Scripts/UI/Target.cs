using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Target : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetAssigned;
    public Point targetPosition;
    public AbilityTargetType targetType;
    public BattleController controller;
    public Text targetDisplay;

    public AbilityTargets owner;

    public Color selectedColor;
    public Color defaultColor;

    void Start()
    {
        defaultColor = targetDisplay.color;
    }

    private void OnEnable()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!owner.stopSelection)
        {
            owner.controller.ActivateTileSelector();    
            owner.selectedTarget = this;
            controller.SelectTile(targetPosition);
            controller.UpdateUnitSprite();

            switch (targetType)
            {
                case AbilityTargetType.BigMonster:
                    controller.tileSelectionToggle.MakeTileSelectionBig();
                    owner.indicator.SetTarget(targetPosition, 3);
                    break;
                case AbilityTargetType.Enemies:
                    controller.tileSelectionToggle.MakeTileSelectionSmall();
                    owner.indicator.SetTarget(targetPosition, 2.5f);

                    break;
                case AbilityTargetType.Allies:
                    controller.tileSelectionToggle.MakeTileSelectionSmall();
                    owner.indicator.SetTarget(targetPosition, 2.5f);

                    break;
                case AbilityTargetType.Obstacles:
                    controller.tileSelectionToggle.MakeTileSelectionSmall();
                    owner.indicator.SetTarget(targetPosition, 2.5f);

                    break;
                default:
                    break;
            }

            targetDisplay.color = selectedColor;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MoveCursorAway();
    }

    public void MoveCursorAway()
    {
        if (!owner.stopSelection)
        {
            owner.indicator.DeactivateTarget();
            owner.selectedTarget = null;
            targetDisplay.color = defaultColor;
            controller.tileSelectionToggle.MakeTileSelectionSmall();
            owner.controller.DeactivateTileSelector();

            controller.SelectTile(controller.currentUnit.tile.pos);
        }
    }
    public void SetTarget()
    {
        switch (targetType)
        {
            case AbilityTargetType.Enemies:
                Unit u = targetAssigned.GetComponent<Unit>();
                targetDisplay.text = u.unitName;
                targetPosition = u.currentPoint;
                break;
            case AbilityTargetType.Allies:
                Unit e = targetAssigned.GetComponent<Unit>();
                targetDisplay.text = e.unitName;
                targetPosition = e.currentPoint;
                break;
            case AbilityTargetType.Obstacles:
                targetPosition = targetAssigned.GetComponent<BearObstacleScript>().pos;
                targetDisplay.text = "Monster Obstacle";
                break;
            case AbilityTargetType.BigMonster:
                Unit a  = targetAssigned.GetComponent<Unit>();
                targetDisplay.text = a.unitName;
                targetPosition = a.currentPoint;
                break;
            default:
                break;
        }
    }
}
