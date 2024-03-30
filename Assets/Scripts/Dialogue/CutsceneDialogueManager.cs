using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneDialogueManager : MonoBehaviour
{
    [SerializeField] CutsceneText[] cutsceneText;
    [SerializeField] float textShowSpeed;
    [SerializeField] float fadeOutSpeed;
    int cutsceneIndex = 0;

    float currentTime;
    bool textActive;
    CutsceneText currentText;
    [SerializeField] TextMeshProUGUI cutsceneTextComponent;

    [SerializeField] bool allowWrite;
    void Start()
    {
        cutsceneIndex = 0;
        Invoke("SetText", 0.5f);
    }

    public void SetText()
    {
        if(cutsceneIndex < cutsceneText.Length)
        {
            cutsceneTextComponent.text = cutsceneText[cutsceneIndex].cutsceneText;
            cutsceneTextComponent.maxVisibleCharacters = 0;
            StartCoroutine(ShowText());
        }
        
    }


    public void NextText()
    {
        cutsceneIndex++;
    }
    IEnumerator ShowText()
    {
        Invoke("FadeOutMethod", cutsceneText[cutsceneIndex].timeUntilFadeOut);
        char[] textCharacters = cutsceneText[cutsceneIndex].cutsceneText.ToCharArray();

        if (allowWrite)
        {
            if (cutsceneIndex >= cutsceneText.Length)
            {
                yield break;
            }

            for (int i = 0; i < textCharacters.Length; i++)
            {
                cutsceneTextComponent.maxVisibleCharacters++;
                yield return new WaitForSeconds(textShowSpeed);
            }

            cutsceneTextComponent.maxVisibleCharacters = textCharacters.Length;
        }
        else
        {
            StartCoroutine(FadeInText());
            cutsceneTextComponent.maxVisibleCharacters = textCharacters.Length;
        }

    }

    public void FadeOutMethod()
    {
        StartCoroutine(FadeOutText());
    }
    IEnumerator FadeOutText()
    {
        float value = 1;

        while(value > 0)
        {
            value -= Time.deltaTime* fadeOutSpeed;
            cutsceneTextComponent.color = new Color(cutsceneTextComponent.color.r, cutsceneTextComponent.color.g, cutsceneTextComponent.color.b, value);
            yield return null;
        }

        cutsceneTextComponent.color = new Color(cutsceneTextComponent.color.r, cutsceneTextComponent.color.g, cutsceneTextComponent.color.b, 0);
        cutsceneTextComponent.text = "";

        cutsceneTextComponent.color = new Color(cutsceneTextComponent.color.r, cutsceneTextComponent.color.g, cutsceneTextComponent.color.b, 1);
        Debug.Log("==");

        Invoke("SetText", cutsceneText[cutsceneIndex].timeUntilNextSentence);
        NextText();
    }

    IEnumerator FadeInText()
    {
        float value = 0;
        cutsceneTextComponent.color = new Color(cutsceneTextComponent.color.r, cutsceneTextComponent.color.g, cutsceneTextComponent.color.b, 0);
        while (value > 0)
        {
            value += Time.deltaTime * fadeOutSpeed;
            cutsceneTextComponent.color = new Color(cutsceneTextComponent.color.r, cutsceneTextComponent.color.g, cutsceneTextComponent.color.b, value);
            yield return null;
        }

        cutsceneTextComponent.color = new Color(cutsceneTextComponent.color.r, cutsceneTextComponent.color.g, cutsceneTextComponent.color.b, 1);
    }
}

[System.Serializable]
public class CutsceneText
{
    [TextArea()] public string cutsceneText;
    public float timeUntilFadeOut;
    public float timeUntilNextSentence;
}
