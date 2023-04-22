using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class PlaceObstacle : ActionNode
{
    [SerializeField] MonsterAbility obstacleRange;
    [SerializeField] int obstaclesToPlace;

    bool treeRunning;
    protected override void OnStart() {
        treeRunning = true;
        owner.controller.StartCoroutine(ObtaclePlacing());
    }

    protected override void OnStop() {
    }

    
        
    protected override State OnUpdate() {

        if (treeRunning)
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }

    IEnumerator ObtaclePlacing()
    {
        MonsterController controller = owner.controller;

        AudioManager.instance.Play("MonsterObstacle");
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
        treeRunning = false;

        //Code for exploding the obstacles, save for event

        //else
        //{
        //    List<Tile> tiles = new List<Tile>();

        //    for (int i = 0; i < obstaclesToPlace; i++)
        //    {
        //        List<Tile> rangeTiles = obstacleRange.GetAttackTiles(controller);

        //        List<Tile> validTiles = new List<Tile>();
        //        Tile tileToPlaceObstacle = new Tile();

        //        foreach (Tile t in rangeTiles)
        //        {
        //            if (t.content == null)
        //            {
        //                validTiles.Add(t);
        //            }
        //        }

        //        if (validTiles != null)
        //        {
        //            tileToPlaceObstacle = validTiles[Random.Range(0, validTiles.Count - 1)];
        //        }

        //        else
        //        {
        //            treeRunning = false;
        //            yield break;
        //        }


        //        tiles.Add(tileToPlaceObstacle);

        //        BearObstacleScript obstacle = controller.SpawnObstacle(tileToPlaceObstacle);
        //        obstacle.controller = controller;
        //        obstacle.pos = tileToPlaceObstacle.pos;
        //        controller.obstaclesInGame.Add(obstacle.GetComponent<BearObstacleScript>());

        //        if (obstacle.IsObstacleValid(controller.battleController.board))
        //        {
        //            controller.validObstacles.Add(obstacle);
        //        }

        //        tileToPlaceObstacle.content = obstacle.gameObject;
        //        obstacle.transform.parent = null;
        //    }

        //    controller.battleController.board.SelectAttackTiles(tiles);

        //    controller.monsterAnimations.SetBool("idle", false);
        //    controller.monsterAnimations.SetBool("roar", true);
        //    AudioManager.instance.Play("BearRoar");
        //    ActionEffect.instance.Play(4, 0.5f, 0.01f, 0.05f);

        //    while (ActionEffect.instance.CheckActionEffectState())
        //    {
        //        yield return null;
        //    }

        //    controller.monsterAnimations.SetBool("idle", true);
        //    controller.monsterAnimations.SetBool("roar", false);
        //    controller.battleController.board.DeSelectTiles(tiles);

        //    treeRunning = false;
        //}
    }
}
