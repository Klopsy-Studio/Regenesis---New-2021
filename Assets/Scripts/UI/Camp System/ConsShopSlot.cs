using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ConsShopSlot : MonoBehaviour
{
    public Image consumableImage;
    public TextMeshProUGUI amountText;

    public void SetVariables(ConsumableSlot _consumableSlot)
    {
        consumableImage.sprite = _consumableSlot.consumable.iconSprite;
        amountText.SetText(_consumableSlot.amount.ToString());
    }
}
