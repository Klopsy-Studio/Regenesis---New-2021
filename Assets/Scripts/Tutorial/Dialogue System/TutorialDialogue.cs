using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    [SerializeField] private DialogueText dialogue;
    
    [Header("References")]
    [SerializeField] private Image lowerVignette;
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Sprite continueSprite;
    [SerializeField] private Image continueIndicator;

    
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

    private Coroutine displayLine;

    bool dialogueEnd;
    private void Start()
    {
        finalYPosition = portrait.rectTransform.anchoredPosition.y;

        lowerVignette.color = new Vector4(lowerVignette.color.r, lowerVignette.color.g, lowerVignette.color.b, 0f);
        dialogueText.color = new Vector4(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0f);
        portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, -portrait.rectTransform.anchoredPosition.y);

        ShowContinueIndicator(false);

        StartCoroutine(FirstContinue());
    }


    private IEnumerator FirstContinue()
    {
        yield return new WaitForSeconds(0.5f);
        Continue();
    }


    public void Continue()
    {
        if (dialogueEnd)
            return;
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
            portrait.sprite = dialogue.dialogueLines[dialogueIndex].portrait;
            portrait.SetNativeSize();

            ShowContinueIndicator(false);

            if(displayLine != null)
                StopCoroutine(displayLine);

            displayLine = StartCoroutine(DisplayLine(dialogueText.text));
        }
        else
        {
            LineComplete();
            if (displayLine != null)
                StopCoroutine(displayLine);
            ShowContinueIndicator(true);
        }
    }

    public virtual void Disable()
    {
        show = false;
        dialogueEnd = true;
        dialogueText.text = "";
        dialogueIndex = 0;
        ShowContinueIndicator(false);
    }


    private void Update()
    {
        if (time < 1f && show)
        {
            time += Time.deltaTime * animationSpeed;
            DisplayAnimations(time);
        }
        else if (time > 0f && !show)
        {
            time -= Time.deltaTime * animationSpeed;
            DisplayAnimations(time);

            if (time <= 0f)
                this.gameObject.SetActive(false);
        }

        if (!dialogueEnd)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                Continue();
            }
        }

    }

    private void DisplayAnimations(float time)
    {
        fadeValue = fadeCurve.Evaluate(time);
        imageValue = imageCurve.Evaluate(time);

        lowerVignette.color = new Vector4(lowerVignette.color.r, lowerVignette.color.g, lowerVignette.color.b, fadeValue);
        dialogueText.color = new Vector4(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, fadeValue);

        portrait.rectTransform.anchoredPosition = new Vector2(portrait.rectTransform.anchoredPosition.x, finalYPosition * imageValue);
    }

    private IEnumerator DisplayLine (string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.maxVisibleCharacters++;

            // Typing sound goes here.
            if(dialogue.dialogueLines[dialogueIndex] != null)
            AudioManager.instance.PlayWithRandomPitch(dialogue.dialogueLines[dialogueIndex].speakerSound, 1f, 1.4f);

            if (dialogueText.maxVisibleCharacters == dialogueText.text.Length)
            {
                LineComplete();
                ShowContinueIndicator(true);
            }
            yield return new WaitForSeconds(typingSpeed);
        }
    }


    private void LineComplete()
    {
        isDisplayingLine = false;
        dialogueIndex++;
        dialogueText.maxVisibleCharacters = dialogueText.text.Length;
    }


    private void ShowContinueIndicator(bool value)
    {
        if (value)
        {
            if (dialogueIndex == dialogue.dialogueLines.Length)
                continueIndicator.sprite = null;
            else
                continueIndicator.sprite = continueSprite;

            continueIndicator.color = new Vector4(continueIndicator.color.r, continueIndicator.color.g, continueIndicator.color.b, 1f);
        }
        else
        {
            continueIndicator.color = new Vector4(continueIndicator.color.r, continueIndicator.color.g, continueIndicator.color.b, 0f);
        }
    }

}
