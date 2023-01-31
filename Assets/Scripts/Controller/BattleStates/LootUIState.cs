using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        owner.lootUIManager.UpdateLootUI(materialsDropped);
    }
}
