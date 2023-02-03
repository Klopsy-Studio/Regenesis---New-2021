using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
//Esta clase sirve para el inventario defecto en la base para que funcione el panel de descripción
public class ConsSlotButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler 
{
    public Image consumableImage;
    public TextMeshProUGUI amountText;
    public string consName;
    // Start is called before the first frame update

    ConsumableInventoryManager consInventoryManager;
    public void FillVariables(ConsumableInventoryManager _consInventoryManager)
    {

        consInventoryManager = _consInventoryManager;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        consInventoryManager.UpdateConsPanelInfo(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
