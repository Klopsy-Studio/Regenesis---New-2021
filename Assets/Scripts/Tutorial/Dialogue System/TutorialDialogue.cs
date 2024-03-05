using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image lowerVignette;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI text;
    
    [Header("Parameters")]
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private AnimationCurve imageCurve;

    [SerializeField] private float time;
    [SerializeField] private float speed = 1f;

    [SerializeField] private float fadeValue;
    [SerializeField] private float imageValue;

    private bool show;
    private float finalYPosition;

    private string currentText;


    private void Start()
    {
        currentText = text.text;
        finalYPosition = portrait.rectTransform.anchoredPosition.y;

        // lowerVignette.color = new Vector4(255f, 255f, 255f, 0f);
        // portrait.color = new Vector4(255f, 255f, 255f, 0f);
    }


    private void Update()
    {
        if (time < 1f && show)
        {
            time += Time.deltaTime * speed;
            fadeValue = fadeCurve.Evaluate(time);
            imageValue = imageCurve.Evaluate(time);

            lowerVignette.color = new Vector4(255f, 255f, 255f, fadeValue);
            text.color = new Vector4(255f, 255f, 255f, fadeValue);

            portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, finalYPosition * imageValue);
        }
        else if (time > 0f && !show)
        {
            time -= Time.deltaTime * speed;
            fadeValue = fadeCurve.Evaluate(time);
            imageValue = imageCurve.Evaluate(time);

            lowerVignette.color = new Vector4(255f, 255f, 255f, fadeValue);
            text.color = new Vector4(255f, 255f, 255f, fadeValue);

            portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, finalYPosition * imageValue);
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            Enable();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Disable();
        }
    }

    public void Enable()
    {
        show = true;
    }

    public void Disable()
    {
        show = false;
    }
}
