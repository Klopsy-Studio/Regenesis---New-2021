using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] GameObject equipmentInventory;
    [SerializeField] GameObject materialInventory;
    [SerializeField] GameObject consumableInventory;

    // Start is called before the first frame update
    void Start()
    {
        materialInventory.SetActive(false);
        consumableInventory.SetActive(false);
        equipmentInventory.SetActive(true);
    }

    

    
}
