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

    public void CreatePopUpBuffIndicator(Modifier modifier)
    {
        var textPopUpGameobject = Instantiate(popUpText, transform.position, popUpText.transform.rotation);
        PopUpText textPopUp = textPopUpGameobject.GetComponentInChildren<PopUpText>();
        textPopUp.SetUpBuff(modifier.modifierType);
    }

    public void CreatePopUpDebuffIndicator(Modifier modifier)
    {
        var textPopUpGameobject = Instantiate(popUpText, transform.position, popUpText.transform.rotation);
        PopUpText textPopUp = textPopUpGameobject.GetComponentInChildren<PopUpText>();
        textPopUp.SetUpDebuff(modifier.modifierType);
    }

    public void CreateRemoveBuffIndicator(Modifier modifier)
    {
        var textPopUpGameobject = Instantiate(popUpText, transform.position, popUpText.transform.rotation);
        PopUpText textPopUp = textPopUpGameobject.GetComponentInChildren<PopUpText>();
        textPopUp.SetUpRemoveBuff(modifier.modifierType);
    }
    public void CreateRemoveDebuffIndicator(Modifier modifier)
    {
        var textPopUpGameobject = Instantiate(popUpText, transform.position, popUpText.transform.rotation);
        PopUpText textPopUp = textPopUpGameobject.GetComponentInChildren<PopUpText>();
        textPopUp.SetUpRemoveDebuff(modifier.modifierType);
    }

}
