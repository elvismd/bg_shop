using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : SingleInstance<DialogueSystem>
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject optionsButtons;
    [SerializeField] private TextMeshProUGUI dialogueText;

    Queue<Dialogue> dialogues = new Queue<Dialogue>();
    bool inDialogue;
    bool playing;
    bool skip;

    Dialogue currentDialogue;

    void Start()
    {
        nextButton.SetActive(false);
        dialogueBox.SetActive(false);
        optionsButtons.SetActive(false);
    }

    void Update()
    {
        if (playing)
        {
            return;
        }

        if(dialogues.Count > 0 ) 
        {
            StartCoroutine(PlayNextDialogue());
        }
    }

    IEnumerator PlayNextDialogue()
    {
        optionsButtons.SetActive(false);

        skip = false;
        playing = true;
        dialogueText.text = "";

        currentDialogue = dialogues.Dequeue();

        dialogueBox.SetActive(true);

        int letter = 0;
        int count = currentDialogue.message.Length;

        inDialogue = true;
        WaitForSeconds letterWfs = new WaitForSeconds(.04f);        
        while(letter < count)
        {
            dialogueText.text += currentDialogue.message[letter++];
            if (skip)
            {
                dialogueText.text = currentDialogue.message;
                break;
            }

            yield return letterWfs;
        }
        inDialogue = false;
        skip = false;

        nextButton.SetActive(true);

        if(currentDialogue.onClickYes != null || currentDialogue.onClickNo != null)
        {
            optionsButtons.SetActive(true);
        }

        yield return null;
    }

    public void OnClickYes()
    {
        if (currentDialogue != null && currentDialogue.onClickYes != null)
            currentDialogue.onClickYes.Invoke();

        Next();
    }

    public void OnClickNo()
    {
        if (currentDialogue != null && currentDialogue.onClickNo != null)
            currentDialogue.onClickNo.Invoke();

        Next();
    }

    public void Next()
    {
        if (inDialogue)
        {
            skip = true;
        }
        else
        {
            playing = false;
            dialogueBox.SetActive(false);
            nextButton.SetActive(false);

            currentDialogue.onEnd?.Invoke();
        }
    }

    public void StopAndClear()
    {
        if (!playing) return;

        skip = true;
        playing = false;
        dialogueBox.SetActive(false);
        nextButton.SetActive(false);

        if(currentDialogue != null)
            currentDialogue.onEnd?.Invoke();

        //currentDialogue = null;
        dialogues.Clear();

    }

    public void Play(Dialogue dialogue) => dialogues.Enqueue(dialogue);
    
}
