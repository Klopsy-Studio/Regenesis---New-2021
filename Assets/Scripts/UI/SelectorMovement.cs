using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TabType
{
    Ability, ItemConsumable, Move, Regular, ItemAction
};
public class SelectorMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Color hoverColor;
    [SerializeField] Color normalColor;


    [SerializeField] OptionSelection optionSelection;
    [SerializeField] Text textButton;

    [SerializeField] int selection;

    public AbilityDescription abilityDescription;
    public Abilities assignedAbility;
    public Consumables assignedConsumable;
    public BattleController controller;

    public bool canBeSelected;

    List<Tile> abilityPreviewTiles = new List<Tile>();

    bool monsterSelected;

    List<SpriteRenderer> targets = new List<SpriteRenderer>();

    [SerializeField] TabType typeOfOption;

    [TextArea]
    [SerializeField] string actionDescription;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            if(abilityDescription!= null)
            {
                abilityDescription.gameObject.SetActive(true);          
            }

            switch (typeOfOption)
            {
                case TabType.Ability:
                    abilityDescription.gameObject.SetActive(true);
                    abilityDescription.AssignData(assignedAbility);
                    controller.board.SelectAbilityTiles(abilityPreviewTiles);

                    controller.currentUnit.playerUI.PreviewActionCost(assignedAbility.actionCost);

                    if (assignedAbility.abilityEquipmentType == KitType.Gunblade)
                    {
                        controller.currentUnit.playerUI.PreviewBulletCost(assignedAbility.ammoCost);
                    }

                    if (targets != null)
                    {
                        if (targets.Count > 0)
                        {
                            foreach (SpriteRenderer target in targets)
                            {
                                target.color = new Color(target.color.r, target.color.g, target.color.b, target.color.a + 0.5f);
                            }
                        }
                    }
                    break;
                case TabType.ItemConsumable:
                    abilityDescription.AssignData(assignedConsumable);
                    Debug.Log(assignedConsumable.consumableDescription);
                    break;
                case TabType.Move:
                    controller.currentUnit.playerUI.PreviewActionCost(controller.moveCost);
                    controller.board.SelectMovementTiles(abilityPreviewTiles);
                    abilityDescription.abilityDescription.text = actionDescription;

                    break;
                case TabType.Regular:
                    abilityDescription.abilityDescription.text = actionDescription;

                    break;
                case TabType.ItemAction:
                    controller.currentUnit.playerUI.PreviewActionCost(controller.itemCost);
                    abilityDescription.abilityDescription.text = actionDescription;
                    break;
                default:
                    break;
            }

            optionSelection.MouseOverEnter(this);
            optionSelection.currentSelection = selection;

            textButton.color = hoverColor;
        }
    }
  
    public void OnPointerExit(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            if(abilityDescription != null)
            {
                abilityDescription.gameObject.SetActive(false);           
            }

            switch (typeOfOption)
            {
                case TabType.Ability:

                    if (abilityPreviewTiles != null)
                    {
                        controller.board.DeSelectTiles(abilityPreviewTiles);
                    }
                    controller.currentUnit.playerUI.ShowActionPoints();

                    if (assignedAbility.abilityEquipmentType == KitType.Gunblade)
                    {
                        controller.currentUnit.playerUI.ShowBullets();
                    }

                    if (targets != null)
                    {
                        if (targets.Count > 0)
                        {
                            foreach (SpriteRenderer target in targets)
                            {
                                target.color = new Color(target.color.r, target.color.g, target.color.b, target.color.a - 0.5f);
                            }
                        }
                    }
                    break;
                case TabType.ItemConsumable:
                    abilityDescription.gameObject.SetActive(false);
                    break;
                case TabType.Move:
                    controller.currentUnit.playerUI.ShowActionPoints();
                    controller.board.DeSelectDefaultTiles(abilityPreviewTiles);
                    break;
                case TabType.ItemAction:
                    controller.currentUnit.playerUI.ShowActionPoints();

                    break;
                default:
                    break;
            }

            optionSelection.MouseOverExit(this);

            textButton.color = normalColor;
        }
    }


    public void ChangeToDefault()
    {
        textButton.color = normalColor;
    }

    public void ClearOption()
    {
        monsterSelected = false;
        if(targets != null)
        {
            targets.Clear();
        }
        if(abilityPreviewTiles != null)
        {
            controller.board.DeSelectDefaultTiles(abilityPreviewTiles);
            abilityPreviewTiles.Clear();
        }

        if(abilityDescription != null)
        {
            abilityDescription.gameObject.SetActive(false);
        }
    }
    public void GetPreviewRange()
    {
        switch (typeOfOption)
        {
            case TabType.Ability:
                foreach (RangeData r in assignedAbility.abilityRange)
                {
                    List<Tile> tiles;
                    AbilityRange range = r.GetOrCreateRange(r.range, controller.currentUnit.gameObject);
                    range.unit = controller.currentUnit;

                    tiles = range.GetTilesInRange(controller.board);

                    foreach (Tile t in tiles)
                    {
                        if (!abilityPreviewTiles.Contains(t))
                        {
                            abilityPreviewTiles.Add(t);
                        }
                    }
                }
                break;
            case TabType.ItemConsumable:
                break;
            case TabType.Move:
                Movement m = controller.currentUnit.GetComponent<Movement>();
                m.range = controller.currentUnit.weapon.range;
                abilityPreviewTiles = m.GetTilesInRange(controller.board, true);
                break;
            default:
                break;
        }
        
        
    }
    public void GetTargets()
    {
        foreach (AbilityTargetType a in assignedAbility.elementsToTarget)
        {
            switch (a)
            {
                case AbilityTargetType.Enemies:
                    foreach (Tile t in abilityPreviewTiles)
                    {
                        if (t.occupied && !monsterSelected)
                        {
                            targets.Add(controller.enemyUnits[0].unitSprite);
                            monsterSelected = true;
                        }

                        if(t.content!= null)
                        {
                            if(t.content.GetComponent<MinionUnit>() != null)
                            {
                                targets.Add(t.content.GetComponent<MinionUnit>().unitSprite);
                            }
                        }
                    }
                    break;
                case AbilityTargetType.Allies:
                    foreach (Tile t in abilityPreviewTiles)
                    {
                        if (t.content != null)
                        {
                            if (t.content.GetComponent<PlayerUnit>() != null)
                            {
                                PlayerUnit u = t.content.GetComponent<PlayerUnit>();

                                if (!u.isNearDeath && !u.isDead)
                                {
                                    targets.Add(t.content.GetComponent<PlayerUnit>().unitSprite);
                                }
                            }
                        }
                    }
                    break;
                case AbilityTargetType.Obstacles:
                    foreach (Tile t in abilityPreviewTiles)
                    {
                        if (t.content != null)
                        {
                            if (t.content.GetComponent<BearObstacleScript>() != null)
                            {
                                targets.Add(t.content.GetComponent<SpriteRenderer>());
                            }
                        }
                    }
                    break;
                case AbilityTargetType.Self:
                    foreach (Tile t in abilityPreviewTiles)
                    {
                        if (t.content != null)
                        {
                            if (t.content.GetComponent<PlayerUnit>() != null)
                            {
                                PlayerUnit u = t.content.GetComponent<PlayerUnit>();

                                if (!u.isNearDeath)
                                {
                                    targets.Add(t.content.GetComponent<PlayerUnit>().unitSprite);
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
