using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    public List<Tile> tiles;
    [SerializeField] int buttonTest = 1;
    Tile originPoint;
    int staminaPreview;
    MovementRange range;

    bool test;
    public override void Enter()
    {
        base.Enter();
        owner.ActivateTileSelector();
        owner.tileSelectionToggle.MakeTileSelectionSmall();
        owner.tileSelectionToggle.SelectionMovement();

        owner.isTimeLineActive = false;

        owner.SetCameraStill(); 

        Movement m = owner.currentUnit.GetComponent<Movement>();

        m.range = owner.currentUnit.weapon.range;
        tiles = m.GetTilesInRange(board, true);
        owner.currentUnit.playerUI.PreviewActionCost(2);
        board.SelectMovementTiles(tiles);
        tiles.Add(owner.currentTile);
        originPoint = owner.currentTile;
        owner.ghostImage.sprite = owner.currentUnit.unitSprite.sprite;
        owner.currentLevelManager.FadeProps();

        owner.ChangeCurrentControls("Move");

        owner.FadeUnits();
    }

    public override void Exit()
    {
        base.Exit();
        owner.ghostImage.gameObject.SetActive(false);
        owner.currentLevelManager.ResetProps();

        owner.ResetUnits();

        board.DeSelectDefaultTiles(tiles);
        test = false;
        tiles = null;

        
        owner.ResetCamera();
    }



    protected override void OnMouseCancelEvent(object sender, InfoEventArgs<KeyCode> e)
    {
        SelectTile(owner.currentUnit.currentPoint);
        AudioManager.instance.Play("Boton" + owner.exitMenu);

        owner.DeactivateTileSelector();
        owner.ChangeState<SelectActionState>();
    }
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if(CanReachTile(e.info+pos, tiles))
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
                    if(owner.currentTile != t)
                    {
                        SelectTile(e.info + t.pos);
                        owner.ghostImage.gameObject.SetActive(true);
                        AudioManager.instance.Play("Boton" + owner.hoverTile);
                        owner.UpdateUnitSprite();
                    }
                    
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
            owner.ChangeState<MoveSequenceState>();
        }
    }

}