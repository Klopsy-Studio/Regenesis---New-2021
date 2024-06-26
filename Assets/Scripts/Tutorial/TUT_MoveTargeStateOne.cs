using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_MoveTargeStateOne : BattleState
{
    [SerializeField] RangeData range;
    public List<Tile> tiles;
    Tile originPoint;

    bool test;
    public override void Enter()
    {
        base.Enter();

        owner.ActivateTileSelector();
        owner.tileSelectionToggle.MakeTileSelectionSmall();
        owner.tileSelectionToggle.SelectionMovement();

        owner.isTimeLineActive = false;

        AbilityRange abilityRange = range.GetOrCreateRange(range.range, owner.currentUnit.gameObject);
        abilityRange.unit = owner.currentUnit;
        tiles = abilityRange.GetTilesInRange(board);
        board.SelectMovementTiles(tiles);

        owner.currentUnit.playerUI.PreviewActionCost(2);
     
        tiles.Add(owner.currentTile);
        originPoint = owner.currentTile;
        owner.ghostImage.sprite = owner.currentUnit.unitSprite.sprite;

        foreach (Unit u in unitsInGame)
        {
            if (u != owner.currentUnit)
            {
                u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.b, u.unitSprite.color.g, u.unitSprite.color.a - 0.35f);
            }
        }

        
    }

    public override void Exit()
    {
        base.Exit();
        owner.ghostImage.gameObject.SetActive(false);

        foreach (Unit u in unitsInGame)
        {
            if (u != owner.currentUnit)
            {
                u.unitSprite.color = new Color(u.unitSprite.color.r, u.unitSprite.color.b, u.unitSprite.color.g, u.unitSprite.color.a + 0.35f);
            }
        }

        board.DeSelectDefaultTiles(tiles);
        test = false;
        tiles = null;
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (CanReachTile(e.info + pos, tiles))
        {
            if (tiles.Contains(board.GetTile(e.info + pos)))
            {
                SelectTile(e.info + pos);
            }
        }
    }

    protected override void OnMouseSelectEvent(object sender, InfoEventArgs<Point> e)
    {
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
                    owner.ghostImage.gameObject.SetActive(true);
                    owner.UpdateUnitSprite();
                }
                else
                {
                    //owner.ghostImage.gameObject.SetActive(false);
                }
            }
        }
    }

    protected override void OnMouseConfirm(object sender, InfoEventArgs<KeyCode> e)
    {
        if (!test)
        {
            test = true;
            return;
        }
        if (tiles.Contains(owner.currentTile) && owner.currentTile != originPoint)
        {
            owner.ghostImage.gameObject.SetActive(false);
            owner.currentUnit.didNotMove = false;
            owner.currentUnit.SpendActionPoints(owner.moveCost);
            owner.currentUnit.actionDone = true;
            owner.ChangeState<TUT_MoveSequenceStateOne>();
        }
    }

    protected override void OnFire(object sender, InfoEventArgs<KeyCode> e)
    {
        if (tiles.Contains(owner.currentTile) && owner.currentTile != originPoint)
        {
            owner.currentUnit.playerUI.SpendActionPoints(2);

            owner.currentUnit.didNotMove = false;
            owner.currentUnit.ActionsPerTurn -= 1;
            owner.currentUnit.actionDone = true;
            owner.ChangeState<TUT_MoveSequenceStateOne>();
        }
    }
}
