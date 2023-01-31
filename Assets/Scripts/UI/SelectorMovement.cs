using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TabType
{
    Ability, Item, Action
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
    public BattleController controller;

    public bool canBeSelected;

    List<Tile> abilityPreviewTiles = new List<Tile>();

    bool monsterSelected;

    List<SpriteRenderer> targets = new List<SpriteRenderer>();

    [SerializeField] TabType typeOfOption;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canBeSelected)
        {
            if(abilityDescription!= null)
            {
                abilityDescription.gameObject.SetActive(true);

                switch (typeOfOption)
                {
                    case TabType.Ability:
                        controller.board.SelectAbilityTiles(abilityPreviewTiles);

                        controller.currentUnit.playerUI.PreviewActionCost(assignedAbility.actionCost);

                        if(assignedAbility.abilityEquipmentType == KitType.Gunblade)
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
                    case TabType.Item:
                        //Yet to be implemented 
                        break;
                    case TabType.Action:
                        //Yet to be implemented
                        break;
                    default:
                        break;
                }
                
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
                    case TabType.Item:
                        //Yet to be implemented 
                        break;
                    case TabType.Action:
                        //Yet to be implemented
                        break;
                    default:
                        break;
                }
              
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
        foreach(RangeData r in assignedAbility.abilityRange)
        {
            List<Tile> tiles;
            AbilityRange range = r.GetOrCreateRange(r.range, controller.currentUnit.gameObject);
            range.unit = controller.currentUnit;

            tiles = range.GetTilesInRange(controller.board);

            foreach(Tile t in tiles)
            {
                if (!abilityPreviewTiles.Contains(t))
                {
                    abilityPreviewTiles.Add(t);
                }
            }
        }
        
    }
    public void GetTargets()
    {
        foreach (AbilityTargetType a in assignedAbility.elementsToTarget)
        {
            switch (a)
            {
                case AbilityTargetType.Enemies:
                    if (!monsterSelected)
                    {
                        foreach (Tile t in abilityPreviewTiles)
                        {
                            if (t.occupied)
                            {
                                targets.Add(controller.enemyUnits[0].unitSprite);
                                monsterSelected = true;
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

                                if (!u.isNearDeath)
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
