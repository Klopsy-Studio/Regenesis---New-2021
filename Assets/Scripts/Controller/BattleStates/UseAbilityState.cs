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
        
        owner.ChangeUIButtons(false);
        CleanSelectTiles();
        owner.currentUnit.WeaponOut();
        owner.isTimeLineActive = false;
        currentAbility = owner.currentUnit.weapon.Abilities[owner.attackChosen];
        owner.targets.noTargetText.SetActive(false);
        owner.currentUnit.playerUI.PreviewActionCost(currentAbility.actionCost);
        owner.timelineUI.CallTimelinePreviewOrderOnAbilitySelect(owner.currentUnit, currentAbility.actionCost);
        //tiles = PreviewAbility();
        tiles = new List<Tile>();
        owner.actionSelectionUI.ThirdWindow();
        owner.abilitySelectionUI.SecondWindow();
        owner.abilitySelectionUI.title.SetActive(false);
        owner.tileSelectionToggle.SelectionTarget();
        owner.targets.stopSelection = false;
        foreach (RangeData r in currentAbility.abilityRange)
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

                        if (t.content != null)
                        {
                            if (t.content.GetComponent<EnemyUnit>() != null)
                            {
                                targetTiles.Add(t);
                            }
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
                                PlayerUnit u = t.content.GetComponent<PlayerUnit>();

                                if(!u.isDead && !u.isNearDeath)
                                {
                                    targetTiles.Add(t);
                                }
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
            if (targetTiles.Count > 0)
            {
                owner.targets.gameObject.SetActive(true);

                if (targetTiles != null || targetTiles.Count > 0)
                {
                    owner.targets.CreateTargets(targetTiles);
                }
            }

            else
            {
                owner.targets.gameObject.SetActive(true);
                owner.targets.CreateNoTarget();
            }

        }
        else
        {
            owner.actionSelectionUI.gameObject.SetActive(false);
            owner.abilitySelectionUI.gameObject.SetActive(false);
            owner.targets.gameObject.SetActive(false);
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
                                owner.UpdateUnitSprite();
                                selectedTile = board.GetTile(e.info + t.pos);

                                if (selectTiles != null)
                                {
                                    board.DeSelectTiles(selectTiles);
                                }

                                GetSelectTiles(currentAbility);
                                
                                if(owner.currentUnit.weapon.EquipmentType == KitType.Drone)
                                {
                                    GetDroneSelectTiles(currentAbility);
                                }
                                
                                switch (currentAbility.abilityEffect)
                                {
                                    case EffectType.Damage:
                                        board.SelectAttackTiles(selectTiles);
                                        break;
                                    case EffectType.Heal:
                                        board.SelectHealTiles(selectTiles);
                                        break;
                                    case EffectType.Buff:
                                        break;
                                    case EffectType.Debuff:
                                        break;
                                    default:
                                        break;
                                }

                                foreach(Tile tile in selectTiles)
                                {
                                    if(tile.content != null)
                                    {
                                        if (tile.content.GetComponent<Unit>() != null)
                                        {
                                            if (!spriteTargets.Contains(tile.content.GetComponent<Unit>().unitSprite))
                                            {
                                                spriteTargets.Add(tile.content.GetComponent<Unit>().unitSprite);
                                            }
                                        }

                                        if(tile.content.GetComponent<BearObstacleScript>()!= null)
                                        {
                                            if (!spriteTargets.Contains(tile.content.GetComponent<SpriteRenderer>()))
                                            {
                                                spriteTargets.Add(tile.content.GetComponent<SpriteRenderer>());
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
        List<Tile> tiles = new List<Tile>();

        foreach(RangeData r in ability.tileTargetAbilityRange)
        {
            tiles = GetTiles(r, owner.currentUnit.gameObject);

            foreach(Tile t in tiles)
            {
                if (!selectTiles.Contains(t))
                {
                    selectTiles.Add(t);
                }
            }
        }
    }

    public void GetDroneSelectTiles(Abilities ability)
    {
        List<Tile> tiles = new List<Tile>();

        if(owner.currentUnit.droneUnit != null)
        {
            PlayerUnit droneUnit = owner.currentUnit.droneUnit;

            foreach(RangeData r in ability.tileTargetAbilityRange)
            {
                tiles = GetOppositeTiles(r, owner.currentUnit.droneUnit.gameObject);


                foreach (Tile t in tiles)
                {
                    if (!selectTiles.Contains(t))
                    {
                        selectTiles.Add(t);
                    }
                }
            }
        }
    }

    public List<Tile> ReturnSelectTiles(Abilities ability)
    {
        List<Tile> tiles = new List<Tile>();

        foreach (RangeData r in ability.tileTargetAbilityRange)
        {
            tiles = GetTiles(r, owner.currentUnit.gameObject);
        }

        return tiles;
    }
     
    public List<Tile> ReturnDroneTiles(Abilities ability)
    {
        List<Tile> tiles = new List<Tile>();

        if (owner.currentUnit.droneUnit != null)
        {
            PlayerUnit droneUnit = owner.currentUnit.droneUnit;

            foreach (RangeData r in ability.tileTargetAbilityRange)
            {
                tiles = GetOppositeTiles(r, owner.currentUnit.droneUnit.gameObject);
            }
        }

        return tiles;
    }
    public List<Tile> GetTiles (RangeData data, GameObject user)
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
                LineAbilityRange line = GetRange<LineAbilityRange>(user);
                line.AssignVariables(data);                
                line.lineDir = owner.currentUnit.tile.GetDirections(owner.currentTile);
                return line.GetTilesInRange(board);
            case TypeOfAbilityRange.SelfAbility:
                return null;
            case TypeOfAbilityRange.SquareAbility:
                SquareAbilityRange square = GetRange<SquareAbilityRange>(user);
                square.AssignVariables(data);
                return square.GetTilesInRangeWithoutUnit(board, owner.currentTile.pos);
            case TypeOfAbilityRange.Side:
                SideAbilityRange side = GetRange<SideAbilityRange>(user);
                side.AssignVariables(data);
                side.unit = owner.currentUnit;
                side.sideDir = owner.currentUnit.tile.GetDirections(owner.currentTile);
                
                return side.GetTilesInRange(board);
            case TypeOfAbilityRange.AlternateSide:
                AlternateSideRange altSide = GetRange<AlternateSideRange>(user);
                altSide.AssignVariables(data);
                return altSide.GetTilesInRange(board);
            case TypeOfAbilityRange.Cross:
                CrossAbilityRange cross = GetRange<CrossAbilityRange>(user);
                cross.AssignVariables(data);
                return cross.GetTilesInRange(board);
            case TypeOfAbilityRange.Normal:
                return null;
            case TypeOfAbilityRange.Item:
                ItemRange item = GetRange<ItemRange>(user);
                item.AssignVariables(data);
                item.tile = owner.currentTile;
                return item.GetTilesInRange(board);
            default:
                return null;
        }
    }
    public List<Tile> GetOppositeTiles(RangeData data, GameObject user)
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
                LineAbilityRange line = GetRange<LineAbilityRange>(user);
                line.AssignVariables(data);
                line.unit = user.GetComponent<PlayerUnit>();

                switch (owner.currentUnit.tile.GetDirections(owner.currentTile))
                {
                    case Directions.North:
                        line.lineDir = Directions.South;
                        break;
                    case Directions.East:
                        line.lineDir = Directions.West;
                        break;
                    case Directions.West:
                        line.lineDir = Directions.East;
                        break;
                    case Directions.South:
                        line.lineDir = Directions.North;
                        break;
                    default:
                        break;
                }

                return line.GetTilesInRange(board);
            case TypeOfAbilityRange.SelfAbility:
                return null;
            case TypeOfAbilityRange.SquareAbility:
                SquareAbilityRange square = GetRange<SquareAbilityRange>(user);
                square.AssignVariables(data);
                return square.GetTilesInRangeWithoutUnit(board, owner.currentTile.pos);
            case TypeOfAbilityRange.Side:
                SideAbilityRange side = GetRange<SideAbilityRange>(user);
                side.AssignVariables(data);
                side.unit = user.GetComponent<PlayerUnit>();

                switch (owner.currentUnit.tile.GetDirections(owner.currentTile))
                {
                    case Directions.North:
                        side.sideDir = Directions.South;
                        break;
                    case Directions.East:
                        side.sideDir = Directions.West;
                        break;
                    case Directions.West:
                        side.sideDir = Directions.East;
                        break;
                    case Directions.South:
                        side.sideDir = Directions.North;
                        break;
                    default:
                        break;
                }

                return side.GetTilesInRange(board);
            case TypeOfAbilityRange.AlternateSide:
                AlternateSideRange altSide = GetRange<AlternateSideRange>(user);
                altSide.AssignVariables(data);
                return altSide.GetTilesInRange(board);
            case TypeOfAbilityRange.Cross:
                CrossAbilityRange cross = GetRange<CrossAbilityRange>(user);
                cross.AssignVariables(data);
                return cross.GetTilesInRange(board);
            case TypeOfAbilityRange.Normal:
                return null;
            case TypeOfAbilityRange.Item:
                ItemRange item = GetRange<ItemRange>(user);
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

                    foreach(Tile t in selectTiles)
                    {
                        if (tiles.Contains(t))
                        {
                            tiles.Remove(t);
                        }
                    }

                    board.DeSelectDefaultTiles(tiles);

                    owner.bowExtraAttackObject.SetActive(false);

                    if(owner.currentUnit.weapon.EquipmentType == KitType.Drone)
                    {
                        StartCoroutine(UseAbilitySequence(ReturnSelectTiles(currentAbility), ReturnDroneTiles(currentAbility)));
                    }
                    else
                    {
                        StartCoroutine(UseAbilitySequence(selectTiles));
                    }

                    owner.actionSelectionUI.gameObject.SetActive(false);
                    owner.abilitySelectionUI.gameObject.SetActive(false);
                    owner.targets.gameObject.SetActive(false);
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
                    owner.actionSelectionUI.gameObject.SetActive(false);
                    owner.abilitySelectionUI.gameObject.SetActive(false);
                    owner.targets.gameObject.SetActive(false);
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
        owner.targets.indicator.DeactivateTarget();

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
        owner.targets.indicator.DeactivateTarget();
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

    public IEnumerator UseAbilitySequence(List<Tile> target, List<Tile> droneTiles)
    {
        attacking = true;
        owner.targets.indicator.DeactivateTarget();
        StartCoroutine(currentAbility.sequence.Sequence(target, droneTiles, owner));

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

        owner.timelineUI.ExitPreviewTurnOrder();
        if(tiles != null)
        {
            board.DeSelectDefaultTiles(tiles);
        }

        if(selectTiles != null)
        {
            board.DeSelectDefaultTiles(selectTiles);
        }
        owner.ChangeUIButtons(true);

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
