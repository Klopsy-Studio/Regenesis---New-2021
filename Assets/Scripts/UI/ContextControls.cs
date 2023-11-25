using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextControls : MonoBehaviour
{

    [SerializeField] List<ContextControlWindow> windows;
    ContextControlWindow currentActiveWindow;

    public ContextControlWindow GetWindow(string id)
    {
        foreach(ContextControlWindow w in windows)
        {
            if(w.GetID() == id)
            {
                return w;
            }
        }

        Debug.Log("No window was found");
        return null;
    }

    public void ChangeCurrentWindow(string id)
    {
        ContextControlWindow newWindow = GetWindow(id);

        if(newWindow == null)
        {
            return;
        }

        if(currentActiveWindow != null)
        {
            if(currentActiveWindow != newWindow)
            {
                DeactivateCurrentWindow();
                currentActiveWindow = newWindow;
                newWindow.GetWindowObject().SetActive(true);          
            }
        }
        else
        {
            currentActiveWindow = newWindow;
            newWindow.GetWindowObject().SetActive(true);
        }
    }

    public void DeactivateCurrentWindow()
    {
        if (currentActiveWindow == null)
            return;

        currentActiveWindow.GetWindowObject().SetActive(false);
        currentActiveWindow = null;
    }
}

[System.Serializable]
public class ContextControlWindow
{
    [SerializeField] string windowID;
    [SerializeField] GameObject windowObject;

    public string GetID()
    {
        return windowID;
    }

    public GameObject GetWindowObject()
    {
        return windowObject;
    }
}
