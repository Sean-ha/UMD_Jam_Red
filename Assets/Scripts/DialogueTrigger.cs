using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueObject[] dialogueObjects;
    public int npcId;

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
        currentDialogue = AdvanceDialogue.GetDialogueCount(npcId);
        dm.SetDialogue(dialogueObjects[currentDialogue], speechBubbleOrigin.position);
    }
}
