using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1CheckRange : Monster1State
{
    public List<Tile> tiles;

    bool check1;

    bool actionChosen;
    public override void Enter()
    {
        base.Enter();
        battleController.SelectTile(owner.currentEnemy.tile.pos);
        StartCoroutine(CheckRange());
    }

    IEnumerator CheckRange()
    {
        yield return new WaitForSeconds(1.5f);
        SquareAbilityRange check1Range = GetRange<SquareAbilityRange>();
        check1Range.unit = owner.currentEnemy;
        SideAbilityRange attack2Range = GetRange<SideAbilityRange>();
        attack2Range.unit = owner.currentEnemy;

        
        int numberOfUnitsInRange = 0;

        tiles = check1Range.GetTilesInRange(owner.battleController.board);




        foreach (Tile t in tiles)
        {
            if (t.content != null)
            {
                //If there is an enemy in range 1, we check if there are more than 2 enemies in this range
                if (t.content.GetComponent<PlayerUnit>() != null)
                {
                    owner.currentTarget = t.content.GetComponent<PlayerUnit>();
                    check1 = true;
                    break;
                }
            }
        }

        if (check1 && !actionChosen)
        {
            tiles.Clear();
            
            //Check if there are more than 2 enemies in each side range
            for (int i = 0; i < 3; i++)
            {
                tiles = attack2Range.GetTilesInRange(battleController.board);

                foreach (Tile e in tiles)
                {
                    if (e.content != null)
                    {
                        if (e.GetComponent<PlayerUnit>() != null)
                        {
                            owner.targets.Add(e.GetComponent<PlayerUnit>());
                            numberOfUnitsInRange++;
                        }
                    }
                }

                //If there are 2 or more enemies in this side, we check if the last attack was 2
                if (numberOfUnitsInRange >= 2)
                {
                    //If it was we do attack 1
                    if (attackToUse == 1)
                    {
                        owner.attackToUse = 0;
                        owner.lastAction = EnemyActions.Attack;

                        //Choose random target
                        //Fill
                        yield return null;
                        owner.ChangeState<Monster1Attack>();   
                        actionChosen = true;
                        break;
                    }
                    //If not, we do attack 2
                    else
                    {
                        owner.attackToUse = 1;
                        owner.currentTarget = owner.targets.ToArray()[0];
                        owner.lastAction = EnemyActions.Attack;
 
                        yield return null;
                        owner.ChangeState<Monster1Attack>();
                        actionChosen = true;
                        break;
                    }
                }


                //If there weren't to or more enemies on this side, we check the other ones.

                //switch (attack2Range.dir)
                //{
                //    case Directions.North:
                //        attack2Range.dir = Directions.East;
                //        break;
                //    case Directions.East:
                //        attack2Range.dir = Directions.South;
                //        break;
                //    case Directions.South:
                //        attack2Range.dir = Directions.West;
                //        break;
                //    case Directions.West:
                //        attack2Range.dir = Directions.West;
                //        break;
                //    default:
                //        break;
                //}

                owner.targets.Clear();
                tiles.Clear();
            }

            //If attack 2 isn't possible, do attack1 or movement

            if (owner.lastAction == EnemyActions.Attack)
            {
                yield return null;
                owner.lastAction = EnemyActions.Move;
                owner.ChangeState<Monster1MoveToClosestUnit>();
            }

            else
            {
                yield return null;
                owner.attackToUse = 0;
                owner.lastAction = EnemyActions.Attack;
                owner.ChangeState<Monster1Attack>();
            }
        }

        //If there wasn't any units in Range 1, we check for range 2

        if (!actionChosen)
        {
            tiles.Clear();
            LineAbilityRange range2 = GetRange<LineAbilityRange>();
            range2.lineLength = 2;
            range2.unit = owner.currentEnemy;
            GetLineRange(range2, Directions.North, tiles);
            GetLineRange(range2, Directions.South, tiles);
            GetLineRange(range2, Directions.East, tiles);
            GetLineRange(range2, Directions.West, tiles);

            foreach (Tile t in tiles)
            {
                if (t.content != null)
                {
                    if (t.content.GetComponent<PlayerUnit>() != null)
                    {
                        //If there is a unit at range 2, we check if the last attack was attack3
                        //If it was, the enemy moves to the closest enemy,
                        if (attackToUse == 2)
                        {
                            owner.lastAction = EnemyActions.Move;
                            yield return null;
                            owner.ChangeState<Monster1MoveToClosestUnit>();
                            actionChosen = true;
                            break;
                        }

                        //If not, we do attack3
                        else
                        {
                            owner.currentTarget = t.content.GetComponent<PlayerUnit>();
                            owner.attackToUse = 2;
                            owner.lastAction = EnemyActions.Attack;
                            yield return null;
                            owner.ChangeState<Monster1Attack>();
                            actionChosen = true;
                            break;
                        }
                    }
                }
            }
        }
        

        //If not, we check if the last action was creating an obstacle
        //If it was, we move

        if(owner.lastAction == EnemyActions.Special && !actionChosen)
        {
            yield return null;
            owner.lastAction = EnemyActions.Move;
            owner.ChangeState<Monster1MoveToClosestUnit>();
        }

        //If not, enemy creates an obstacle
        else
        {
            if (!actionChosen)
            {
                yield return null;
                owner.lastAction = EnemyActions.Special;
                owner.ChangeState<Monster1SpecialAbility>();
            }
            
        }

        actionChosen = false;
    }

    public override void Exit()
    {
        base.Exit();
        check1 = false;
    }


    public void GetLineRange(LineAbilityRange range, Directions lineDir, List<Tile> tileList)
    {
        range.ChangeDirection(lineDir);

        foreach (Tile t in range.GetTilesInRange(battleController.board))
        {
            tileList.Add(t);
        }
    }
}
