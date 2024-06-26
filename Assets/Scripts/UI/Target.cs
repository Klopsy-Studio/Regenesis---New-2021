using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class Target : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetAssigned;
    public Point targetPosition;
    public AbilityTargetType targetType;
    public BattleController controller;
    public TextMeshProUGUI targetDisplay;

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
            GameCursor.instance.SetHandCursor();
            AudioManager.instance.Play("Boton" + owner.controller.hoverOption);
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

                case AbilityTargetType.DroneTarget:
                    HighlightTarget();
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
        GameCursor.instance.SetRegularCursor();

        switch (targetType)
        {
            case AbilityTargetType.DroneTarget:
                ReturnTarget();
                break;
            default:
                break;
        }
    }
    public void HighlightTarget()
    {
        Unit u = targetAssigned.GetComponent<Unit>();
        u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.g, u.unitSprite.color.b, 1f);
    }

    public void ReturnTarget()
    {
        Unit u = targetAssigned.GetComponent<Unit>();
        u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.g, u.unitSprite.color.b, 0.5f);
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
                targetDisplay.text = "Rotten Seed";
                break;
            case AbilityTargetType.BigMonster:
                Unit a  = targetAssigned.GetComponent<Unit>();
                targetDisplay.text = a.unitName;
                targetPosition = a.currentPoint;
                break;
            case AbilityTargetType.DroneTarget:
                Unit p = targetAssigned.GetComponent<Unit>();
                targetDisplay.text = p.unitName;
                targetPosition = p.currentPoint;
                break;
            default:
                break;
        }
    }
}
