using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : WalkMovement
{
    public bool allowStun = true;

    public bool willStun = false;
    public int currentStunBuildUp = 0;
    public int stunlimit = 1;
    public int maxStunBuildUp = 3;
    public override void PushUnit(Directions pushDir, int pushStrength, Board board)
    {
        List<Tile> tiles = new List<Tile>();
        LineAbilityRange lineRange = GetComponent<LineAbilityRange>();
        lineRange.lineDir = pushDir;
        lineRange.lineLength = pushStrength;
        lineRange.lineOffset = 0;
        lineRange.stopLine = true;
        lineRange.monsterUse = true;
        List<Tile> rangeTiles = lineRange.GetTilesInRange(board);

        int indexTest = 0;
        foreach(Tile t in rangeTiles)
        {
            tiles.Add(t);
        }

        Tile desiredTile = null;

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] != null && tiles[i].CheckSurroundings(board) != null)
            {
                desiredTile = tiles[i];
                indexTest = i;
            }
            else
            {
                break;
            }

        }
        
        if(pushStrength > tiles.Count)
        {
            willStun = true;
            
        }

        if (desiredTile != null)
        {
            if (willStun)
            {
                if(indexTest > 0)
                {
                    desiredTile = tiles[indexTest - 1];

                }
            }
            StartCoroutine(Traverse(desiredTile, board, tiles));


        }        
        else
        {
            Stun();
        }


    }

    public void Stun()
    {
        if (allowStun)
        {
            unit.Stun();
            allowStun = false;
        }

        else
        {
            currentStunBuildUp++;

            if (currentStunBuildUp >= stunlimit)
            {
                stunlimit++;
                if (stunlimit >= maxStunBuildUp)
                {
                    stunlimit = maxStunBuildUp;
                }
                currentStunBuildUp = 0;

                allowStun = true;

            }
        }

        willStun = false;
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
