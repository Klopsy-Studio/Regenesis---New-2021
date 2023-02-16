using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemState : BattleState
{
    public List<Tile> tiles;
    List<Tile> targetTiles = new List<Tile>();
    public List<Tile> selectTiles;
    Consumables currentItem;

    bool isTargetTile;
    ItemRange range;
    
    bool isTimelineItem = false;

    bool itemUsed;

    bool firstClick = false;
    public override void Enter()
    {
        base.Enter();
        isTargetTile = false;
        itemUsed = false;
        owner.isTimeLineActive = false;
        owner.actionSelectionUI.gameObject.SetActive(false);
        owner.ChangeUIButtons(false);

        currentItem = owner.backpackInventory.consumableContainer[owner.itemChosen].consumable;

        owner.currentUnit.playerUI.PreviewActionCost(2);

        if(currentItem.ConsumableType == ConsumableType.NormalConsumable)
        {
            StartCoroutine(Init());
        }

        else if (currentItem.ConsumableType == ConsumableType.TimelineConsumable || currentItem.ConsumableType == ConsumableType.TargetConsumable)
        {
            isTimelineItem = true;
            owner.currentUnit.animations.SetPrepareThrow();
            tiles = GetRangeOnItems(currentItem.itemRange);
            board.SelectMovementTiles(tiles);

            if(currentItem.itemRange != null)
            {
                owner.ghostImage.sprite = currentItem.itemSprite;
            }

            owner.timelineUI.CallTimelinePreviewOrderOnItemSelect();

        }


        foreach (AbilityTargetType target in currentItem.elementsToTarget)
        {
            switch (target)
            {
                case AbilityTargetType.Enemies:
                    foreach (Tile t in tiles)
                    {
                        if (t.occupied)
                        {
                            targetTiles.Add(t);
                        }
                    }
                    break;
                case AbilityTargetType.Allies:
                    foreach (Tile t in tiles)
                    {
                        if (t.content != null)
                        {
                            if (t.content.GetComponent<PlayerUnit>() != null)
                            {
                                targetTiles.Add(t);
                            }
                        }
                    }
                    break;
                case AbilityTargetType.Obstacles:
                    foreach (Tile t in tiles)
                    {
                        if (t.content != null)
                        {
                            if (t.content.GetComponent<BearObstacleScript>() != null)
                            {
                                targetTiles.Add(t);
                            }
                        }
                    }
                    break;
                case AbilityTargetType.Self:
                    targetTiles.Add(owner.currentUnit.tile);
                    break;

                case AbilityTargetType.Tile:
                    isTargetTile = true;
                    break;
                default:
                    break;
            }
        }

        if (!isTargetTile)
        {
            owner.targets.gameObject.SetActive(true);

            if (targetTiles != null || targetTiles.Count > 0)
            {
                owner.targets.CreateTargets(targetTiles);
            }
        }
        else
        {
            owner.targets.gameObject.SetActive(false);
        }
    }

    public List<Tile> GetRangeOnItems(RangeData data)
    {
        switch (data.range)
        {
            case TypeOfAbilityRange.Cone:
                return null;
            case TypeOfAbilityRange.Constant:
                return null;
            case TypeOfAbilityRange.Infinite:
                return null;
            case TypeOfAbilityRange.LineAbility:
                LineAbilityRange line = GetRange<LineAbilityRange>();
                line.AssignVariables(data);
                return line.GetTilesInRange(board);
            case TypeOfAbilityRange.SelfAbility:
                return null;
            case TypeOfAbilityRange.SquareAbility:
                SquareAbilityRange square = GetRange<SquareAbilityRange>();
                square.AssignVariables(data);
                return square.GetTilesInRangeWithoutUnit(board, owner.currentTile.pos);
            case TypeOfAbilityRange.Side:
                SideAbilityRange side = GetRange<SideAbilityRange>();
                side.AssignVariables(data);
                return side.GetTilesInRange(board);
            case TypeOfAbilityRange.AlternateSide:
                AlternateSideRange altSide = GetRange<AlternateSideRange>();
                altSide.AssignVariables(data);
                return altSide.GetTilesInRange(board);
            case TypeOfAbilityRange.Cross:
                CrossAbilityRange cross = GetRange<CrossAbilityRange>();
                cross.AssignVariables(data);
                return cross.GetTilesInRange(board);
            case TypeOfAbilityRange.Normal:
                return null;
            case TypeOfAbilityRange.Item:
                ItemRange item = GetRange<ItemRange>();
                item.AssignVariables(data);
                item.tile = owner.currentTile;
                return item.GetTilesInRange(board);
            default:
                return null;
        }
    }
    IEnumerator Init()
    {
        owner.backpackInventory.UseConsumable(owner.itemChosen, targetUnit: owner.currentUnit);
        owner.currentUnit.playerUI.gameObject.SetActive(false);

        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);
        
        while(ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        owner.currentUnit.SpendActionPoints(owner.itemCost);

        owner.ChangeState<FinishPlayerUnitTurnState>();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (!isTimelineItem) return;
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!isTimelineItem) return;

        if (owner.currentTile.content == null)
        {
            owner.backpackInventory.UseConsumable(owner.itemChosen, tileSpawn: owner.currentTile);
            itemUsed = true;
            owner.ChangeState<FinishPlayerUnitTurnState>();
        }
    }

    protected override void OnMouseSelectEvent(object sender, InfoEventArgs<Point> e)
    {
        if (isTargetTile)
        {
            if (!isTimelineItem || itemUsed) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                var a = hit.transform.gameObject;
                var t = a.GetComponent<Tile>();
                if (t != null)
                {
                    if (tiles.Contains(t))
                    {
                        SelectTile(e.info + t.pos);
                        owner.ActivateTileSelector();
                        if (selectTiles != null)
                        {
                            board.DeSelectTiles(selectTiles);
                            selectTiles.Clear();
                        }
                        owner.ghostImage.gameObject.SetActive(true);
                        selectTiles = GetRangeOnItems(currentItem.effectRange);
                        board.SelectAttackTiles(selectTiles);
                    }

                    else
                    {
                        if (selectTiles != null)
                        {
                            board.DeSelectTiles(selectTiles);
                            selectTiles.Clear();
                        }

                        owner.DeactivateTileSelector();
                    }
                }

            }
        }
    }

    protected override void OnMouseConfirm(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!isTimelineItem || itemUsed) return;

        if (!firstClick)
        {
            firstClick = true;
        }
        else
        {
            if (isTargetTile)
            {
                if (!itemUsed)
                {
                    owner.currentUnit.playerUI.HideActionPoints();
                    owner.ResetUnits();

                    board.DeSelectDefaultTiles(tiles);

                    owner.targets.gameObject.SetActive(false);
                    owner.targets.stopSelection = true;

                    switch (currentItem.ConsumableType)
                    {
                        case ConsumableType.NormalConsumable:
                            break;
                        case ConsumableType.TimelineConsumable:
                            if (owner.currentTile.content == null && tiles.Contains(owner.currentTile))
                            {
                                StartCoroutine(UseItemSpace());
                            }
                            break;
                        case ConsumableType.TargetConsumable:
                            if (owner.currentTile.content != null && tiles.Contains(owner.currentTile))
                            {
                                StartCoroutine(UseItemSpace());
                            }
                            break;
                        default:
                            break;
                    }
                }
                
            }
            else
            {
                if (!itemUsed)
                {
                    if(owner.targets.selectedTarget != null)
                    {
                        owner.currentUnit.playerUI.HideActionPoints();

                        board.DeSelectTiles(tiles);
                        owner.ResetUnits();
                        owner.targets.gameObject.SetActive(false);
                        owner.targets.stopSelection = true;
                        StartCoroutine(UseItemSpace());

                    }
                }
            }
            
        }
    }
    IEnumerator UseItemSpace()
    {
        itemUsed = true;
        owner.DeactivateTileSelector();
        owner.backpackInventory.UseConsumable(owner.itemChosen, tileSpawn: owner.currentTile, battleController: owner);
        //Unit item pose
        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        owner.currentUnit.SpendActionPoints(owner.itemCost);

        owner.ChangeState<SelectActionState>();
    }
    protected override void OnEscape(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!isTimelineItem) return;
        SelectTile(owner.currentUnit.currentPoint);
        owner.ChangeState<SelectItemState>();

    }
    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!isTimelineItem) return;
        owner.currentUnit.animations.SetIdle();
        SelectTile(owner.currentUnit.currentPoint);
        owner.ChangeState<SelectItemState>();

    }


    public override void Exit()
    {
        base.Exit();
        owner.itemChosen = 0;
        owner.targets.ClearTargets();
        targetTiles.Clear();
        owner.targets.gameObject.SetActive(false);
        if (tiles != null)
        {
            board.DeSelectDefaultTiles(tiles);
            tiles.Clear();
        }

        if (selectTiles != null)
        {
            board.DeSelectDefaultTiles(selectTiles);
            selectTiles.Clear();
        }
        
        owner.ghostImage.gameObject.SetActive(false);

        owner.ChangeUIButtons(true);

        firstClick = false;
    }
}
