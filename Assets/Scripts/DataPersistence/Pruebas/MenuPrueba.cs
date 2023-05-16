using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrueba : MonoBehaviour
{
    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            //DESACTIVAR EL BOTÓN DE CONTINUAR
        }
    }
    public void OnNewGameClicked()
    {
        //create a new game - initialize game data
        DataPersistenceManager.instance.NewGame();
      
        Debug.Log("has new game data " + DataPersistenceManager.instance.HasGameData());
        //load the gameplay scene - whick will in turn save the game because of 
     
        SceneManager.LoadSceneAsync("SaveSystemScene");
    }

    public void OnContinueGameClicked()
    {
        //save the game anytime before loading a scene
     
        DataPersistenceManager.instance.SaveGame();

        //Load the next scene - which will in turn load the game because of 
        //OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("SaveSystemScene");
    }
}
