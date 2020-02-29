using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private RigidbodyController rbController;

    private void Update()
    {
        anim.speed = rbController.wishedSpeed;
    }
}
