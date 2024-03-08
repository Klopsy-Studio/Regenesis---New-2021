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
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private DialogueText dialogue;
    
    [Header("Parameters")]
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private AnimationCurve imageCurve;

    [SerializeField] private float time;
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private float typingSpeed = 0.2f;

    private float fadeValue;
    private float imageValue;

    private bool show;
    private float finalYPosition;

    [SerializeField] private bool isDisplayingLine = false;

    [SerializeField] private int dialogueIndex = 0;


    private void Start()
    {
        finalYPosition = portrait.rectTransform.anchoredPosition.y;

        lowerVignette.color = new Vector4(255f, 255f, 255f, 0f);
        dialogueText.color = new Vector4(255f, 255f, 255f, 0f);
        portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, -portrait.rectTransform.anchoredPosition.y);
    }

    public void Continue()
    {
        show = true;

        if (!isDisplayingLine)
        {
            if (dialogueIndex == dialogue.dialogueLines.Length)
            {
                Disable();
                return;
            }

            isDisplayingLine = true;
            dialogueText.text = dialogue.dialogueLines[dialogueIndex].line;
            StartCoroutine(DisplayLine(dialogueText.text));
        }
        else
        {
            isDisplayingLine = false;
            dialogueIndex++;
            dialogueText.maxVisibleCharacters = dialogueText.text.Length;
        }
    }

    public void Disable()
    {
        show = false;
        dialogueText.text = "";
        dialogueIndex = 0;
    }


    private void Update()
    {
        if (time < 1f && show)
        {
            time += Time.deltaTime * animationSpeed;
            fadeValue = fadeCurve.Evaluate(time);
            imageValue = imageCurve.Evaluate(time);

            lowerVignette.color = new Vector4(255f, 255f, 255f, fadeValue);
            dialogueText.color = new Vector4(255f, 255f, 255f, fadeValue);

            portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, finalYPosition * imageValue);
        }
        else if (time > 0f && !show)
        {
            time -= Time.deltaTime * animationSpeed;
            fadeValue = fadeCurve.Evaluate(time);
            imageValue = imageCurve.Evaluate(time);

            lowerVignette.color = new Vector4(255f, 255f, 255f, fadeValue);
            dialogueText.color = new Vector4(255f, 255f, 255f, fadeValue);

            portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, finalYPosition * imageValue);
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            Continue();
        }
    }


    private IEnumerator DisplayLine (string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.maxVisibleCharacters++;
            if (dialogueText.maxVisibleCharacters == dialogueText.text.Length)
            {
                isDisplayingLine = false;
                dialogueIndex++;
            }
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void NextLine()
    {
        dialogueIndex++;
        if (dialogueIndex >= dialogue.dialogueLines.Length)
        {
            Disable();
        }
        else
        {
            Continue();
        }
    }


}
