using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/Weapon Sequences/Gunblade Sequences/Advancing Slash")]

public class AdvancingSlash : AbilitySequence
{
    
    [SerializeField] int ammoGain;
    [SerializeField] int moveLength;
    public override IEnumerator Sequence(GameObject target, BattleController controller)
    {
        user = controller.currentUnit;
        playing = true;

        //Bullet Increase
        user.SpendActionPoints(ability.actionCost);
        user.IncreaseBullets(ammoGain);

        Tile t;
        Unit u;

        BearObstacleScript b;

        if (target.GetComponent<Unit>())
        {
            t = target.GetComponent<Unit>().tile;
        }
        else if (target.GetComponent<BearObstacleScript>())
        {
            b = target.GetComponent<BearObstacleScript>();
            t = controller.board.GetTile(target.GetComponent<BearObstacleScript>().pos);
        }
        else
        {
            t = null;
        }

        controller.PlayCorotuine(MoveInADirection(user.tile.GetDirections(t), controller, moveLength));

        while (moving)
        {
            yield return null;
        }

        ActionEffect.instance.Play(ability.cameraSize, ability.effectDuration, ability.shakeIntensity, ability.shakeDuration);

        if (target.GetComponent<Unit>() != null)
        {
            u = target.GetComponent<Unit>();
            Attack(u);
        }

        if(target.GetComponent<BearObstacleScript>()!= null)
        {
            b = target.GetComponent<BearObstacleScript>();
            b.GetDestroyed(controller.board);
        }

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        playing = false;
    }
}
