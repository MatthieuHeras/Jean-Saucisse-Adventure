using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator anim = default;
    [SerializeField] private RigidbodyController rbController = default;

    private void Update()
    {
        anim.speed = rbController.wishedSpeed;
    }
}
