using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Place Obstacle")]

public class PlaceObstacleAction : Action
{
    [SerializeField] MonsterAbility obstacleRange;

    public int obstaclesToPlace;
    public override void Act(MonsterController controller)
    {
        controller.CallCoroutine(PlaceObstacle(controller));
    }

    IEnumerator PlaceObstacle(MonsterController controller)
    {
        if(controller.obstaclesInGame.Count >= controller.obstacleLimit)
        {
            AudioManager.instance.Play("MonsterObstacle");
            foreach(BearObstacleScript o in controller.obstaclesInGame)
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
            OnExit(controller);

        }
        else
        {
            List<Tile> tiles = new List<Tile>();

            for (int i = 0; i < obstaclesToPlace; i++)
            {
                List<Tile> rangeTiles = obstacleRange.GetAttackTiles(controller);

                List<Tile> validTiles = new List<Tile>();
                Tile tileToPlaceObstacle = new Tile();

                foreach (Tile t in rangeTiles)
                {
                    if (t.content == null)
                    {
                        validTiles.Add(t);
                    }
                }

                if (validTiles != null)
                {
                    tileToPlaceObstacle = validTiles[Random.Range(0, validTiles.Count - 1)];
                }

                else
                {
                    OnExit(controller);
                    yield break;
                }


                tiles.Add(tileToPlaceObstacle);

                BearObstacleScript obstacle = Instantiate(controller.obstacle, new Vector3(tileToPlaceObstacle.pos.x, 1, tileToPlaceObstacle.pos.y), controller.obstacle.transform.rotation).GetComponent<BearObstacleScript>();
                obstacle.controller = controller;
                obstacle.pos = tileToPlaceObstacle.pos;
                controller.obstaclesInGame.Add(obstacle.GetComponent<BearObstacleScript>());

                if (obstacle.IsObstacleValid(controller.battleController.board))
                {
                    controller.validObstacles.Add(obstacle);
                }

                tileToPlaceObstacle.content = obstacle.gameObject;
                obstacle.transform.parent = null;
            }

            controller.battleController.board.SelectAttackTiles(tiles);

            controller.monsterAnimations.SetBool("idle", false);
            controller.monsterAnimations.SetBool("roar", true);
            AudioManager.instance.Play("MonsterRoar");
            ActionEffect.instance.Play(4, 0.5f, 0.01f, 0.05f);

            while (ActionEffect.instance.CheckActionEffectState())
            {
                yield return null;
            }

            controller.monsterAnimations.SetBool("idle", true);
            controller.monsterAnimations.SetBool("roar", false);
            controller.battleController.board.DeSelectTiles(tiles);
            controller.currentEnemy.Default();

            OnExit(controller);
        }
        
    }
}
