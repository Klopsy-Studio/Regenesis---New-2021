using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LootUIState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("LOOT UI STATE");

        StartCoroutine(ActivateLootUI());

    }

    IEnumerator ActivateLootUI()
    {
        yield return new WaitForSeconds(1);
        var monsterDrops = owner.lootSystem;
        monsterDrops.DropMaterials();
        var materialsDropped = monsterDrops.droppedMaterials;
      

        owner.lootUIManager.gameObject.SetActive(true);
        StartCoroutine(owner.lootUIManager.LootSequence(materialsDropped));
    }

    public void StartReturn()
    {
        StartCoroutine(ReturnToCampSequence());
    }

    public IEnumerator ReturnToCampSequence()
    {
        owner.turnStatusUI.gear.gameObject.SetActive(false);
        ActionEffect.instance.BlackAndWhite();

        yield return new WaitForSeconds(1f);

        owner.questComplete.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        owner.ReturnToCamp();
    }
}
