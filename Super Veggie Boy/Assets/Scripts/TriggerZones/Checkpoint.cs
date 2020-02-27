using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : TriggerZone
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            gameManager.ReachCheckpoint(transform.position); // not worth keeping the reference as a variable
        }
    }
}
