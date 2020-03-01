using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform playerTransform = default;
    [SerializeField] private LayerMask characterMask = 0;
    [SerializeField] private Animator anim = default;
    [SerializeField] private RigidbodyController rbController = default;

    private void Update()
    {
        anim.speed = rbController.wishedSpeed;
        if (Input.GetKeyDown(KeyCode.E) && 
            Physics.Raycast(playerTransform.position, playerTransform.forward, out RaycastHit info, Mathf.Infinity, characterMask) && 
            info.collider.TryGetComponent(out DialogueTrigger dialogueTrigger)){
                dialogueTrigger.TriggerDialogue();
        }
    }
}
