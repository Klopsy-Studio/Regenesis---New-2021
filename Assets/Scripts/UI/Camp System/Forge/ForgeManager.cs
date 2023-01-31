using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ForgeManager : MonoBehaviour
{
    [SerializeField] WeaponUpgradeSystem weaponUpgradeSystem;
    [SerializeField] Transform transformContent;
    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject weaponSlotPrefab;

    public WeaponPanelInfo weaponPanelInfo;

    [SerializeField] MaterialRequiredSlot[] materialsRequiredSlot;

    //Ponerlo en privado cuando se termine el testeo
    public WeaponInfoTemplate currentWeaponInfoTemplate;

    private void Start()
    {
        CreateDisplay();
    }
    void CreateDisplay()
    {
        //The first FOR is used to create each weapon tree
        for (int i = 0; i < weaponUpgradeSystem.allWeaponsTrees.Length; i++)
        {
            var weaponTree = Instantiate(treePrefab, Vector3.zero, Quaternion.identity, transformContent);
            var weaponTreeTemplate = weaponTree.GetComponent<WeaponTreeTemplate>();
            weaponTreeTemplate.treeNameText.SetText(weaponUpgradeSystem.allWeaponsTrees[i].TreeName);

            //The seconf FOR is used to create the weapon slot inside of the weapon tree
            for (int w = 0; w < weaponUpgradeSystem.allWeaponsTrees[i].weaponUpgrade.Length; w++)
            {
                var weaponSlot = Instantiate(weaponSlotPrefab, Vector3.zero, Quaternion.identity, weaponTreeTemplate.contentTransform);
                //weaponSlot.GetComponent<WeaponInfoTemplate>().weaponName.SetText(weaponUpgradeSystem.allWeaponsTrees[i].weaponUpgrade[w].itemName);
                weaponSlot.GetComponent<WeaponInfoTemplate>().SetWeaponInfo(weaponUpgradeSystem.allWeaponsTrees[i].weaponUpgrade[w], this);
            }
        }
    }

    public void UpdateMaterialRequiredPanel(WeaponUpgrade _weaponUpgrade)
    {
        //Deactivate all materialSlot
        foreach (var material in materialsRequiredSlot)
        {
            material.gameObject.SetActive(false);
        }
        for (int i = 0; i < _weaponUpgrade.materialsRequired.Length; i++)
        {
            //Activate the materialSlot if is needed
            materialsRequiredSlot[i].gameObject.SetActive(true);
            var materialRequired = _weaponUpgrade.materialsRequired[i];
            
            materialsRequiredSlot[i].SetUpMaterialRequiredSlot(materialRequired);
        }
    }

    public void SelectCurrentWeaponPanelInfo(WeaponInfoTemplate _weaponInfoTemplate)
    {
        currentWeaponInfoTemplate = _weaponInfoTemplate;
    }

    public bool CanPurchaseWeapon()
    {
        //Check if enough materials

        //var materialsRequired = currentWeaponInfoTemplate

        //ESTO ESTA MAL revisar weaponinfotemplate
        for (int i = 0; i < currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired.Length; i++)
        {
    
            if (!currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired[i].DoIHaveEnoughMaterial(GameManager.instance.materialInventory))
            {
           
                Debug.Log("no tienes suficiente materiales");
                return false;
            }
        }

        //check if you have the required weapon
        if(currentWeaponInfoTemplate.WeaponUpgrade.weaponRequired != null)
        {
            if (!currentWeaponInfoTemplate.WeaponUpgrade.HasRequiredWeapon(GameManager.instance.equipmentInventory))
            {
                Debug.Log("NO TIENES EL ARMA REQUERIDA");
                return false;
            }
        }

        return true;
    }

    public void ReduceMaterialAndWeapon()
    {
        for (int i = 0; i < currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired.Length; i++)
        {
            currentWeaponInfoTemplate.WeaponUpgrade.materialsRequired[i].ReduceMaterial(GameManager.instance.materialInventory);
        }

        currentWeaponInfoTemplate.WeaponUpgrade.QuitRequiredWeapon(GameManager.instance.equipmentInventory);
        
    }

}

[System.Serializable]
public class WeaponPanelInfo
{
    public TextMeshProUGUI weaponDamage;
    public TextMeshProUGUI weaponRange;
    public TextMeshProUGUI weaponCritic;
    public TextMeshProUGUI weaponDefense;
    public TextMeshProUGUI weaponElementalDefense;
    public TextMeshProUGUI weaponElementalAttack;

    public void UpdatePanelInfo(WeaponInfoTemplate _weaponInfoTemplate)
    {
        weaponDamage.SetText("DMG: "+_weaponInfoTemplate.WeaponDamage.ToString());
        weaponRange.SetText("RNG: "+_weaponInfoTemplate.WeaponRange.ToString());
        weaponCritic.SetText("CRT: "+_weaponInfoTemplate.WeaponCritic.ToString()+"%");
        weaponDefense.SetText("DEF: "+_weaponInfoTemplate.WeaponDefense.ToString());
    }
}
