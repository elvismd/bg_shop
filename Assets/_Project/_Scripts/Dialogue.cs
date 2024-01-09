using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueOption
{
    public string id;
    [HideInInspector] public Action onChoose;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    [TextArea]
    public string message;
    public List<DialogueOption> options;

    [HideInInspector] public Action onEnd;
}
