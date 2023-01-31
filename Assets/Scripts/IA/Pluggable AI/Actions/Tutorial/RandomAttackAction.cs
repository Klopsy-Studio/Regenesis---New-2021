using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Random Attack")]
public class RandomAttackAction : Action
{
    public List<MonsterAbility> abilities;
    public override void Act(MonsterController controller)
    {
        controller.CallCoroutine(Attack(controller, abilities[Random.Range(0, abilities.Count)]));
    }


    IEnumerator Attack(MonsterController controller, MonsterAbility ability)
    {
        Directions dir = controller.currentEnemy.tile.GetDirections(controller.target.tile);
        List<Tile> tiles = ability.ShowAttackRange(dir, controller);
        controller.targetsInRange.Clear();

        switch (ability.targetType)
        {
            case TypeOfTarget.SingleTarget:
                controller.targetsInRange.Add(controller.target);
                break;
            case TypeOfTarget.MultipleTarget:

                foreach (Tile t in tiles)
                {
                    if (t.content != null)
                    {
                        if (t.content.GetComponent<PlayerUnit>() != null)
                        {
                            controller.targetsInRange.Add(t.content.GetComponent<PlayerUnit>());
                        }
                    }
                }

                break;
            default:
                break;
        }
        
        AudioManager.instance.Play("MonsterAttack");


        controller.battleController.board.SelectAttackTiles(tiles);

        foreach(PlayerUnit u in controller.targetsInRange)
        {
            ability.UseAbility(u, controller.currentEnemy, controller.battleController);
            u.DamageEffect();
        }

        controller.monsterAnimations.SetBool("idle", false);

        controller.monsterAnimations.SetBool(ability.attackTrigger, true);

        if (ability.inAbilityEffects != null)
        {
            foreach (Effect e in ability.inAbilityEffects)
            {
                foreach(PlayerUnit u in controller.targetsInRange)
                {
                    switch (e.effectType)
                    {
                        case TypeOfEffect.PushUnit:
                            e.PushUnit(u, dir, controller.battleController.board);
                            break;
                        default:
                            break;
                    }
                }
                
            }
        }
        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        Debug.Log("Test");
        controller.monsterAnimations.SetBool(ability.attackTrigger, false);
        controller.monsterAnimations.SetBool("idle", true);

        if (ability.postAbilityEffect != null)
        {
            foreach (Effect e in ability.postAbilityEffect)
            {
                switch (e.effectType)
                {
                    case TypeOfEffect.FallBack:
                        e.FallBack(controller.currentEnemy, dir, controller.battleController.board);
                        break;
                    default:
                        break;
                }
            }
        }
        while (ActionEffect.instance.recovery)
        {
            yield return null;
        }

        
        controller.battleController.board.DeSelectDefaultTiles(tiles);

        foreach(PlayerUnit u in controller.targetsInRange)
        {
            if (!u.isNearDeath)
            {
                u.Default();
            }
        }

        controller.validAbilities.Clear();
        OnExit(controller);
    }
}
