using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLimit : TriggerZone
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.Lose();
    }
}
