using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    [SerializeField] private float gravityValue = 9.81f;
    [SerializeField] private float cooldown = 5f;

    private Vector3 gravityDir = Vector3.down;
    private bool isInside = false;
    private float timer = 0f;
    private Vector3 previousGravity = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        isInside = true;
        previousGravity = Physics.gravity;
    }

    private void OnTriggerExit(Collider other)
    {
        isInside = false;
        Physics.gravity = previousGravity;
    }

    private void Update()
    {
        if (timer < cooldown)
            timer += Time.deltaTime;
        else
        {
            timer = 0f;
            ChangeGravityDir();
        }
        if (isInside && Physics.gravity != gravityValue * gravityDir)
            Physics.gravity = gravityValue * gravityDir;
    }

    private void ChangeGravityDir()
    {
        if (gravityDir.Equals(Vector3.down))
            gravityDir = Vector3.left;
        else if (gravityDir.Equals(Vector3.left))
            gravityDir = Vector3.up;
        else if (gravityDir.Equals(Vector3.up))
            gravityDir = Vector3.right;
        else
            gravityDir = Vector3.down;
    }
}
