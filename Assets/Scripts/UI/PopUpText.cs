using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpText : MonoBehaviour
{
    private TextMeshPro textMesh;
    private Animator textAnim;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textAnim = GetComponent<Animator>();
    }
  
    public void SetUp(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textAnim.SetTrigger("dmg");

    }

    public void SetUpCritical(int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textAnim.SetTrigger("crit");
    }
    public void DestroyPopUp()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
}
