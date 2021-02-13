using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class DialogueObject : ScriptableObject
{
    public Dialogue[] dialogues;
    public int id;
}

[Serializable]
public struct Dialogue
{
    [TextArea(6, 10)]
    public string dialogue;
    public Vector2 dialogueBoxScale;
}