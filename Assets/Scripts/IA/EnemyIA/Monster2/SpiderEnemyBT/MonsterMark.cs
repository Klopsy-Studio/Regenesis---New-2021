using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterMark : ActionNode
{
    [SerializeField] DamageModifier monsterMark;


    bool treeUpdate;
    protected override void OnStart() {
        treeUpdate = true;
        owner.controller.StartCoroutine(MarkHunter());
    }

    protected override void OnStop() {
    }

    IEnumerator MarkHunter()
    {
        //First we randomly select the hunter which will receive the mark
        BattleController controller = owner.controller.battleController;
        Unit chosenTarget = controller.playerUnits[Random.Range(0, controller.playerUnits.Count)];
        chosenTarget.damageIncrease.Add(monsterMark);
        
        if(chosenTarget.GetComponent<PlayerUnit>()!= null)
        {
            chosenTarget.GetComponent<PlayerUnit>().marked = true;
        }

        //Action Effect
        controller.SelectTile(chosenTarget.tile.pos);
        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        controller.SelectTile(owner.controller.currentEnemy.tile.pos);

        yield return new WaitForSeconds(0.5f);

        treeUpdate = false;

    }
    protected override State OnUpdate() {

        if (treeUpdate)
        {
            return State.Running;
        }
        else
        {
            return State.Success;

        }
    }
}
