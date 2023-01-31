using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpText : MonoBehaviour
{
    private TextMeshPro textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }
  
    public void SetUp(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
    }
    public void DestroyPopUp()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
}
