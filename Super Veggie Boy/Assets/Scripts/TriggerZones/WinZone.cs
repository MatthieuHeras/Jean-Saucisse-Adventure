using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : TriggerZone
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
            gameManager.Win();
    }
}
