using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineIconHighlight : MonoBehaviour
{
    [SerializeField] private Image imageCmp;
    [SerializeField] private AnimationCurve curve;
    
    private float targetAlpha;
    private float currentTime;


    private void Awake()
    {
        imageCmp = GetComponent<Image>();
    }


    private void Update()
    {
        targetAlpha = curve.Evaluate(currentTime);
        imageCmp.color = new Vector4(imageCmp.color.r, imageCmp.color.g, imageCmp.color.b, targetAlpha);

        currentTime += Time.deltaTime;
    }
}
