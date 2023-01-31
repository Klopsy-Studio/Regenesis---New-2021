using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : WalkMovement
{

    public bool allowStun = false;

    public override void PushUnit(Directions pushDir, int pushStrength, Board board)
    {
        LineAbilityRange lineRange = GetComponent<LineAbilityRange>();
        lineRange.lineDir = pushDir;
        lineRange.lineLength = pushStrength;
        //lineRange.lineOffset = pushStrength - 1;
        lineRange.stopLine = true; 

        CrossAbilityRange crossRange = GetComponent<CrossAbilityRange>();
        crossRange.offset = pushStrength - 1;
        crossRange.crossLength = pushStrength;
        range = pushStrength;
        List<Tile> tiles = lineRange.GetTilesInRange(board);
        Tile desiredTile = null;


        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] != null && tiles[i].CheckSurroundings(board) != null)
            {
                desiredTile = tiles[i];
            }
            else
            {
                break;
            }

        }
        
        

        if (desiredTile != null)
        {
            desiredTile.prev = unit.tile;
            StartCoroutine(Traverse(desiredTile, board));
        }
        else
        {
            if (allowStun)
            {
                unit.Stun();
            }
        }
    }

    public void EnableMonsterStun()
    {
        allowStun = true;
    }

    public void DisableMonsterStun()
    {
        allowStun = false;
    }
}
