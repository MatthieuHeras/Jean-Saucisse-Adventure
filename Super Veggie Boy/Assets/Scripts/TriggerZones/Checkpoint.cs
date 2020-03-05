using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : TriggerZone
{
    [SerializeField] private Transform checkpointTransform = default;

    private void OnTriggerEnter(Collider other)
    {
        gameManager.ReachCheckpoint(checkpointTransform.position);
    }
}
