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

        //Check how many minions can i spawn and which type first type 1, second type 2, third type random.

        int numberToSpawn = owner.controller.maxMinions - owner.controller.minionsInGame.Count;

        if (numberToSpawn > 0)
        {
            ActionEffect.instance.Play(5, 2f, 0.01f, 0.05f);
            owner.controller.monsterAnimations.SetBool("idle", false);

            for (int i = 0; i < numberToSpawn; i++)
            {
                owner.controller.monsterAnimations.SetBool("spawn"+i, true);

                Tile t = areaTiles[Random.Range(0, areaTiles.Count)];
                areaTiles.Remove(t);
                yield return new WaitForSeconds(0.4f);

                if (i > owner.controller.minionsPrefab.Count - 1)
                {
                    owner.controller.SpawnMinion(Random.Range(0, owner.controller.minionsPrefab.Count), t);
                }

                else
                {
                    owner.controller.SpawnMinion(i, t);
                }

                yield return new WaitForSeconds(0.6f);
            }

            //Action Effect
            for (int i = 0; i < 3; i++)
            {
                owner.controller.monsterAnimations.SetBool("spawn" + i, false);
            }
            owner.controller.monsterAnimations.SetBool("idle", true);

            yield return new WaitForSeconds(1f);
            while (ActionEffect.instance.CheckActionEffectState())
            {
                yield return null;
            }

            owner.controller.monsterAnimations.SetBool("spawn", false);
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
