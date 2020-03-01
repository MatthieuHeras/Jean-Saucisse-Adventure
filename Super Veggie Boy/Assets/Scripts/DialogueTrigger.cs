using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue = default;
    [SerializeField] private DialogueManager dialogueManager = default;

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogue, this);
    }

    public abstract void EndAction();
}
