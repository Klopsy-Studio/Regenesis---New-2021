using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemTemplate : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCost;


    ShopManager shopManager;
    [HideInInspector] public ShopItemInfo item;
    public Sprite sprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        //shopManager.shopItemSelected = this;
        if (!shopManager.itemPanelInfo.GO.activeSelf) shopManager.itemPanelInfo.GO.SetActive(true);
        shopManager.itemPanelInfo.SetItemInfo(this);

    }

    public void SetItemInfo(ShopItemInfo _itemInfo, ShopManager _shopManager)
    {
        
        this.gameObject.name = _itemInfo.name;
        itemImage.sprite = _itemInfo.consumable.iconSprite;
        itemName.SetText(_itemInfo.name);
        itemCost.SetText(_itemInfo.pointCosts.ToString());

        shopManager = _shopManager;
        item = _itemInfo;
    }
    
}
