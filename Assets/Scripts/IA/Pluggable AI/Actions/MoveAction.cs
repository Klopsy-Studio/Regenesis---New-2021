using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    ClosestUnit, LeastHealthUnit, GetAway, Random
};
[CreateAssetMenu(menuName = "PluggableAI/Actions/MoveAction")]
public class MoveAction : Action
{
    public List<MoveType> moveOptions;
    public bool isCalled  = false;

    public bool differentTarget;

    bool impossibleMovement;
    Movement m;
    public override void Act(MonsterController controller)
    {
        if (!isCalled)
        {
            controller.CallCoroutine(MoveMonster(controller));
        }
    }

    private void OnEnable()
    {
        isCalled = false;
    }

    IEnumerator MoveMonster(MonsterController controller)
    {
        yield return new WaitForSeconds(1);
        MoveType chosenType = moveOptions[Random.Range(0, moveOptions.Count)];
        PlayerUnit t = new PlayerUnit();
        isCalled = true;
        float value = 0f;
        Tile tileToMove = new Tile();
        List<Tile> tiles = new List<Tile>();
        List<Tile> validTiles = new List<Tile>();
        List<PlayerUnit> invalidUnits = new List<PlayerUnit>();
        m = controller.GetComponent<Movement>();


        switch (chosenType)
        {
            case MoveType.ClosestUnit:
                while (validTiles.Count == 0)
                {
                    if(invalidUnits.Count >= 3)
                    {
                        impossibleMovement = true;
                        break;
                    }
                    foreach (PlayerUnit p in controller.battleController.playerUnits)
                    {
                        if (invalidUnits.Contains(p))
                            continue;
                        if (differentTarget && p == controller.target)
                            continue;
                        if (p.isNearDeath)
                            continue;
                        if (Vector3.Distance(controller.currentEnemy.transform.position, p.transform.position) <= value || value == 0f)
                        {
                            t = p;
                            value = Vector3.Distance(controller.currentEnemy.transform.position, p.transform.position);
                        }
                    }

                    if (t == null)
                    {
                        impossibleMovement = true;
                        break;
                    }

                    tiles = t.GetSurroundings(controller.battleController.board);

                    foreach (Tile tile in tiles)
                    {
                        if (tile.CheckSurroundings(controller.battleController.board) != null)
                        {
                            validTiles.Add(tile);
                        }
                    }

                    if (validTiles == null)
                    {
                        invalidUnits.Add(t);
                    }

                    else
                    {
                        controller.target = t;
                    }
                }
                break;
            case MoveType.LeastHealthUnit:

                while (validTiles.Count == 0)
                {
                    if (invalidUnits.Count >= 3)
                    {
                        impossibleMovement = true;
                        break;
                    }
                    foreach (PlayerUnit p in controller.battleController.playerUnits)
                    {
                        if (invalidUnits.Contains(p))
                            continue;
                        if (differentTarget && p == controller.target)
                            continue;
                        if (p.isNearDeath)
                            continue;
                        if (p.health < value || value == 0)
                        {
                            t = p;
                            value = t.health;
                        }
                    }

                    if (t == null)
                    {
                        impossibleMovement = true;
                        break;
                    }

                    tiles = t.GetSurroundings(controller.battleController.board);

                    foreach (Tile tile in tiles)
                    {
                        if (tile.CheckSurroundings(controller.battleController.board) != null)
                        {
                            validTiles.Add(tile);
                        }
                    }

                    if (validTiles.Count == 0)
                    {
                        invalidUnits.Add(t);
                    }
                    else
                    {
                        controller.target = t;
                    }
                }
                break;
            case MoveType.GetAway:
                m.range = 8;

                tiles = m.GetTilesInRangeForEnemy(controller.battleController.board, false);

                foreach (Tile tile in tiles)
                {
                    if (tile.CheckSurroundings(controller.battleController.board) != null)
                    {
                        validTiles.Add(tile);
                    }
                }

                if (validTiles.Count == 0)
                {
                    impossibleMovement = true;
                }

                else
                {
                    foreach (Tile tile in tiles)
                    {
                        if (value == 0 || Vector3.Distance(controller.currentEnemy.transform.position, tile.transform.position) >= value)
                        {
                            tileToMove = tile;
                            value = Vector3.Distance(controller.currentEnemy.transform.position, tile.transform.position);
                        }
                    }
                }
                break;

            case MoveType.Random:
                List<Tile> allTiles = new List<Tile>(controller.battleController.board.playableTiles.Values);

                foreach(Tile e in allTiles)
                {
                    if (e.CheckSurroundings(controller.battleController.board) != null) 
                    {
                        validTiles.Add(e);
                    }
                }

                if(validTiles.Count <= 0)
                {
                    impossibleMovement = true;
                }
                break;
            default:
                break;
        }

        if (!impossibleMovement)
        {
            tileToMove = validTiles[Random.Range(0, validTiles.Count)];
            //controller.battleController.board.SelectMovementTiles(test);


            controller.monsterAnimations.SetBool("hide", true);
            AudioManager.instance.Play("MonsterMovement");
            yield return new WaitForSeconds(1f);

            controller.monsterAnimations.SetBool("hide", false);

            controller.battleController.tileSelectionToggle.MakeTileSelectionBig();
            controller.CallCoroutine(m.SimpleTraverse(tileToMove));
            controller.battleController.SelectTile(tileToMove.pos);
            controller.monsterAnimations.SetBool("appear", true);
            //AudioManager.instance.Play("MonsterMovement");

            yield return new WaitForSeconds(1f);

            controller.monsterAnimations.SetBool("appear", false);
            controller.monsterAnimations.SetBool("idle", true);
            controller.monsterAnimations.SetBool("idle", false);

            controller.currentEnemy.currentPoint = tileToMove.pos;
            controller.currentEnemy.UpdateMonsterSpace(controller.battleController.board);

            controller.currentEnemy.actionDone = true;
        }

        OnExit(controller);
    }


    protected override void OnExit(MonsterController controller)
    {
        isCalled = false;
        base.OnExit(controller);
    }
}
