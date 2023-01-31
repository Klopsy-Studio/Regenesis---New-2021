using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BunkhouseUnitManager : MonoBehaviour
{

    public delegate void ClickAction();
    public static event ClickAction changeUnitWeaponID;


    public DisplayEquipmentBunkhouse openSelectWeaponPanel;
    [SerializeField] GameObject gameObjectSelectWeaponPanel;


    public SetWeaponInfoText[] weaponsInfo;

    [Header("Unit Panel Info")]
    public Image weaponIMG;
    public Text movimiento;
    public Text defensa;
    public Text defElemental;
    public Text poder;
    public Text critico;
    public Text ataqElemental;

   

    private void Start()
    {
       
        UpdateDefaultWeaponPanel();
    }
    public void UpdateDefaultWeaponPanel()
    {

        for (int i = 0; i < GameManager.instance.unitProfilesList.Length; i++)
        {
            weaponsInfo[i].SetWeaponText(GameManager.instance.unitProfilesList[i].unitWeapon);
        }
    }
  
    public void FillUnitVariables(int id) //unity button (post its)
    {
        
        var unitProfile = GameManager.instance.unitProfilesList[id];

        openSelectWeaponPanel.SetUnitProfileID(id);
        if (changeUnitWeaponID != null) changeUnitWeaponID();
        weaponIMG.sprite = unitProfile.unitWeapon.Sprite;
        movimiento.text = unitProfile.unitWeapon.range.ToString();
        defensa.text = unitProfile.unitWeapon.Defense.ToString();
        defElemental.text = unitProfile.unitWeapon.WeaponDefenseElement.ToString();
        poder.text = unitProfile.unitWeapon.Power.ToString();
        critico.text = unitProfile.unitWeapon.CriticalPercentage.ToString();
        ataqElemental.text = unitProfile.unitWeapon.ElementPower.ToString();

    }

    public void UpdateWeaponIMG(int unitID)
    {
        var unitProfile = GameManager.instance.unitProfilesList[unitID];
        weaponIMG.sprite = unitProfile.unitWeapon.Sprite;
        Debug.Log("se ha llamado a updateWeaponIMG");
    }

    

    public void OpenSelectWeapon() //KIT Equipado Button

    {
        if (gameObjectSelectWeaponPanel != null)
        {

            gameObjectSelectWeaponPanel.SetActive(true);
        }
        else
        {
            Debug.Log("OpenSelectWeaponPanel es null");
        }
    }

    
}

[System.Serializable]
public class SetWeaponInfoText
{
    public string unitName;
    public Image weaponIMG;
    public TextMeshProUGUI range;
    public TextMeshProUGUI def;
    public TextMeshProUGUI eleDef;
    public TextMeshProUGUI power;
    public TextMeshProUGUI crit;
    public TextMeshProUGUI elePower;
    public void SetWeaponText(Weapons weapon)
    {
        weaponIMG.sprite = weapon.Sprite;
        range.SetText(weapon.range.ToString());
        def.SetText(weapon.Defense.ToString());
        eleDef.SetText(weapon.WeaponDefenseElement.ToString());
        power.SetText(weapon.Power.ToString());
        crit.SetText(weapon.CriticalPercentage.ToString());
        elePower.SetText(weapon.WeaponDefenseElement.ToString());
    }
}
