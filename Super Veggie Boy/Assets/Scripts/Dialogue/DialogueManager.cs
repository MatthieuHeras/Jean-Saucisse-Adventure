using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Animator anim = default;
    [SerializeField] private TextMeshProUGUI title = default;
    [SerializeField] private TextMeshProUGUI text = default;
    [SerializeField] private CameraController camController = default;

    private Queue<string> sentences;
    private DialogueTrigger trigger;


    public void StartDialogue(Dialogue dialogue, DialogueTrigger dialogueTrigger)
    {
        trigger = dialogueTrigger;
        camController.isLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        anim.SetBool("Open", true);
        sentences = new Queue<string>();
        title.text = dialogue.name;
        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count <= 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        text.text = sentence;
    }

    private void EndDialogue()
    {
        anim.SetBool("Open", false);
        camController.isLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        trigger.EndAction();
        Debug.Log("End of conv");
    }
}
