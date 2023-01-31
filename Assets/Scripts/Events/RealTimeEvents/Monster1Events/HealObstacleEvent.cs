using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealObstacleEvent : MonsterEvent
{

    public override IEnumerator Event()
    {
        acting = true;
        BattleController battleController = controller.battleController;
        yield return null;

        List<BearObstacleScript> recheckedObstacles = new List<BearObstacleScript>();

        foreach(BearObstacleScript b in controller.validObstacles)
        {
            if (b.IsObstacleValid(battleController.board))
            {
                recheckedObstacles.Add(b);
            }
        }

        if(recheckedObstacles.Count > 0)
        {
            BearObstacleScript chosenObstacle = recheckedObstacles[Random.Range(0, recheckedObstacles.Count)];

            List<Tile> tiles = chosenObstacle.GetValidTiles(battleController.board);

            Tile chosenTile = tiles[Random.Range(0, tiles.Count)];

            controller.monsterAnimations.SetBool("hide", true);
            AudioManager.instance.Play("MonsterMovement");
            yield return new WaitForSeconds(1f);

            Movement m = controller.GetComponent<Movement>();
            controller.monsterAnimations.SetBool("hide", false);

            controller.battleController.tileSelectionToggle.MakeTileSelectionBig();
            controller.CallCoroutine(m.SimpleTraverse(chosenTile));
            controller.battleController.SelectTile(chosenTile.pos);
            controller.monsterAnimations.SetBool("appear", true);
            //AudioManager.instance.Play("MonsterMovement");

            yield return new WaitForSeconds(1f);

            controller.monsterAnimations.SetBool("appear", false);
            controller.monsterAnimations.SetBool("idle", true);
            controller.monsterAnimations.SetBool("idle", false);

            controller.currentEnemy.currentPoint = chosenTile.pos;
            controller.currentEnemy.UpdateMonsterSpace(controller.battleController.board);

            yield return new WaitForSeconds(1f);

            controller.monsterAnimations.SetBool("roar", true);
            controller.monsterAnimations.SetBool("idle", false);

            //Hardcoded heal, replace when skills are implemented
            controller.currentEnemy.Heal(30);
            controller.currentEnemy.HealEffect();
            ActionEffect.instance.Play(4, 0.5f, 0.01f, 0.05f);


            while (ActionEffect.instance.play || ActionEffect.instance.recovery)
            {
                yield return null;
            }

            controller.monsterAnimations.SetBool("roar", false);
            controller.monsterAnimations.SetBool("idle", true);

            battleController.board.GetTile(chosenObstacle.pos).content = null;
            controller.obstaclesInGame.Remove(chosenObstacle);
            controller.validObstacles.Remove(chosenObstacle);
            chosenObstacle.gameObject.SetActive(false);
        }

        acting = false;
    }
}
