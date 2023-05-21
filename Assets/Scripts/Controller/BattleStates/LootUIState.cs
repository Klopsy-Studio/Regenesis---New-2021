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
      

        owner.VictoryLootUIManager.gameObject.SetActive(true);
        StartCoroutine(owner.VictoryLootUIManager.LootSequence(materialsDropped));
    }

    public void VictoryReturnToCamp() //Unity button
    {
        StartCoroutine(VictoryReturnToCampSequence());
    }

    public IEnumerator VictoryReturnToCampSequence()
    {
        owner.turnStatusUI.gear.gameObject.SetActive(false);
        ActionEffect.instance.BlackAndWhite();
        owner.levelData.CompleteHunt();
        yield return new WaitForSeconds(1f);

        owner.questComplete.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        DataPersistenceManager.instance.SaveGame();

        owner.ReturnToCamp();
    }

   

}
