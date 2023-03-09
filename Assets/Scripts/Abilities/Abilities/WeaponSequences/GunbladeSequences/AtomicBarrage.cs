using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/Atomic Barrage")]

public class AtomicBarrage : AbilitySequence
{
    public override IEnumerator Sequence(List<Tile> tiles, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.currentTargets.Clear();
        user.abilityTiles.Clear();
        user.abilityTiles = tiles;
        user.currentAbility = ability;
        user.SpendActionPoints(ability.actionCost);
        user.SpendBullets(ability.ammoCost);

        List<GameObject> targets = new List<GameObject>();
        
        foreach(Tile t in tiles)
        {
            if(t.content != null)
            {
                if (!targets.Contains(t.content))
                {
                    targets.Add(t.content);
                }
            }

            if (t.occupied)
            {
                if (!targets.Contains(controller.enemyUnits[0].gameObject))
                {
                    targets.Add(controller.enemyUnits[0].gameObject);
                }
            }
        }

        user.currentTargets = targets;
        user.animations.unitAnimator.SetFloat("attackIndex", 0.2f);
        user.animations.unitAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(2f);

        //if(targets!= null)
        //{
        //    if(targets.Count > 0)
        //    {
        //        foreach(GameObject o in targets)
        //        {
        //            if (o.GetComponent<Unit>())
        //            {
        //                Attack(o.GetComponent<Unit>());
        //            }
        //            else if (o.GetComponent<BearObstacleScript>())
        //            {
        //                o.GetComponent<BearObstacleScript>().GetDestroyed(controller.board);
        //            }
        //        }
        //    }
        //}


        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
