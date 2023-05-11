using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Hammer Sequences/Battlecry")]

public class Battlecry : AbilitySequence
{
    public Modifier battlecryData;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        CleanTargets();

        playing = true;
        user.SpendActionPoints(ability.actionCost);

        user.currentAbility = ability;
        //ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        AbilityRange r = ability.abilityRange[0].GetOrCreateRange(ability.abilityRange[0].range, user.gameObject);
        r.unit = user;

        List<Tile> tiles = r.GetTilesInRange(controller.board);
        List<GameObject> units = new List<GameObject>();

        foreach(Tile t in tiles)
        {
            if(t.content != null)
            {
                if(t.content.GetComponent<PlayerUnit>() != null)
                {
                    units.Add(t.content);
                }
            }
        }

        user.currentTargets = units;
        user.currentModifier = battlecryData;

        user.animations.unitAnimator.SetTrigger("attack");
        user.animations.unitAnimator.SetFloat("attackIndex", 0.6f);

        yield return new WaitForSeconds(0.5f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        foreach(GameObject o in units)
        {
            o.GetComponent<Unit>().AddBuff(battlecryData);
        }

        yield return new WaitForSeconds(1f);        
        playing = false;
    }
}
