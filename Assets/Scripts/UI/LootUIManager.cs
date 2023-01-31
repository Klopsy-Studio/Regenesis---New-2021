using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LootUIManager : MonoBehaviour
{
    [SerializeField] GameObject lootDropSlot;
    [SerializeField] Transform parentTransform;
    
    
    // Start is called before the first frame update
   
   
    public void UpdateLootUI(/*DropsContainer dropContainer*/DroppedMaterials dropped)
    {
        for (int i = 0; i < dropped.materialContainer.Count; i++)
        {
            var obj = Instantiate(lootDropSlot, Vector3.zero, Quaternion.identity, parentTransform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = dropped.materialContainer[i].material.sprite;
            obj.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(dropped.materialContainer[i].amount.ToString());
        }
       
    } 
}
