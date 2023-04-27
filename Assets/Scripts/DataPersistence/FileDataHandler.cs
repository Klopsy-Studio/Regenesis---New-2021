using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    //directory path of where we want to save the data
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        //string fullPath = dataDirPath + "/" + dataFileName; --> use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;

        //Check if the fill actually exists
        if (File.Exists(fullPath))
        {
            try
            {
                //load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //deserialize the data from Json back into the c# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error ocurred when trying lo load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                Debug.Log("ha entrado ha exist, el path es " + fullPath);
                
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to deleye data, but data was not found ath path_ " + fullPath);
            }

        }
        catch(Exception e)
        {
            Debug.LogError("Failed to delete profile data at path: " + fullPath + "\n" + e);
        }
    }

    public void Save(GameData data)
    {

        //string fullPath = dataDirPath + "/" + dataFileName; --> use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            //create the directory if it does not exit yet
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the c# game data object into Json
            string datoToStore = JsonUtility.ToJson(data, true);

            //write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(datoToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("error ocurred when trying to save data to file: "+ fullPath + "\n" +e);
        }
    }
}
