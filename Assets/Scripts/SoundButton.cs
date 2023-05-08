using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] string soundNumber;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(AudioManager.instance != null)
        {
            AudioManager.instance.Play("Boton" + soundNumber);
        }
      
    }
}
