using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class LinkHandlerForTMPText : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text _tmpTextBox;
    private Canvas _canvasToCheck;
    [SerializeField] private Camera cameraToUse;

    public delegate void ClickOnLinkEvent(string keyword);
    public static event ClickOnLinkEvent OnClickedOnLinkEvent;

    private void Awake()
    {
        _tmpTextBox = GetComponent<TMP_Text>();
        _canvasToCheck = GetComponentInParent<Canvas>();

        if(_canvasToCheck.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            cameraToUse = null;
        }
        else
        {
            cameraToUse = _canvasToCheck.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePos = new Vector3(eventData.position.x, eventData.position.y, 0);

        int linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePos, cameraToUse);

        if(linkTaggedText != -1)//-1 significa que no ha encontrado nada.
        {
            TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkTaggedText];

            OnClickedOnLinkEvent?.Invoke(linkInfo.GetLinkID());

          
        } 
    }
}
