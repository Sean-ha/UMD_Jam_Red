using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueObject[] dialogueObjects;

    private DialogueManager dm;
    private Transform speechBubbleOrigin;

    private int currentDialogue;

    private void Awake()
    {
        speechBubbleOrigin = transform.Find("SpeechBubbleOrigin");
        dm = FindObjectOfType<DialogueManager>();
    }

    // To be called whenever a dialogue is to be initiated
    public void TriggerDialogue()
    {
        dm.SetDialogue(dialogueObjects[currentDialogue], speechBubbleOrigin.position);
    }
}
