using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    public GameObject popUpText;

    
    public void CreatePopUpText(Vector3 position, int dmgAmount)
    {
        var textPopUpGameobject = Instantiate(popUpText, position, Quaternion.identity);
        PopUpText textPopUp = textPopUpGameobject.GetComponentInChildren<PopUpText>();
      
        textPopUp.SetUp(dmgAmount);
        Debug.Log("Pop up :(");
    }
   
}
