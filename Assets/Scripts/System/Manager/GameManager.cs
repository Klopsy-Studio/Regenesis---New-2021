using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MaterialInventory materialInventory;
    public EquipmentInventory equipmentInventory;
    public ConsumableInventory consumableInventory;
    public ConsumableBackpack consumableBackpack;
    public static GameManager instance;

    public LevelData currentMission;

   /* [HideInInspector] */public UnitProfile[] unitProfilesList;

    public GameObject unitsPrefab;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
       
        DontDestroyOnLoad(this.gameObject);
    }


    public void LoadMission()
    {
        SceneManager.LoadScene("Battle");
    }


    public string sceneToLoad;
 
}
