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

    public ConsumableBackpack TUT_consumbaleBackpack;
    public static GameManager instance;

    public LevelData currentMission;

   /* [HideInInspector] */public UnitProfile[] unitProfilesList;
    /* [HideInInspector] */
    public UnitProfile[] unitProfilesTutorialList;

    public GameObject unitsPrefab;

    public float previousMusicSliderValue = 1f;
    public float previousSfxSliderValue = 1f;

    public bool gameIsPaused;
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

    public void RecordSliderValues(float sfxValue, float musicValue)
    {
        previousSfxSliderValue = sfxValue;
        previousMusicSliderValue = musicValue;
    }
    
 
}
