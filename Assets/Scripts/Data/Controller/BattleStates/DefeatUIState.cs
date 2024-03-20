using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatUIState : BattleState
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

        //Habría que borrar este código
        var monsterDrops = owner.lootSystem;
        monsterDrops.DropMaterials();
        var materialsDropped = monsterDrops.droppedMaterials;

      
        owner.DefeatLootUIManager.gameObject.SetActive(true);
        StartCoroutine(owner.DefeatLootUIManager.LootDefeat());
    }

  

    public void DefeatReturnToCamp() //Unity button
    {
        StartCoroutine(DefeatReturnToCampSequence());
    }

    public IEnumerator DefeatReturnToCampSequence()
    {
        owner.turnStatusUI.gear.gameObject.SetActive(false);
        ActionEffect.instance.BlackAndWhite();

        yield return new WaitForSeconds(1f);

        owner.questFailed.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);



        owner.ReturnToCamp();
    }
}
