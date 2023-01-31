using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbilityState : BattleState
{
    List<Tile> tiles;
    List<Tile> targetTiles = new List<Tile>();
    List<Tile> selectTiles = new List<Tile>();

    public Abilities currentAbility;

    bool onMonster;
    //PLACEHOLDER 
    bool attacking;
    bool selected;

    bool test;

    bool isTargetTile;
    public Tile selectedTile;
    List<SpriteRenderer> spriteTargets = new List<SpriteRenderer>();
    public override void Enter()
    {
        base.Enter();
        CleanSelectTiles();
        owner.currentUnit.WeaponOut();
        owner.isTimeLineActive = false;
        owner.ActivateTileSelector();
        currentAbility = owner.currentUnit.weapon.Abilities[owner.attackChosen];

        owner.currentUnit.playerUI.PreviewActionCost(currentAbility.actionCost);
        //tiles = PreviewAbility();
        tiles = new List<Tile>();

        foreach(RangeData r in currentAbility.abilityRange)
        {
            List<Tile> dirtyTiles = PreviewAbility(r);
            foreach(Tile t in dirtyTiles)
            {
                if (!tiles.Contains(t))
                {
                    tiles.Add(t);
                }
            }
        }

        board.SelectAbilityTiles(tiles);

        switch (owner.currentUnit.weapon.EquipmentType)
        {
            case KitType.Hammer:
                break;
            case KitType.Bow:
                owner.bowExtraAttackObject.SetActive(true);
                break;
            case KitType.Gunblade:
                break;
            default:
                break;
        }

        foreach(AbilityTargetType target in currentAbility.elementsToTarget)
        {
            switch (target)
            {
                case AbilityTargetType.Enemies:
                    foreach(Tile t in tiles)
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
                        if(t.content != null)
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
                        if(t.content != null)
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
    }


    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (!attacking)
        {
            SelectTile(e.info + pos);
        }
    }

    protected override void OnFire(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!attacking)
        {
            if (owner.currentTile.occupied)
            {
                if (owner.currentTile.content.gameObject.GetComponent<EnemyUnit>() != null && tiles.Contains(owner.currentTile))
                {
                    owner.currentUnit.playerUI.HideActionPoints();
                    //StartCoroutine(UseAbilitySequence(owner.currentTile.content.gameObject.GetComponent<EnemyUnit>()));
                }
            }
        }

    }

    public void ExtraAttackBow()
    {
        if(currentAbility.actionCost+1<= owner.currentUnit.actionsPerTurn)
        {
            owner.SetBowExtraAttack();

            if (owner.bowExtraAttack)
            {
                owner.currentUnit.playerUI.PreviewActionCost(currentAbility.actionCost + 1);
            }
            else
            {
                owner.currentUnit.playerUI.ShowActionPoints();
                owner.currentUnit.playerUI.PreviewActionCost(currentAbility.actionCost);
            }
        }
    }

    protected override void OnMouseSelectEvent(object sender, InfoEventArgs<Point> e)
    {
        if (isTargetTile)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                var a = hit.transform.gameObject;
                var t = a.GetComponent<Tile>();

                if (t != null)
                {
                    if (!attacking)
                    {
                        if (tiles != null)
                        {
                            if (tiles.Contains(board.GetTile(e.info + t.pos)))
                            {
                                SelectTile(e.info + t.pos);
                                selectedTile = board.GetTile(e.info + t.pos);

                                if (selectTiles != null)
                                {
                                    board.DeSelectTiles(selectTiles);
                                }

                                GetSelectTiles(currentAbility);

                                board.SelectAttackTiles(selectTiles);

                                foreach(Tile tile in selectTiles)
                                {
                                    if(tile.content != null)
                                    {
                                        if (t.content.GetComponent<Unit>())
                                        {
                                            if (!spriteTargets.Contains(t.content.GetComponent<Unit>().unitSprite))
                                            {
                                                spriteTargets.Add(t.content.GetComponent<Unit>().unitSprite);
                                            }
                                        }

                                        if(t.content.GetComponent<BearObstacleScript>()!= null)
                                        {
                                            if (!spriteTargets.Contains(t.content.GetComponent<SpriteRenderer>()))
                                            {
                                                spriteTargets.Add(t.content.GetComponent<SpriteRenderer>());
                                            }
                                        }
                                    }

                                    if (tile.occupied)
                                    {
                                        if (!spriteTargets.Contains(owner.enemyUnits[0].unitSprite))
                                        {
                                            spriteTargets.Add(owner.enemyUnits[0].unitSprite);
                                        }
                                    }
                                }


                                if(spriteTargets != null)
                                {
                                    if (spriteTargets.Count > 0)
                                    {
                                        foreach(SpriteRenderer s in spriteTargets)
                                        {
                                            s.color = new Color(s.color.r, s.color.g, s.color.b, 1f);
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                board.DeSelectTiles(selectTiles);
                                selectTiles.Clear();

                                if (spriteTargets != null)
                                {
                                    if (spriteTargets.Count > 0)
                                    {
                                        foreach (SpriteRenderer s in spriteTargets)
                                        {
                                            s.color = new Color(s.color.r, s.color.g, s.color.b, 0.6f);
                                        }
                                    }

                                    spriteTargets.Clear();
                                    selectedTile = null;
                                }

                            }
                        }


                    }
                }

            }

           
        }
        
    }

    public void GetSelectTiles(Abilities ability)
    {
        List<Tile> tiles;

        foreach(RangeData r in ability.tileTargetAbilityRange)
        {
            tiles = GetTiles(r);

            foreach(Tile t in tiles)
            {
                if (!selectTiles.Contains(t))
                {
                    selectTiles.Add(t);
                }
            }
        }
    }
    public List<Tile> GetTiles (RangeData data)
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
    public void SelectMonster(EnemyUnit enemy, Tile t)
    {
        CleanSelectTiles();
        selectTiles = enemy.GiveMonsterSpace(board);
        board.SelectAttackTiles(selectTiles);
        SelectTile(enemy.currentPoint);
        owner.tileSelectionToggle.MakeTileSelectionBig();
        onMonster = true;
    }
    
    public void CleanSelectTiles()
    {
        if(selectTiles != null)
        {
            board.DeSelectTiles(selectTiles);
            selectTiles.Clear();
            selectTiles = new List<Tile>();
        }
    }
    protected override void OnMouseConfirm(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!test)
        {
            test = true;
            return;
        }
        if (!attacking)
        {
            if (isTargetTile)
            {
                if(selectedTile!= null)
                {
                    if (spriteTargets != null)
                    {
                        if (spriteTargets.Count > 0)
                        {
                            foreach (SpriteRenderer s in spriteTargets)
                            {
                                s.color = new Color(s.color.r, s.color.g, s.color.b, 1f);
                            }
                        }
                    }

                    owner.ResetUnits();

                    owner.currentUnit.playerUI.HideActionPoints();
                    owner.currentUnit.playerUI.HideBullets();

                    board.DeSelectTiles(tiles);
                    board.DeSelectDefaultTiles(selectTiles);

                    owner.bowExtraAttackObject.SetActive(false);
                    StartCoroutine(UseAbilitySequence(selectTiles));
                    owner.ResetUnits();
                    owner.targets.gameObject.SetActive(false);
                    owner.targets.stopSelection = true;
                }
            }

            else
            {
                if (owner.targets.selectedTarget != null)
                {
                    owner.currentUnit.playerUI.HideActionPoints();
                    owner.currentUnit.playerUI.HideBullets();
                    GameObject objectTarget = owner.targets.selectedTarget.targetAssigned;

                    board.DeSelectTiles(tiles);
                    owner.bowExtraAttackObject.SetActive(false);
                    StartCoroutine(UseAbilitySequence(objectTarget));
                    owner.ResetUnits();
                    owner.targets.gameObject.SetActive(false);
                    owner.targets.stopSelection = true;
                }
            }
            
        }
    }

    protected override void OnEscape(object sender, InfoEventArgs<KeyCode> e)
    {
        //if (!attacking)
        //{
        //    SelectTile(owner.currentUnit.currentPoint);
        //    owner.ChangeState<SelectAbilityState>();
        //}
        
    }

    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!attacking)
        {
            owner.currentUnit.animations.SetIdle();
            owner.currentUnit.playerUI.ShowActionPoints();
            owner.DeactivateTileSelector();
            SelectTile(owner.currentUnit.currentPoint);
            owner.ChangeState<SelectAbilityState>();
        }
    }

    IEnumerator UseAbilitySequence(GameObject target)
    {
        attacking = true;

        StartCoroutine(currentAbility.sequence.Sequence(target, owner));

        while (currentAbility.sequence.playing)
        {
            yield return null;
        }

        if (!owner.endTurnInstantly)
        {
            owner.currentUnit.animations.SetIdle();
            owner.ChangeState<SelectActionState>();
        }

        else
        {
            owner.endTurnInstantly = false;
            owner.ChangeState<FinishPlayerUnitTurnState>();
        }
    }

    public IEnumerator UseAbilitySequence(List<Tile> target)
    {
        attacking = true;

        StartCoroutine(currentAbility.sequence.Sequence(target, owner));

        while (currentAbility.sequence.playing)
        {
            yield return null;
        }

        if (!owner.endTurnInstantly)
        {
            owner.currentUnit.animations.SetIdle();
            owner.ChangeState<SelectActionState>();
        }

        else
        {
            owner.endTurnInstantly = false;
            owner.ChangeState<FinishPlayerUnitTurnState>();
        }
    }
    public override void Exit()
    {
        base.Exit();

        if(tiles != null)
        {
            board.DeSelectDefaultTiles(tiles);
        }

        if(selectTiles != null)
        {
            board.DeSelectDefaultTiles(selectTiles);
        }

        attacking = false;
        tiles = null;
        Movement mover = owner.currentUnit.GetComponent<Movement>();
        mover.ResetRange();

        owner.attackChosen = 0;

        targetTiles.Clear();
        spriteTargets.Clear();
        owner.targets.ClearTargets();
        owner.ResetUnits();
        owner.targets.gameObject.SetActive(false);
        owner.targets.stopSelection = false;

        //For the first click inside the state
        test = false;

        //Bow variables
        owner.bowExtraAttackObject.SetActive(false);
        owner.bowExtraAttack = false;
        owner.ResetBowExtraAttack();
        isTargetTile = false;
        selectedTile = null;
    }

    public List<Tile> PreviewAbility(RangeData data)
    {
        List<Tile> t = new List<Tile>();
        AbilityRange range = data.GetOrCreateRange(data.range,owner.currentUnit.gameObject);
        range.unit = owner.currentUnit;

        t = range.GetTilesInRange(board);
        return t;

    }
   
}
