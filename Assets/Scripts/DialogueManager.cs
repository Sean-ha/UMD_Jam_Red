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
    private SoundManager sm;
    private AudioSource talkSource;

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

    private void Start()
    {
        sm = SoundManager.instance;
        talkSource = sm.GetAudioSource(SoundManager.Sound.Talk);
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
                if (!talkSource.isPlaying)
                {
                    talkSource.Play();
                }
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

    // Called whenever new dialogue string is to be shown (including first time)
    private void StartDialogue()
    {
        sm.PlaySound(SoundManager.Sound.DialogueBox);

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
        dialogueText.rectTransform.localScale = new Vector2(0, 0);

        // DialogueArrow only has popup effect on first text
        if (dialogueCounter == 0)
        {
            dialogueArrow.transform.localScale = new Vector2(0, 0);
            LeanTween.scale(dialogueArrow, new Vector2(1.2f, 1.2f), 0.1f).setOnComplete(ScaleBackArrow);
        }
        LeanTween.scale(dialogueBox, new Vector2(boxScale.x + 0.2f, boxScale.y + 0.2f), 0.1f).setOnComplete(ScaleBackBox);
        LeanTween.scale(dialogueText.rectTransform, new Vector2(1.2f, 1.2f), 0.1f).setOnComplete(ScaleBackText);

        dialogueText.gameObject.SetActive(true);
        dialogueText.maxVisibleCharacters = 0;

        // Sets the text to have the given string
        dialogueText.text = currentDialogueObject.dialogues[dialogueCounter].dialogue;
        dialogueText.ForceMeshUpdate();

        totalVisibleCharacters = dialogueText.textInfo.characterCount;
        visibleCount = 0;

        // Handles dialogue events at start of dialogue
        switch (currentDialogueObject.id)
        {
            case 3: AfterWateringByFarmer(); break;
        }

        canProceed = false;
        isWriting = true;
    }

    private void EndDialogue()
    {
        isWriting = false;
        canProceed = false;

        LeanTween.scale(dialogueArrow, new Vector2(0, 0), 0.1f);
        LeanTween.scale(dialogueText.rectTransform, new Vector2(0, 0), 0.1f);
        LeanTween.scale(dialogueBox, new Vector2(0, 0), 0.1f).setOnComplete(EnablePlayerMovement);
    }

    #region Dialogue Events

    private void AfterFirstApril()
    {
        GameObject.Find("ToMomsRoom").GetComponent<Door>().EnterDoor();
    }

    private void AfterWateringByFarmer()
    {
        GameObject.Find("Farmer").transform.localScale = new Vector2(-1, 1);
    }

    private void NeedToVisitMom()
    {
        StartCoroutine(WalkBack());
    }

    private IEnumerator WalkBack()
    {
        pc.SetCanMove(false);
        pc.SetHorizontal(-1);

        yield return new WaitForSeconds(0.1f);

        pc.SetHorizontal(0);
        pc.SetCanMove(true);
    }

    private void BoyTurns()
    {
        pc.SetCanMove(false);
        pc.GetComponent<Animator>().Play("Player_LookUp");
        StartCoroutine(BoyTurning());
    }

    private IEnumerator BoyTurning()
    {
        GameObject boy = GameObject.Find("Boy");
        yield return new WaitForSeconds(1.3f);

        boy.transform.localScale = new Vector2(1, 1);

        yield return new WaitForSeconds(1.8f);

        boy.GetComponent<DialogueTrigger>().TriggerDialogue();
    }

    private void BoyLeaves()
    {
        pc.SetCanMove(false);
        StartCoroutine(BoyLeaving());
    }

    private IEnumerator BoyLeaving()
    {
        GameObject boy = GameObject.Find("Boy");
        yield return new WaitForSeconds(.75f);

        boy.GetComponent<Animator>().Play("BoyNormal_Walk");
        LeanTween.moveLocalX(boy, 20, 6).setOnComplete(SetPlayerCanMove);
        Destroy(boy, 6.1f);
    }

    private void BoyTurns2()
    {
        pc.SetCanMove(false);
        pc.GetComponent<Animator>().Play("Player_LookUp");
        StartCoroutine(BoyDialogueContinued());
    }

    private IEnumerator BoyDialogueContinued()
    {
        GameObject boy = GameObject.Find("Boy");
        yield return new WaitForSeconds(1.3f);

        boy.transform.localScale = new Vector2(-1, 1);
        boy.GetComponent<Animator>().Play("BoyNormal_Walk");
        LeanTween.moveLocalX(boy, boy.transform.localPosition.x + 2, 2);

        yield return new WaitForSeconds(2);

        pc.SetFacingRight();
        boy.GetComponent<Animator>().Play("BoyNormal_Idle");

        yield return new WaitForSeconds(1.5f);

        boy.GetComponent<DialogueTrigger>().TriggerDialogue();
    }

    private void BoyTurnsBack()
    {
        pc.SetCanMove(false);
        pc.GetComponent<Animator>().Play("Player_LookUp");
        StartCoroutine(BoyTurnsBackCR());
    }

    private IEnumerator BoyTurnsBackCR()
    {
        GameObject boy = GameObject.Find("Boy");
        yield return new WaitForSeconds(1.5f);
        boy.transform.localScale = new Vector2(1, 1);
        yield return new WaitForSeconds(2);
        boy.GetComponent<DialogueTrigger>().TriggerDialogue();
    }

    private void BoyLeaves2()
    {
        pc.SetCanMove(false);
        StartCoroutine(BoyLeaving2());
    }

    private IEnumerator BoyLeaving2()
    {
        GameObject boy = GameObject.Find("Boy");
        yield return new WaitForSeconds(.75f);

        boy.transform.localScale = new Vector2(-1, 1);
        boy.GetComponent<Animator>().Play("BoyNormal_Walk");

        LeanTween.moveLocalX(boy, boy.transform.localPosition.x + 8, 4);
        Destroy(boy, 4.1f);

        yield return new WaitForSeconds(4);

        pc.SetCanMove(true);
    }

    private void SetPlayerCanMove()
    {
        pc.SetCanMove(true);
    }

    #endregion

    #region LeanTween setOnComplete
    private void ScaleBackArrow()
    {
        LeanTween.scale(dialogueArrow, new Vector2(1f, 1f), 0.07f);
    }

    private void ScaleBackBox()
    {
        LeanTween.scale(dialogueBox, boxScale, 0.07f);
    }

    private void ScaleBackText()
    {
        LeanTween.scale(dialogueText.rectTransform, new Vector2(1, 1), 0.07f);
    }

    private void EnablePlayerMovement()
    {
        pc.SetCanMove(true);
        pc.GetComponent<Animator>().Play("Player_Idle");

        // Handles dialogue events that happen after dialogue ends
        switch (currentDialogueObject.id)
        {
            case 1: AfterFirstApril(); break;
            case 4: NeedToVisitMom(); break;
            case 5: BoyTurns(); break;
            case 6: BoyLeaves(); break;
            case 7: BoyTurns2(); break;
            case 8: BoyTurnsBack(); break;
            case 9: BoyLeaves2(); break;
        }
    }

    #endregion
}
