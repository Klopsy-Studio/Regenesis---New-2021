using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsEvent : MonsterEvent
{
    [SerializeField] MonsterAbility rootsAbility;
    [SerializeField] bool canPush;
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
            t.SetRootVfx();
            if(t.content != null)
            {
                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    targets.Add(t.content.GetComponent<PlayerUnit>());
                }
            }
        }

        if(targets.Count > 0)
        {
            foreach(PlayerUnit p in targets)
            {
                //Hardcoded, change it with implementation of monster skills
                if (!p.isNearDeath)
                {
                    rootsAbility.UseAbility(p, controller.currentEnemy, battleController);
                    p.DamageEffect();
                    p.animations.SetDamage();

                    if(p.buffModifiers.Count > 0)
                    {
                        foreach(Modifier m in p.buffModifiers)
                        {
                            if(m.modifierType == TypeOfModifier.Antivirus)
                            {
                                p.RemoveBuff(m);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (canPush)
                        {
                            p.GetComponent<Movement>().PushUnit(controller.currentEnemy.tile.GetDirections(p.tile), 1, controller.battleController.board);
                        }
                        p.DecreaseTimelineVelocity(1);
                        p.AddDebuff(new Modifier { timelineSpeedReduction = 1, modifierType = TypeOfModifier.TimelineSpeed });
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
}
