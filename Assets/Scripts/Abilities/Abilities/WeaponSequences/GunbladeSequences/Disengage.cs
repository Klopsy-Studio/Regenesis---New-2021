using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/Disengage")]

public class Disengage : AbilitySequence
{
    [SerializeField] int ammoGain;
    [SerializeField] int moveLength;

    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        playing = true;
        user = controller.currentUnit;
        user.SpendActionPoints(ability.actionCost);
        user.IncreaseBullets(ammoGain);

        Tile t;

        if (target.GetComponent<Unit>())
        {
            t = target.GetComponent<Unit>().tile;
        }

        else if (target.GetComponent<BearObstacleScript>())
        {
            t = controller.board.GetTile(target.GetComponent<BearObstacleScript>().pos);
        }
        else
        {
            t = null;
        }

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if(target != null)
        {
            if (target.GetComponent<Unit>())
            {
                Unit u = target.GetComponent<Unit>();

                Attack(u);
            }

            else if (target.GetComponent<BearObstacleScript>())
            {
                BearObstacleScript b = target.GetComponent<BearObstacleScript>();
                b.GetDestroyed(controller.board);
                user.Attack();
            }

        }
        
        

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        

        if(t != null)
        {
            controller.PlayCorotuine(MoveInADirection(t.GetDirections(user.tile), controller, moveLength));

            while (moving)
            {
                yield return null;
            }
        }

        playing = false;
    }
}
