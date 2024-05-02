using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public void SaveGameFunction()
    {
        DataPersistenceManager.instance.SaveGame();
    }
}
