using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static LinkHandlerForTMPTextOnHover;

public class LinkHandlerForTMPTextOnHover : MonoBehaviour
{
    private TMP_Text _tmpTextBox;
    private Canvas _canvasToCheck;
    [SerializeField] private Camera cameraToUse;

    private RectTransform _textBoxRectTransform;

    private int _currentActiveLinkedElement;

    public delegate void HoverOnLinkEvent(string keyword, Vector3 mousePos);
    public static event HoverOnLinkEvent OnHoverOnLinkEvent;

    public delegate void CloseTooltipEvent();
    public static event CloseTooltipEvent OnCloseTooltipEvent;

    private void Awake()
    {
        _tmpTextBox = GetComponent<TMP_Text>();
        _canvasToCheck = GetComponentInParent<Canvas>();
        _textBoxRectTransform = GetComponentInParent<RectTransform>();

        if (_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            cameraToUse = null;
        }
        else
        {
            cameraToUse = _canvasToCheck.worldCamera;
        }
    }
    private void Update()
    {
        CheckForLinkAtMousePos();
    }


    private void CheckForLinkAtMousePos()
    {
        Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);

        bool isIntersectingRectTransform = TMP_TextUtilities.IsIntersectingRectTransform(_textBoxRectTransform, mousePos, cameraToUse); //comprueba primero si el ratón está dentro de la caja de texto
        if (!isIntersectingRectTransform) return;

        int intersectingLink = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePos, cameraToUse);

        Debug.Log("currentActiveLink " + _currentActiveLinkedElement + " intersectnglink es " + intersectingLink);
        if (_currentActiveLinkedElement != intersectingLink) 
        {

            Debug.Log("ha entrado a OnCloseToolTipEvent");
            OnCloseTooltipEvent?.Invoke();
        
        }
        if (intersectingLink == -1) return;

        TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[intersectingLink];

        OnHoverOnLinkEvent?.Invoke(linkInfo.GetLinkID(), mousePos);

        _currentActiveLinkedElement = intersectingLink;
    
    }
}
