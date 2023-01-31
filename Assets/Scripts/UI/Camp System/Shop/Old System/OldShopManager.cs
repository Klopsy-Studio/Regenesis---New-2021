using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldShopManager : MonoBehaviour
{
    public OldShopItem[] shopItems;
    public OldShopTemplate[] shopPanels;
    public GameObject[] shopPanelsGO;

    public Button[] myPurchaseButton;
    public int coins;
    public Text coinUI;

  
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }

        coinUI.text = coins.ToString();
        LoadPanels();
        CheckPurchaseable();
    }
    
    public void AddCoins()
    {
        coins+=10;
        coinUI.text = coins.ToString();
        CheckPurchaseable();
    }

    private void CheckPurchaseable()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (coins >= shopItems[i].baseCost)//If I have enough money
            {
                myPurchaseButton[i].interactable = true;
            }
            else
            {
                myPurchaseButton[i].interactable = false;
            }
        }
    }

    public void PurchaseItem(int itemIndex)
    {
        if (coins >= shopItems[itemIndex].baseCost)
        {
            coins = coins - shopItems[itemIndex].baseCost;
            coinUI.text = coins.ToString();
            CheckPurchaseable();
            //UNLOCK ITEM;
        }
    }
    public void LoadPanels()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItems[i].title;
            shopPanels[i].descriptionTxt.text = shopItems[i].description;
            shopPanels[i].costTxt.text = shopItems[i].baseCost.ToString();
        }
    }
}
