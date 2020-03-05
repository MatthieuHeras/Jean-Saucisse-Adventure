using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPTrigger : DialogueTrigger
{
    [SerializeField] private Rigidbody rb = default;
    [SerializeField] private float force = 20f;
    private void Awake()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public override void EndAction()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddRelativeForce(Vector3.back * force, ForceMode.Impulse);        
    }
}
