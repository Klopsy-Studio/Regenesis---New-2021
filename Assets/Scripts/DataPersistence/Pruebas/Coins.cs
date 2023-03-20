using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour, IDataPersistence
{
    public bool isDeactive = false;
    [SerializeField] private string id;

    public void LoadData(GameData data)
    {
        data.coinsCollected.TryGetValue(id, out isDeactive);
        if (!isDeactive)
        {
            gameObject.SetActive(true);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.coinsCollected.ContainsKey(id))
        {
            data.coinsCollected.Remove(id);
        }

        data.coinsCollected.Add(id, isDeactive);
    }

    [ContextMenu("Generate guid for id")]
    private void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void Update()
    {
        if (!isDeactive)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);    
        }
    }
}
