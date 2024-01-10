using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGeneric : MonoBehaviour
{
    [SerializeField] private Dialogue[] conversation;

    public void Talk()
    {
        for (int i = 0; i < conversation.Length; i++)
        {
            DialogueSystem.Instance.Play(conversation[i]);
        }
    }
}
