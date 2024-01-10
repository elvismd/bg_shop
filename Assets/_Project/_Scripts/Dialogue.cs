using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    [TextArea]
    public string message;

    public Action onClickYes;
    public Action onClickNo;
    public Action onEnd;
}
