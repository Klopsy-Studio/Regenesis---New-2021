using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataifNull = false;
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Destroyed the newest one");
            Destroy(this.gameObject);
            return;

        }

        instance = this;
        DontDestroyOnLoad(this);
        //Application.persistentDataPath give the standard directory for a Unity Game
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
      

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
       
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

   
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        //var dataPersistence = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        //THE SCRIPTS NEED TO EXTENDS FROM MONOBEHAVIOR.
        //FindObjectsOfType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersitence = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersitence);
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        //Start a new game if the data is null and we have configured to initialize data for debugging
        if(this.gameData == null && initializeDataifNull)
        {
            NewGame(); 
        }
        //If no data can be loaded, initializa to a new game
        if(this.gameData == null)
        {
            Debug.Log("no data was found. A NEW GAME needs to be started before data can be loaded");
            return;
        }

        //push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
     
    }

    public void SaveGame()
    {
        //if we dont have any data to save, log a warning here 
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found. A NEW GAME needs to be started before data can be saved");
            return;
        }

        //pass the data to other scripts so they can update it

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }


        // save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public bool HasData()
    {
        return gameData != null; 
    }
}
