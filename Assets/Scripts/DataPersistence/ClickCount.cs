using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickCount : MonoBehaviour, IDataPersistence
{
    int count = 0;
    TextMeshProUGUI clickCount;

    public void LoadData(GameData data)
    {
        count = data.clickCount;
    }

    public void SaveData(GameData data)
    {
       data.clickCount = this.count;
    }

    private void Awake()
    {
        clickCount = GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        clickCount.text = count.ToString();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            count++;
         
            clickCount.text = count.ToString();
        }
    }
}
