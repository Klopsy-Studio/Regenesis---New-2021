using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BestiaryManager : MonoBehaviour
{
    [SerializeField] Image monsterImage;
    [SerializeField] TextMeshProUGUI monsterName;
    [SerializeField] TextMeshProUGUI monsterHabitat;
    [SerializeField] List<TextMeshProUGUI> monsterMaterials;
    [SerializeField] TextMeshProUGUI monsterPhisology;
    [SerializeField] TextMeshProUGUI monsterBehaviour;

    [SerializeField] BeastInfo[] monsters;
    int index = 0;
    public void SetMonster(BeastInfo info)
    {
        monsterImage.sprite = info.monsterImage;
        monsterImage.SetNativeSize();
        monsterName.SetText(info.monsterName);
        monsterHabitat.SetText(info.monsterHabitat);


        for (int i = 0; i < monsterMaterials.Count; i++)
        {
            monsterMaterials[i].SetText(info.monsterMaterials[i]);
        }

        monsterPhisology.SetText(info.monsterPhisiology);
        monsterBehaviour.SetText(info.monsterBehaviour);
    }

    private void Start()
    {
        SetMonster(monsters[0]);
        index = 0;
    }

    public void ShowNextMonster()
    {
        index++;
        if(index>= monsters.Length)
        {
            index = 0;
        }

        SetMonster(monsters[index]);
    }

    public void ShowPreviousMonster()
    {
        index--;
        if (index <0)
        {
            index = monsters.Length-1;
        }

        SetMonster(monsters[index]);
    }
}
