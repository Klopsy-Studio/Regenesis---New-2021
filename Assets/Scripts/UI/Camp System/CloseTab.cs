using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseTab : MonoBehaviour
{
    [SerializeField] CanvasScaler canva;
    [SerializeField] GameObject window;
    Vector2 defaultResolution = new Vector2(1920, 1080);
    public void CloseWindow() //Unity button function
    {
        window.gameObject.SetActive(false);
        canva.referenceResolution = defaultResolution;

    }
}
