using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_MoveTargeStateOne : BattleState
{
    [SerializeField] RangeData range;
    public List<Tile> tiles;
    Tile originPoint;
    public override void Enter()
    {
        base.Enter();

        owner.ActivateTileSelector();
        owner.tileSelectionToggle.MakeTileSelectionSmall();
        owner.tileSelectionToggle.SelectionMovement();

        owner.isTimeLineActive = false;


        AbilityRange abilityRange = range.GetOrCreateRange(range.range, owner.currentUnit.gameObject);
        tiles = abilityRange.GetTilesInRange(board);

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
}
