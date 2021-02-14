using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    private TextMeshProUGUI dialogueText;
    private GameObject dialogueArrow;
    private GameObject dialogueBox;
    private Vector2 textDimensions;
    private Vector2 boxScale;
    private PlayerController pc;

    private DialogueObject currentDialogueObject;
    private Vector2 pointTo;
    private int dialogueCounter;

    private bool isWriting = false;
    private bool canProceed = false;

    private int totalVisibleCharacters;
    private int visibleCount;

    // The total time it has been since the previous character has been printed
    private float timeSinceLastChar;
    private float timeBetweenChars = 0.015f;

    private void Awake()
    {
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueArrow = GameObject.Find("DialogueArrow");
        dialogueBox = GameObject.Find("DialogueBox");
        pc = FindObjectOfType<PlayerController>();
        textDimensions = dialogueText.rectTransform.sizeDelta;

        dialogueText.gameObject.SetActive(false);
        dialogueArrow.SetActive(false);
        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (isWriting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueText.maxVisibleCharacters = totalVisibleCharacters;
                isWriting = false;
                canProceed = true;
                return;
            }
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
                canProceed = true;
            }
        }
        else if (canProceed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Proceed to next dialogue if possible. Otherwise, end it
                dialogueCounter++;
                if (dialogueCounter == currentDialogueObject.dialogues.Length)
                {
                    EndDialogue();
                } else
                {
                    StartDialogue();
                }
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
        boxScale = currentDialogueObject.dialogues[dialogueCounter].dialogueBoxScale;

        // Speech bubble popup
        dialogueBox.SetActive(true);
        dialogueArrow.SetActive(true);
        dialogueArrow.transform.localPosition = pointTo;
        dialogueBox.transform.localPosition = new Vector2(pointTo.x, pointTo.y + .75f);
        dialogueBox.transform.localScale = new Vector2(0, 0);

        // Dialogue text positioning
        Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(pointTo.x, pointTo.y + .8f));
        dialogueText.rectTransform.anchoredPosition = screenPos;
        dialogueText.rectTransform.sizeDelta = new Vector2(textDimensions.x * boxScale.x, textDimensions.y * boxScale.y);

        // DialogueArrow only has popup effect on first text
        if (dialogueCounter == 0)
        {
            dialogueArrow.transform.localScale = new Vector2(0, 0);
            LeanTween.scale(dialogueArrow, new Vector2(1.3f, 1.3f), 0.15f).setOnComplete(ScaleBackArrow);
        }
        LeanTween.scale(dialogueBox, new Vector2(boxScale.x + 0.3f, boxScale.y + 0.3f), 0.15f).setOnComplete(ScaleBackBox);

        dialogueText.gameObject.SetActive(true);
        dialogueText.maxVisibleCharacters = 0;

        // Sets the text to have the given string
        dialogueText.text = currentDialogueObject.dialogues[dialogueCounter].dialogue;
        dialogueText.ForceMeshUpdate();

        totalVisibleCharacters = dialogueText.textInfo.characterCount;
        visibleCount = 0;

        canProceed = false;
        isWriting = true;
    }

    private void EndDialogue()
    {
        isWriting = false;
        canProceed = false;

        LeanTween.scale(dialogueArrow, new Vector2(0, 0), 0.15f);
        LeanTween.scale(dialogueBox, new Vector2(0, 0), 0.15f).setOnComplete(EnablePlayerMovement);
        dialogueText.gameObject.SetActive(false);
    }

    #region LeanTween setOnComplete
    private void ScaleBackArrow()
    {
        LeanTween.scale(dialogueArrow, new Vector2(1f, 1f), 0.1f);
    }

    private void ScaleBackBox()
    {
        LeanTween.scale(dialogueBox, boxScale, 0.1f);
    }

    private void EnablePlayerMovement()
    {
        pc.SetCanMove(true);
    }

    #endregion
}
