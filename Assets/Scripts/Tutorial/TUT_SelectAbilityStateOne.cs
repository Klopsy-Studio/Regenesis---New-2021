using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TUT_SelectAbilityStateOne : BattleState
{
    public int currentActionIndex;
    bool canShowUI;
    Abilities[] abilityList;
    List<Tile> tiles = new List<Tile>();

    bool onPreview;

    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            owner.hammerTraitObject.GetComponent<MenuButton>().MakeButtonAppear();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            owner.hammerTraitObject.GetComponent<MenuButton>().MakeButtonHide();
        }

    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Ha entrado a TUT:SELECTABILITY STATE");
        owner.actionSelectionUI.gameObject.SetActive(true);
        owner.isTimeLineActive = false;
        owner.moveAbilitySelector = true;

        AbilitySelectionUI.gameObject.SetActive(true);
        owner.abilitySelectionUI.EnableAbilitySelection();
        abilityList = owner.currentUnit.weapon.Abilities;
        AbilitySelectionUI.ChangeAllAbilitiesToDefault();
        AbilitySelectionUI.DeactivateAllAbilitySelection();
        owner.actionSelectionUI.SecondWindow();
        owner.actionSelectionUI.title.SetActive(false);
        owner.abilitySelectionUI.title.SetActive(true);
        owner.abilitySelectionUI.OriginalColor();
        owner.hammerTraitObject.gameObject.SetActive(false);
        owner.gunbladeUI.bulletParent.SetActive(false);

        switch (owner.currentUnit.weapon.EquipmentType)
        {
            case KitType.Hammer:
                owner.hammerTraitObject.gameObject.SetActive(true);
                if (owner.hammerCurrentFury != null)
                {
                    owner.hammerCurrentFury.value = owner.currentUnit.hammerFuryAmount;
                }
              
                owner.hammerTraitObject.GetComponent<MenuButton>().SetDefaultPosition();

                owner.hammerTraitObject.GetComponent<MenuButton>().MakeButtonAppear();
                owner.hammerPreviewFury.value = 0;
                break;
            case KitType.Bow:
                break;
            case KitType.Gunblade:
                owner.gunbladeUI.bulletParent.SetActive(true);
                owner.gunbladeUI.button.SetDefaultPosition();
                owner.gunbladeUI.button.MakeButtonAppear();
                owner.gunbladeUI.ResetBullets();
                owner.gunbladeUI.ShowBullets(owner.currentUnit.gunbladeAmmoAmount);
                break;
            case KitType.Drone:
                foreach (PlayerUnit u in playerUnits)
                {
                    if (u != owner.currentUnit)
                    {
                        owner.droneController.CreateTargets(u);
                    }
                }
                break;
            default:
                break;
        }


        owner.FadeUnits();

        for (int i = 0; i < abilityList.Length; i++)
        {
            if (owner.currentUnit.weapon.Abilities[i] != null)
            {
                AbilitySelectionUI.options[i].gameObject.SetActive(true);

                if (owner.currentUnit.weapon.EquipmentType == KitType.Gunblade)
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
                e.assignedAbility = abilityList[i];
                e.ClearOption();
                e.GetPreviewRange();
                e.GetTargets();
            }
        }

        owner.abilitySelectionUI.ResetSelector();


        //Meter ActivarUI
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.info.y >= 1)
        {
            AbilitySelectionUI.MoveBackwards();

            if (currentActionIndex > 0)
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

            if (currentActionIndex < owner.currentUnit.weapon.Abilities.Length - 1)
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

        if (owner.currentUnit.weapon.EquipmentType == KitType.Gunblade)
        {
            if (abilityList[owner.attackChosen].CanDoAbility(owner.currentUnit.actionsPerTurn, owner.currentUnit))
            {
                //ActionSelectionUI.gameObject.SetActive(false);

                owner.ChangeState<TutShowslideState>();
            }
        }
        else
        {
            if (abilityList[owner.attackChosen].CanDoAbility(owner.currentUnit.actionsPerTurn))
            {
                //ActionSelectionUI.gameObject.SetActive(false);
                owner.ChangeState<TutShowslideState>();
            }
        }

    }

    protected override void OnSelectCancelEvent(object sender, InfoEventArgs<int> e)
    {
        owner.currentUnit.playerUI.ShowActionPoints();
        owner.attackChosen = e.info;
    }

 
    public override void Exit()
    {
        base.Exit();
        owner.abilitySelectionUI.DisableAbilitySelection();

        owner.moveAbilitySelector = false;
        currentActionIndex = 0;
        AbilitySelectionUI.ResetSelector();

        AbilitySelectionUI.DisableAbilitySelection();
        AbilitySelectionUI.onOption = false;
        onPreview = false;

        tiles.Clear();

        for (int i = 0; i < AbilitySelectionUI.options.Length; i++)
        {
            AbilitySelectionUI.options[i].GetComponent<SelectorMovement>().ClearOption();
        }
    }
}
