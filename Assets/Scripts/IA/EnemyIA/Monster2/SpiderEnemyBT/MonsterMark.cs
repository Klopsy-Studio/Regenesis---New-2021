using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class MonsterMark : ActionNode
{
    [SerializeField] Modifier monsterMark = new Modifier { modifierType = TypeOfModifier.SpiderMark};

    bool canGetMarked;
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
        canGetMarked = true;
        BattleController controller = owner.controller.battleController;
        Unit chosenTarget = controller.playerUnits[Random.Range(0, controller.playerUnits.Count)];
        
        if(chosenTarget.GetComponent<PlayerUnit>()!= null)
        {
            PlayerUnit u = chosenTarget.GetComponent<PlayerUnit>();
            if (u.buffModifiers.Count > 0)
            {
                foreach (Modifier m in u.buffModifiers)
                {
                    if (m.modifierType == TypeOfModifier.Antivirus)
                    {
                        u.RemoveBuff(m);
                        canGetMarked = false;
                        break;
                    }
                }
            }


            if (canGetMarked)
            {
                u.marked = true;
                u.EnableCriticalMark();
                monsterMark.modifierType = TypeOfModifier.SpiderMark;
                u.AddDebuff(monsterMark);
            }
            
        }

        //Action Effect
        owner.controller.monsterAnimations.SetBool("mark", true);
        owner.controller.monsterAnimations.SetBool("idle", false);

        yield return new WaitForSeconds(0.5f);
        controller.SelectTile(chosenTarget.tile.pos);
        AudioManager.instance.Play("HunterMark");
        ActionEffect.instance.Play(3, 0.5f, 0.01f, 0.05f);

        while (ActionEffect.instance.CheckActionEffectState())
        {
            yield return null;
        }

        controller.SelectTile(owner.controller.currentEnemy.tile.pos);
        owner.controller.monsterAnimations.SetBool("mark", false);

        owner.controller.monsterAnimations.SetBool("idle", true);
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
