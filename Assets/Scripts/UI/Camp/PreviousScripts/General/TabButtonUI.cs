using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButtonUI : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image currentImage;

    public Sprite idleImage;
    public Sprite selectedImage;
    public Sprite hoverImage;

    private void Start()
    {
        currentImage = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
        //currentImage.sprite = selectedImage;
    }
   
    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
}