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
    [SerializeField] private AnimationCurve curve;

    [SerializeField] private float time;
    [SerializeField] private float speed = 1f;

    [SerializeField] private float huh;
    [SerializeField] private float value;

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
            value = curve.Evaluate(time);

            lowerVignette.color = new Vector4(255f, 255f, 255f, value);
            text.color = new Vector4(255f, 255f, 255f, value);

            portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, finalYPosition * value);
        }
        else if (time > 0f && !show)
        {
            time -= Time.deltaTime * speed;
            value = curve.Evaluate(time);

            lowerVignette.color = new Vector4(255f, 255f, 255f, value);
            text.color = new Vector4(255f, 255f, 255f, value);

            portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, finalYPosition * value);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            Enable();
        }
        else if (Input.GetKeyDown(KeyCode.D))
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
