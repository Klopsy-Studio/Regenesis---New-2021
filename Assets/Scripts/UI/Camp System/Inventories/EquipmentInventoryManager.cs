using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventoryManager : MonoBehaviour
{
    public EquipmentSlotButton slotPrefab;
    public EquipmentInventory inventory;
   
    [HideInInspector] public Dictionary<WeaponSlot, EquipmentSlotButton> consumableDisplayed = new Dictionary<WeaponSlot, EquipmentSlotButton>();

    public WeaponPanelInfo weaponPanelInfo;

    [SerializeField] Transform contentTransform;

    public AbilityTooltip[] abilityTooltipList;
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }


    // Update is called once per frame
    void Update()
    {
        //UpdateDisplay();
    }

    private void CreateDisplay()
    {
        for (int i = 0; i < inventory.container.Count; i++)
        {

            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, contentTransform);
           
            var equipmentSlot = inventory.container[i];
            obj.SetSlotButton(equipmentSlot);
            consumableDisplayed.Add(equipmentSlot, obj);
            var weapon = inventory.container[i].weapon;
            if (obj.TryGetComponent(out EquipmentSlotButton slotButton))
            {

                slotButton.SetWeaponSlotInfo(this,weapon);
            }

        }
    }

    private void OnDisable()
    {
        weaponPanelInfo.ResetInfo();
    }



    public void UpdateEquipmentPanelInfo(EquipmentSlotButton slotButton)
    {
        weaponPanelInfo.UpdateInventoryPanelInfo(slotButton);
    }
}
