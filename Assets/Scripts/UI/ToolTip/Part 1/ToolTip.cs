using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[ExecuteInEditMode()]
public class ToolTip : MonoBehaviour
{
    [SerializeField] float offset;
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;

    public int characterWrapLimit;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        //layoutElement.enabled = (headerLength>characterWrapLimit || contentLength > characterWrapLimit) ? true:false;
        layoutElement.enabled = Mathf.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;
    } 

 
    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            //layoutElement.enabled = (headerLength>characterWrapLimit || contentLength > characterWrapLimit) ? true:false;
            layoutElement.enabled = Mathf.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;
        }

        //Vector2 mousePosition = Input.mousePosition;
        //float pivotX = mousePosition.x / Screen.width;
        //float pivotY = mousePosition.y / Screen.height;

        //rectTransform.pivot = new Vector2(pivotX, pivotY);
        //Debug.Log("mousePositionX " + mousePosition.x + " Screen.width es " + Screen.width);
        //transform.position = mousePosition;

        Vector2 mousePosition = Input.mousePosition;
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;

        float finalPivotX = 0f;
        float finalPivotY = 0f;

        if (pivotX < 0.5) //If mouse on left of screen move tooltip to right of cursor and vice vera
        {
            finalPivotX = -0.1f;
        }

        else
        {
            finalPivotX = 1.01f;
        }



        if (pivotY < 0.5) //If mouse on lower half of screen move tooltip above cursor and vice versa
        {
            finalPivotY = 0;
        }

        else
        {
            finalPivotY = 1;
        }


        rectTransform.pivot = new Vector2(finalPivotX, finalPivotY);


        transform.position = mousePosition;
    }
}
