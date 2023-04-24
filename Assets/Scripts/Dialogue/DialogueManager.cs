using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue Users")]

    [SerializeField] DialogueUser[] users;
    //Text displayer, use TextMeshPro for better animations and better display
    TMP_Text textComponent;
    //Reference to button to make the text continue
    GameObject continueButton;
    GameObject dialogueBox;


    //Text that will display
    public Conversation currentConversation;
    //Current sentence inside the currentText variable
    public int textIndex;

    //Multiple checks to make the sentence work, sentence over checks if the current sentence is over,
    //writing sentence checks if its currently writing a sentence, and wholeTextOver checks if there
    //are more sentences to write in the currentText array
    bool sentenceOver;
    bool writingSentence;
    bool wholeTextOver;


    //Variable to call when you want someone to say something, just feed him an array of as many strings as you like and done
    //It resets the wholeTextOver variable to false

    [SerializeField] float textSpeed;
    [SerializeField] float timeBetweenTexts;

    public void Start()
    {
        CreateDialogue();
    }

    public void CreateDialogue()
    {
        wholeTextOver = false;
        
        textIndex = 0;
        ChangeDialogue(currentConversation.dialogue[textIndex].user);
        StartCoroutine(DisplayText());
    }

    //Method to display the text in the currentTextVariable
    IEnumerator DisplayText()
    {
        sentenceOver = false;
        textComponent.text = "";
        ChangeDialogue(currentConversation.dialogue[textIndex].user);
        string originalText = currentConversation.dialogue[textIndex].line;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in currentConversation.dialogue[textIndex].line)
        {
            alphaIndex++;
            textComponent.text = originalText;
            displayedText = textComponent.text.Insert(alphaIndex, "<color=#00000000>");
            textComponent.text = displayedText;
            //AudioManager.instance.PlaySound("Dialogue0" + Random.Range(1, 10));

            yield return new WaitForSecondsRealtime(textSpeed);
        }

        sentenceOver = true;

        yield return new WaitForSecondsRealtime(timeBetweenTexts);

        NextSentence();
    }

    //Method to continue displaying sentences in the text box
    public void NextSentence()
    {
        if(textIndex >= currentConversation.dialogue.Length - 1)
        {
            DeactivateDialogue();
        }
        else
        {
            if (sentenceOver)
            {
                if (textIndex < currentConversation.dialogue.Length - 1)
                {
                    textIndex++;
                    StartCoroutine(DisplayText());
                }
                else
                {
                    textComponent.text = "";
                    wholeTextOver = true;
                }
            }
        }
        
    }


    public void ChangeDialogue(int user)
    {
        if(textComponent != null)
        {
            textComponent.gameObject.SetActive(false);
        }

        if(continueButton != null)
        {
            continueButton.gameObject.SetActive(false);
        }

        if(dialogueBox != null)
        {
            dialogueBox.gameObject.SetActive(false);
        }

        AssignUser(user);

        textComponent.gameObject.SetActive(true);
        dialogueBox.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(true);

    }

    public void AssignUser(int user)
    {
        textComponent = users[user].textComponent;
        continueButton = users[user].continueButton;
        dialogueBox = users[user].dialogueBox;

    }
    public void DeactivateDialogue()
    {
        textComponent.gameObject.SetActive(false);
        dialogueBox.SetActive(false);
        continueButton.SetActive(false);
    }
}


[System.Serializable]
public class DialogueUser
{
    public int dialogueUser;
    public TMP_Text textComponent;
    public GameObject continueButton;
    public GameObject dialogueBox;
}