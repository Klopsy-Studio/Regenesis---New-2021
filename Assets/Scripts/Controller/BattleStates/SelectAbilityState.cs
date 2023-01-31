using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAbilityState : BattleState
{
    public int currentActionIndex;
    bool canShowUI;
    Abilities[] abilityList;
    List<Tile> tiles = new List<Tile>();

    bool onPreview;
    public override void Enter()
    {
        base.Enter();
        owner.actionSelectionUI.gameObject.SetActive(true);
        owner.isTimeLineActive = false;
        owner.moveAbilitySelector = true;

        AbilitySelectionUI.gameObject.SetActive(true);
        owner.abilitySelectionUI.EnableAbilitySelection();
        abilityList = owner.currentUnit.weapon.Abilities;
        AbilitySelectionUI.ChangeAllAbilitiesToDefault();
        AbilitySelectionUI.DeactivateAllAbilitySelection();

        if(owner.currentUnit.weapon.EquipmentType == KitType.Gunblade)
        {
            owner.currentUnit.playerUI.ShowBullets();
        }

        owner.FadeUnits();

        for (int i = 0; i < abilityList.Length; i++)
        {
            if(owner.currentUnit.weapon.Abilities[i] != null)
            {
                AbilitySelectionUI.options[i].gameObject.SetActive(true);

                if(owner.currentUnit.weapon.EquipmentType == KitType.Gunblade)
                {
                    if (owner.currentUnit.weapon.Abilities[i].CanDoAbility(owner.currentUnit.actionsPerTurn, owner.currentUnit))
                    {
                        AbilitySelectionUI.EnableSelectAbilty(i);
                    }
                    else
                    {
                        AbilitySelectionUI.DisableSelectAbilty(i);
                    }
                }
                else
                {
                    if (owner.currentUnit.weapon.Abilities[i].CanDoAbility(owner.currentUnit.actionsPerTurn))
                    {
                        AbilitySelectionUI.EnableSelectAbilty(i);

                    }
                    else
                    {
                        AbilitySelectionUI.DisableSelectAbilty(i);
                    }
                }
                

                AbilitySelectionUI.options[i].GetComponent<Text>().text = abilityList[i].abilityName;

                SelectorMovement e = AbilitySelectionUI.options[i].GetComponent<SelectorMovement>();
                e.abilityDescription.AssignData(abilityList[i]);
                e.assignedAbility = abilityList[i];
                e.ClearOption();
                e.GetPreviewRange();
                e.GetTargets();
            }
        }

        owner.abilitySelectionUI.ResetSelector();

        
        //Meter ActivarUI
    }

    protected override void OnEscape(object sender, InfoEventArgs<KeyCode> e)
    {
        owner.ChangeState<SelectActionState>();
    }

    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        owner.ResetUnits();
        owner.currentUnit.playerUI.HideBullets();
        owner.ChangeState<SelectActionState>();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.info.y >= 1)
        {
            AbilitySelectionUI.MoveBackwards();
            
            if(currentActionIndex > 0)
            {
                currentActionIndex--;
            }

            else
            {
                currentActionIndex = owner.currentUnit.weapon.Abilities.Length - 1;
            }

        }

        if (e.info.y <= -1)
        {
            AbilitySelectionUI.MoveForward();

            if(currentActionIndex < owner.currentUnit.weapon.Abilities.Length - 1)
            {
                currentActionIndex++;
            }
            else
            {
                currentActionIndex = 0;
            }
        }
    }

    protected override void OnSelectAction(object sender, InfoEventArgs<int> e)
    {
        owner.attackChosen = e.info;

        if(owner.currentUnit.weapon.EquipmentType == KitType.Gunblade)
        {
            if (abilityList[owner.attackChosen].CanDoAbility(owner.currentUnit.actionsPerTurn, owner.currentUnit))
            {
                ActionSelectionUI.gameObject.SetActive(false);
                owner.ChangeState<UseAbilityState>();
            }
        }
        else
        {
            if (abilityList[owner.attackChosen].CanDoAbility(owner.currentUnit.actionsPerTurn))
            {
                ActionSelectionUI.gameObject.SetActive(false);
                owner.ChangeState<UseAbilityState>();
            }
        }
        
    }

    //protected override void OnMouseSelectEvent(object sender, InfoEventArgs<Point> e)
    //{
    //    if (owner.abilitySelectionUI.onOption)
    //    {
    //        if (onPreview)
    //        {
    //            board.DeSelectTiles(tiles);
    //            tiles.Clear();
    //            onPreview = false;
    //        }

    //        Abilities currentAbility = AbilitySelectionUI.options[AbilitySelectionUI.currentSelection].GetComponent<SelectorMovement>().assignedAbility;
    //        AbilityRange range = currentAbility.rangeData.GetOrCreateRange(currentAbility.rangeData.range, owner.currentUnit.gameObject);
    //        range.unit = owner.currentUnit;

    //        tiles = range.GetTilesInRange(board);

    //        board.SelectAbilityTiles(tiles);

    //        owner.currentUnit.playerUI.PreviewActionCost(currentAbility.actionCost);

    //        onPreview = true;
    //    }

    //    else
    //    {
    //        if(tiles != null || tiles.Count > 0)
    //        {
    //            board.DeSelectTiles(tiles);
    //            tiles.Clear();
    //            onPreview = false;
    //        }

    //        owner.currentUnit.playerUI.ShowActionPoints();
    //    }
    //}
    protected override void OnSelectCancelEvent(object sender, InfoEventArgs<int> e)
    {
        owner.currentUnit.playerUI.ShowActionPoints();
        owner.attackChosen = e.info;
    }

    protected override void OnFire(object sender, InfoEventArgs<KeyCode> e)
    {
        owner.attackChosen = currentActionIndex;


        if (owner.currentUnit.ActionsPerTurn >= abilityList[currentActionIndex].actionCost)
        {
            ActionSelectionUI.gameObject.SetActive(false);
            owner.ChangeState<UseAbilityState>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        owner.abilitySelectionUI.DisableAbilitySelection();

        owner.moveAbilitySelector = false;
        currentActionIndex = 0;
        AbilitySelectionUI.ResetSelector();
        AbilitySelectionUI.gameObject.SetActive(false);
        AbilitySelectionUI.onOption = false;
        onPreview = false;

        tiles.Clear();

        for (int i = 0; i < AbilitySelectionUI.options.Length; i++)
        {
            AbilitySelectionUI.options[i].GetComponent<SelectorMovement>().ClearOption();
        }
    }

}
