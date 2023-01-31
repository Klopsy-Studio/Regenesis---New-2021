using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MaterialRequiredSlot : MonoBehaviour
{
    [SerializeField] Image materialImage;
    [SerializeField] TextMeshProUGUI requiredMaterial;
    [SerializeField] TextMeshProUGUI currentMaterial;

    [SerializeField] Image sufficientMat;
    [SerializeField] Image insufficientMat;

    MonsterMaterialSlot inventoryMaterial;
    public void SetUpMaterialRequiredSlot(MaterialRequirement _materialRequirement)
    {
        materialImage.sprite = _materialRequirement.monsterMaterial.sprite;
        requiredMaterial.SetText(_materialRequirement.numberOfMaterial.ToString());
        UpdateCurrentMaterialText(_materialRequirement.monsterMaterial);
        CheckIfSufficientMaterial(_materialRequirement);

    }

    void UpdateCurrentMaterialText(MonsterMaterial _material)
    {
        var inventory = GameManager.instance.materialInventory;
        inventoryMaterial = inventory.materialContainer.Find(x=> x.material == _material);
        if(inventoryMaterial != null)
        {
            currentMaterial.SetText(inventoryMaterial.amount.ToString());
        }
        else
        {
            currentMaterial.SetText("0");
        }
  

    }

    void CheckIfSufficientMaterial(MaterialRequirement _materialRequirement)
    {
        int numberOfMaterialNeeded = _materialRequirement.numberOfMaterial;
        int numberOfMaterialInInventory;
        if (inventoryMaterial != null)
        {
            numberOfMaterialInInventory = inventoryMaterial.amount;
        }
        else
        {
            numberOfMaterialInInventory = 0;
        }

        if (numberOfMaterialInInventory >= numberOfMaterialNeeded)
        {
            insufficientMat.gameObject.SetActive(false);
            sufficientMat.gameObject.SetActive(true);
        }
        else if (numberOfMaterialInInventory < numberOfMaterialNeeded) 
        {
            insufficientMat.gameObject.SetActive(true);
            sufficientMat.gameObject.SetActive(false);
        }

        
        
    }
}
