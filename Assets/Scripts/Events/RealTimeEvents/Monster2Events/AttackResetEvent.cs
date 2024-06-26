using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackResetEvent : MonsterEvent
{
    [SerializeField] MonsterAbility ability;
    public override IEnumerator Event()
    {
        acting = true;

        BattleController battleController = controller.battleController;
        List<Tile> tiles = GetEventTiles();
        battleController.SelectTile(controller.currentEnemy.tile.pos);
        //Tile animation:
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
                    if(p.GetComponent<Movement>() != null)
                    {
                        p.GetComponent<Movement>().PushUnit(controller.currentEnemy.tile.GetDirections(p.tile), 2, controller.battleController.board);
                    }
                    p.DamageEffect();
                    p.animations.SetDamage();
                }

            }
        }

        controller.monsterAnimations.SetTrigger("attackReset");

        ActionEffect.instance.Play(6, 0.5f, 0.01f, 0.05f);


        while (ActionEffect.instance.play || ActionEffect.instance.recovery)
        {
            yield return null;
        }


        battleController.board.DeSelectTiles(tiles);

        acting = false;
    }
}
