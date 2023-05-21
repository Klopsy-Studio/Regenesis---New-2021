using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
   
    public DropsContainer dropContainer;
    public List<MonsterMaterial> monsterMaterials = new List<MonsterMaterial>();
    public DroppedMaterials droppedMaterials = new DroppedMaterials();
    int ciclo = 0;


    public void DropMaterials()
    {
        Debug.Assert(dropContainer != null, "Drop container is null", gameObject);
       
        if(dropContainer.containerList.Count == 0)
        {
            Debug.Assert(dropContainer.containerList.Count > 0, "drop container list is void. Add info", gameObject);
            return;
        }
        monsterMaterials.Clear();
        //droppedMaterials.materialContainer.Clear();

        while (monsterMaterials.Count < dropContainer.minimumDrops/* droppedMaterials.materialContainer.Count<3*/)
        {
            CalculaNumberOfMatDrops();
            ciclo++;
            Debug.Log("numero de ciclos: " + ciclo);
        }

        AddToMaterialInventory();
    }

    void CalculaNumberOfMatDrops()
    {


        monsterMaterials.Clear();
        droppedMaterials.materialContainer.Clear();
        //droppedMaterials.materialContainer.Clear();


        foreach (var drop in dropContainer.containerList)
        {
            int testInt = 0;
            int rolls = (int)drop.monsterMaterial.rarity;
            
            Debug.Log("rolls " + rolls);
            for (int i = 0; i < rolls; i++)
            {
                var random =  Random.value * 100;
                bool isDropSuccessful = random <= drop.dropProbabilty;
                testInt++;
                if (isDropSuccessful)
                {
                    monsterMaterials.Add(drop.monsterMaterial);
                    //droppedMaterials.AddMonsterMaterial(drop.monsterMaterial, 1);
                }
            }



        }



        //foreach (var drop in dropContainer.containerList)
        //{
        //    droppedMaterials.AddMonsterMaterial(drop.monsterMaterial, 0);
        //}
        foreach (var material in monsterMaterials)
        {
            Debug.Log("materials name: " + material.name);
            droppedMaterials.AddMonsterMaterial(material, 1);
        }


    }

    void AddToMaterialInventory()
    {
        //foreach (var material in droppedMaterials.materialContainer)
        //{
        //    //if (material.amount <= 0) continue;
        //    //GameManager.instance.materialInventory.AddMonsterMaterial(material.material, material.amount);


        //}

        foreach (var material in monsterMaterials)
        {
            GameManager.instance.materialInventory.AddMonsterMaterial(material, 1);
        }

      
    }

    
}

[System.Serializable]
public class DroppedMaterials
{
    public List<MonsterMaterialSlot> materialContainer = new List<MonsterMaterialSlot>();
    public void AddMonsterMaterial(MonsterMaterial _material, int _amount)
    {
        bool hasMaterial = false;
        for (int i = 0; i < materialContainer.Count; i++)
        {
            if (materialContainer[i].material == _material)
            {
                materialContainer[i].AddAmount(_amount);
                hasMaterial = true;
                break;
            }
        }
        if (!hasMaterial)
        {
            materialContainer.Add(new MonsterMaterialSlot(_material, _amount));
        }
    }
}

