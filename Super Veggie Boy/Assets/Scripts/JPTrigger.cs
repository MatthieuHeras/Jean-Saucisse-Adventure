using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPTrigger : DialogueTrigger
{
    [SerializeField] private float force = 20f;
    public override void EndAction()
    {
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.AddRelativeForce(Vector3.back * force, ForceMode.Impulse);
        }
    }
}
