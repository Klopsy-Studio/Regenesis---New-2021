using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterAttack : ActionNode
{
    [SerializeField] List<MonsterAbility> abilities;
    bool treeRunning;
    protected override void OnStart() {

        treeRunning = true;
        owner.controller.StartCoroutine(Attack(abilities[Random.Range(0, abilities.Count)]));
    }

    protected override void OnStop() {
    }
    IEnumerator Attack(MonsterAbility ability)
    {
        MonsterController controller = owner.controller;
        List<Tile> tiles = new List<Tile>();
        Directions dir = Directions.North;
        if(controller.target != null)
        {
            owner.controller.currentEnemy.UpdateEnemyUnitSprite();
            dir = controller.currentEnemy.tile.GetDirections(controller.target.tile);
            tiles = ability.ShowAttackRange(dir, controller);
        }
        


        controller.targetsInRange.Clear();

        switch (ability.targetType)
        {
            case TypeOfTarget.SingleTarget:
                controller.targetsInRange.Add(controller.target);
                owner.controller.currentEnemy.UpdateEnemyUnitSprite();
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
            case TypeOfTarget.RandomSingleTarget:
                foreach (Tile t in tiles)
                {
                    List<PlayerUnit> possibleTargets = new List<PlayerUnit>();
                    if (t.content != null)
                    {
                        if (t.content.GetComponent<PlayerUnit>() != null)
                        {
                            possibleTargets.Add(t.content.GetComponent<PlayerUnit>());
                        }
                    }

                    controller.targetsInRange.Add(possibleTargets[Random.Range(0, possibleTargets.Count)]);
                }
                break;
            default:
                break;
        }

        AudioManager.instance.Play("MonsterAttack");


        controller.battleController.board.SelectAttackTiles(tiles);

        foreach (PlayerUnit u in controller.targetsInRange)
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
                foreach (PlayerUnit u in controller.targetsInRange)
                {
                    switch (e.effectType)
                    {
                        case TypeOfEffect.PushUnit:
                            e.PushUnit(u, dir, controller.battleController.board);
                            break;
                        case TypeOfEffect.SlowDown:

                            if (u.buffModifiers.Count > 0)
                            {
                                foreach (Modifier m in u.buffModifiers)
                                {
                                    if (m.modifierType == TypeOfModifier.Antivirus)
                                    {
                                        u.RemoveBuff(m);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                e.SlowDown(u);
                                u.AddDebuff(new Modifier { modifierType = TypeOfModifier.TimelineSpeed });
                            }                           
                            break;
                        case TypeOfEffect.DefenseDown:
                            if (u.buffModifiers.Count > 0)
                            {
                                foreach (Modifier m in u.buffModifiers)
                                {
                                    if (m.modifierType == TypeOfModifier.Antivirus)
                                    {
                                        u.RemoveBuff(m);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                u.defense -= 5;
                                u.AddDebuff(new Modifier { modifierType = TypeOfModifier.Defense });
                            }
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

        //foreach (PlayerUnit u in controller.targetsInRange)
        //{
        //    if (!u.isNearDeath)
        //    {
        //        u.Default();
        //    }
        //}

        controller.validAbilities.Clear();
        treeRunning = false;


    }
    protected override State OnUpdate() {
        if (owner.controller.turnFinished)
            return State.Success;
        if (treeRunning)
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }
}
