using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTimeline : ItemElements
{
    public SquareAbilityRange itemRange;
    List<Tile> tiles;

    public int damage;
    BattleController battleController;

    [SerializeField] Animator bombAnimator;
   
    bool monsterDamaged;
    public void Init(BattleController bController, Tile t)
    {
        battleController = bController;
        bController.timelineElements.Add(this);
        timelineTypes = TimeLineTypes.Items;

        tile = t;
        currentPoint = t.pos;
        tiles = itemRange.GetTilesInRangeWithoutUnit(battleController.board, tile.pos);
    }



    public override bool UpdateTimeLine()
    {
      
        if (timelineFill >= timelineFull)
        {
            return true;
        }
        timelineFill += fTimelineVelocity * Time.deltaTime;
        return false;
    }

    public override IEnumerator ApplyEffect(BattleController controller)
    {
        yield return new WaitForSeconds(0.5f);
        timelineFill = 0;

        foreach (var t in tiles)
        {
            t.SetSmokeBomb();
            if(t.content != null)
            {
                if (t.content.TryGetComponent(out Unit unit))
                {
                    unit.ReceiveDamage(damage, false);
                    unit.Damage();

                    if(unit.GetComponent<EnemyUnit>() == controller.enemyUnits[0])
                    {
                        monsterDamaged = true;
                    }
                }

                if(t.occupied && !monsterDamaged)
                {
                    Unit monster = controller.enemyUnits[0];
                    monster.ReceiveDamage(50, false);
                    monster.Damage();
                    monster.DamageEffect();
                    monsterDamaged = true;
                }

                if(t.content.GetComponent<BearObstacleScript>() != null)
                {
                    t.content.GetComponent<BearObstacleScript>().GetDestroyed(controller.board);
                }
            }

            if(t.occupied && !monsterDamaged)
            {
                Unit monster = controller.enemyUnits[0];
                monster.ReceiveDamage(30, false);
                monster.Damage();
                monster.DamageEffect();
                monsterDamaged = true;
            }
            
        }
        bombAnimator.SetTrigger("explode");

        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        elementEnabled = false;
        battleController.timelineElements.Remove(this);
        tile.content = null;
    }

    public void Place(Tile target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        // Link unit and tile references
        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    public void Match()
    {
        transform.localPosition = tile.center;
     
        currentPoint = tile.pos;
    }


}
