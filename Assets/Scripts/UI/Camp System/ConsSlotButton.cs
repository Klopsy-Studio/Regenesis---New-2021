using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//Esta clase sirve para el inventario defecto en la base para que funcione el panel de descripción
public class ConsSlotButton : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField] Image consumableImage;
    [SerializeField] TextMeshProUGUI amountText;
    public string consName;
    public string description;
    // Start is called before the first frame update

    ConsumableInventoryManager consInventoryManager;

    public void SetSlotButton(ConsumableSlot _consumableSlot, ConsumableInventoryManager _consInventoryManager)
    {
        consumableImage.sprite = _consumableSlot.consumable.iconSprite;
        amountText.SetText(_consumableSlot.amount.ToString());
        consName = _consumableSlot.consumable.name;
        description = _consumableSlot.consumable.consumableDescription;
        consInventoryManager = _consInventoryManager;
    }
  

    public void OnPointerClick(PointerEventData eventData)
    {
        consInventoryManager.UpdateConsPanelInfo(this);
    }

}
