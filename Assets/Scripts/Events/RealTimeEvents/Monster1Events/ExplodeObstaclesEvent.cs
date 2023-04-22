using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeObstaclesEvent : MonsterEvent
{
    public override IEnumerator Event()
    {
        acting = true;
        foreach (BearObstacleScript o in controller.obstaclesInGame)
        {
            List<Tile> tiles = o.Explode(controller.battleController.board, controller.battleController);

            controller.battleController.SelectTile(o.pos);
            ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);
            AudioManager.instance.Play("ObstacleExplosion");

            while (ActionEffect.instance.CheckActionEffectState())
            {
                yield return null;
            }

            controller.battleController.board.DeSelectTiles(tiles);

            o.gameObject.SetActive(false);

        }

        controller.obstaclesInGame.Clear();

        acting = false;

    }

    public override List<Tile> GetEventTiles()
    {
        List<Tile> eventTiles = new List<Tile>();

        foreach (BearObstacleScript o in controller.obstaclesInGame)
        {
            List<Tile> tiles = o.GetAreaTiles(controller.battleController.board);

            foreach (Tile t in tiles)
            {
                if (!eventTiles.Contains(t))
                {
                    eventTiles.Add(t);
                }
            }
        }

        return eventTiles;
    }
}
