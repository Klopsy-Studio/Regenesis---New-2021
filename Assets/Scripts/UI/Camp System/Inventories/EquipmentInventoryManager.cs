using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EquipmentInventoryManager : MonoBehaviour
{
    public EquipmentSlotButton slotPrefab;
    public EquipmentInventory inventory;
   
    [HideInInspector] public Dictionary<WeaponSlot, EquipmentSlotButton> consumableDisplayed = new Dictionary<WeaponSlot, EquipmentSlotButton>();

    public WeaponPanelInfo weaponPanelInfo;

    [SerializeField] Transform contentTransform;
    public GameObject rightPanel;

    public AbilityTooltip[] abilityTooltipList;

    public delegate void EquipmentButtonSlotClicked();
    public static event EquipmentButtonSlotClicked OnEquipmentButtonCliked;

    //List<EquipmentSlotButton> slotButtonsList = new List<EquipmentSlotButton>();

    public void ButtonClicked()
    {

        OnEquipmentButtonCliked?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
       
        rightPanel.SetActive(false);
    }

    private void OnEnable()
    {
        CreateDisplay();
    }



    // Update is called once per frame
    //void Update()
    //{
    //    UpdateDisplay();
    //}

    private void CreateDisplay()
    {

        //foreach (var item in slotButtonsList)
        //{
        //    slotButtonsList.Remove(item);
        //    Destroy(item.gameObject);
        //}
       
        consumableDisplayed.Clear();
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

            //slotButtonsList.Add(slotButton);

        }
    }

    //private void UpdateDisplay()
    //{
    //    for (int i = 0; i < inventory.container.Count; i++)
    //    {
    //        if (consumableDisplayed.ContainsKey(inventory.container[i]))
    //        {
    //            continue;
    //        }
    //        else
    //        {
    //            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, contentTransform);

    //            var equipmentSlot = inventory.container[i];
    //            obj.SetSlotButton(equipmentSlot);
    //            consumableDisplayed.Add(equipmentSlot, obj);
    //            var weapon = inventory.container[i].weapon;
    //            if (obj.TryGetComponent(out EquipmentSlotButton slotButton))
    //            {

    //                slotButton.SetWeaponSlotInfo(this, weapon);
    //            }
    //        }
    //    }
    //}
   

    private void OnDisable()
    {
        weaponPanelInfo.ResetInfo();
        rightPanel.SetActive(false);
    }



    public void UpdateEquipmentPanelInfo(EquipmentSlotButton slotButton)
    {
        weaponPanelInfo.UpdateInventoryPanelInfo(slotButton);
    }
}
