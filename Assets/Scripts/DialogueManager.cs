using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    private TextMeshProUGUI dialogueText;

    private DialogueObject currentDialogueObject;
    private Vector2 pointTo;
    private int dialogueCounter;

    private bool isWriting;

    private int totalVisibleCharacters;
    private int visibleCount;

    // The total time it has been since the previous character has been printed
    private float timeSinceLastChar;
    private float timeBetweenChars = 0.05f;

    private void Awake()
    {
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isWriting)
        {
            // Play sound here

            dialogueText.maxVisibleCharacters = visibleCount;

            timeSinceLastChar += Time.deltaTime;

            if (timeSinceLastChar < timeBetweenChars)
            {
                return;
            }

            timeSinceLastChar = 0;

            if (visibleCount < totalVisibleCharacters)
            {
                visibleCount++;
            }
            else
            {
                isWriting = false;
            }
        }
    }

    // pointTo is where the speech bubble arrow points. bubbleSize is size of speech bubble.
    public void SetDialogue(DialogueObject dialogueObject, Vector2 pointTo)
    {
        currentDialogueObject = dialogueObject;
        this.pointTo = pointTo;
        dialogueCounter = 0;

        StartDialogue();
    }

    private void StartDialogue()
    {
        dialogueText.gameObject.SetActive(true);

        // Sets the text to have the given string
        dialogueText.text = currentDialogueObject.dialogues[0].dialogue;
        dialogueText.ForceMeshUpdate();

        totalVisibleCharacters = dialogueText.textInfo.characterCount;
        visibleCount = 0;

        isWriting = true;
    }
}
