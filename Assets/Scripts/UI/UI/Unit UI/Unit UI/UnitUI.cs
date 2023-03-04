using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    public GameObject popUpText;

    
    public void CreatePopUpText(Vector3 position, int dmgAmount, bool isCritical)
    {
        var textPopUpGameobject = Instantiate(popUpText, position, popUpText.transform.rotation);
        PopUpText textPopUp = textPopUpGameobject.GetComponentInChildren<PopUpText>();

        if (isCritical)
        {
            textPopUp.SetUpCritical(dmgAmount);
        }
        else
        {
            textPopUp.SetUp(dmgAmount);
        }
        Debug.Log("Pop up :(");
    }
   
}
