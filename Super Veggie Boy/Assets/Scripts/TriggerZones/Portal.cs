using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : TriggerZone
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.Win();
    }
}
