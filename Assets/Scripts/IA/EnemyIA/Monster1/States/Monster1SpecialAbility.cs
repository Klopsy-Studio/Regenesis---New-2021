using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1SpecialAbility : Monster1State
{
    public override void Enter()
    {
        stateName = "Special Ability";
        base.Enter();
        StartCoroutine(PlaceObstacle());
    }


    IEnumerator PlaceObstacle()
    {
        if(owner.tileToPlaceObstacle == null)
        {
            MonsterAbilityRange range = GetComponent<MonsterAbilityRange>();
            List<Tile> tiles = range.GetTilesInRange(owner.battleController.board);
            int e = Random.Range(0, tiles.Count);

            while(tiles.ToArray()[e].content != null && !tiles.ToArray()[e].occupied)
            {
                e = Random.Range(0, tiles.Count);
            }

            owner.tileToPlaceObstacle = tiles.ToArray()[e];
        }

        
        GameObject obstacle = Instantiate(owner.obstacle, new Vector3(owner.tileToPlaceObstacle.pos.x, 1, owner.tileToPlaceObstacle.pos.y), owner.obstacle.transform.rotation);
        owner.tileToPlaceObstacle.content = obstacle;
        obstacle.transform.parent = null;

        owner.currentEnemy.Damage();

        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.play)
        {
            yield return null;
        }

        owner.currentEnemy.Default();

        yield return new WaitForSeconds(0.5f);

        owner.ChangeState<Monster1CheckNextAction>();

    }

    public override void Exit()
    {
        base.Exit();
        owner.tileToPlaceObstacle = null;
        owner.lastAction = EnemyActions.Special;
        owner.currentEnemy.actionDone = true;
    }
}
