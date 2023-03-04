using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttackEvent : MonsterEvent
{
    [SerializeField] MonsterAbility ability;

    public override IEnumerator Event()
    {
        List<Tile> tiles = new List<Tile>();
        BattleController battleController = controller.battleController;
        acting = true;

        if(controller.minionsInGame.Count > 0)
        {
            tiles = GetEventTiles();
            battleController.SelectTile(controller.currentEnemy.tile.pos);

            yield return new WaitForSeconds(1);
            List<PlayerUnit> targets = new List<PlayerUnit>();
            battleController.board.SelectAttackTiles(tiles);

            foreach (Tile t in tiles)
            {
                if (t.content != null)
                {
                    if (t.content.GetComponent<PlayerUnit>() != null)
                    {
                        targets.Add(t.content.GetComponent<PlayerUnit>());
                    }
                }
            }

            if (targets.Count > 0)
            {
                foreach (PlayerUnit p in targets)
                {
                    if (!p.isNearDeath)
                    {
                        ability.UseAbility(p, controller.currentEnemy, battleController);
                        p.DamageEffect();
                        p.animations.SetDamage();
                    }

                }
            }
        }

        controller.monsterAnimations.SetBool("roar", true);
        controller.monsterAnimations.SetBool("idle", false);

        ActionEffect.instance.Play(6, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.play || ActionEffect.instance.recovery)
        {
            yield return null;
        }

        controller.monsterAnimations.SetBool("roar", false);
        controller.monsterAnimations.SetBool("idle", true);

        battleController.board.DeSelectTiles(tiles);

        acting = false;
    }

    public override List<Tile> GetEventTiles()
    {
        if(controller.minionsInGame.Count > 0)
        {
            List<Tile> eventTiles = new List<Tile>();

            foreach (MinionUnit u in controller.minionsInGame)
            {
                List<Tile> tiles = u.GetMinionAttackArea(controller.battleController.board);

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
        else
        {
            return null;
        }
        
    }
}
