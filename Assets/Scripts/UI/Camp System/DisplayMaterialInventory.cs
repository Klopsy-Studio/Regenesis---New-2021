using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMaterialInventory : MonoBehaviour
{
    public MonsterMaterial a;
    public MonsterMaterial b;
   
    public void AddConsumables()
    {
        inventory.AddMonsterMaterial(a, 2);
        inventory.AddMonsterMaterial(b, 1);
     
    }
    public GameObject slotPrefab;
    public MaterialInventory inventory;

    Dictionary<MonsterMaterialSlot, GameObject> materialDisplayed = new Dictionary<MonsterMaterialSlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }


    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    private void CreateDisplay()
    {
        for (int i = 0; i < inventory.materialContainer.Count; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.materialContainer[i].material.sprite;
            materialDisplayed.Add(inventory.materialContainer[i], obj);
        }
    }


    private void UpdateDisplay()
    {
        for (int i = 0; i < inventory.materialContainer.Count; i++)
        {
            if (materialDisplayed.ContainsKey(inventory.materialContainer[i]))
            {

                materialDisplayed[inventory.materialContainer[i]].GetComponentInChildren<Text>().text = inventory.materialContainer[i].amount.ToString();
            }
            else
            {
                var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.materialContainer[i].material.sprite;
                obj.GetComponentInChildren<Text>().text = inventory.materialContainer[i].amount.ToString();
                materialDisplayed.Add(inventory.materialContainer[i], obj);
            }

        }
    }
}
