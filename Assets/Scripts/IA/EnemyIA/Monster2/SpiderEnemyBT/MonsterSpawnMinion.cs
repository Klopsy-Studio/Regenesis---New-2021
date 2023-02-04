using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterSpawnMinion : ActionNode
{
    bool treeUpdate;
    protected override void OnStart() {
        treeUpdate = true;
        owner.controller.StartCoroutine(SpawnMinions());
    }

    protected override void OnStop() {

    }

    IEnumerator SpawnMinions()
    {
        //Choose random Hunter

        BattleController controller = owner.controller.battleController;
        Unit randomHunter = controller.playerUnits[Random.Range(0, controller.playerUnits.Count)];

        //Select Direction

        List<Tile> areaTiles = new List<Tile>();
        Directions chosenDirection = owner.controller.currentEnemy.tile.GetDirections(randomHunter.tile);
        foreach(RangeData r in owner.controller.minionRangeSpawn)
        {
            r.sideDir = chosenDirection;
            AbilityRange range = r.GetOrCreateRange(r.range, owner.controller.gameObject);
            //Get Spawn Area
            List<Tile> tiles = range.GetTilesInRange(controller.board);
            
            foreach(Tile t in tiles)
            {
                if(!areaTiles.Contains(t) && t.content == null && !t.occupied)
                {
                    areaTiles.Add(t);
                }
            }
        }

        controller.board.SelectAttackTiles(areaTiles);
        //Check how many minions can i spawn and which type first type 1, second type 2, third type random.

        int numberToSpawn = owner.controller.maxMinions - owner.controller.minionsInGame.Count;

        if(numberToSpawn > 0)
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                Tile t = areaTiles[Random.Range(0, areaTiles.Count)];
                areaTiles.Remove(t);

                if (i > owner.controller.minionsPrefab.Count - 1)
                {
                    owner.controller.SpawnMinion(Random.Range(0, owner.controller.minionsPrefab.Count), t);
                }

                else
                {
                    owner.controller.SpawnMinion(i, t);
                }
            }

            //Action Effect

            ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);
            owner.controller.monsterAnimations.SetBool("roar", true);
            owner.controller.monsterAnimations.SetBool("idle", false);

            while (ActionEffect.instance.CheckActionEffectState())
            {
                yield return null;
            }

            owner.controller.monsterAnimations.SetBool("roar", false);
            owner.controller.monsterAnimations.SetBool("idle", true);
        }

        treeUpdate = false;
    }
    protected override State OnUpdate() {

        if (treeUpdate)
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }
}
