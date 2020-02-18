using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float cooldown = 5f;

    private Vector3 gravityDir = Vector3.down;
    private float timer = 0f;

    [SerializeField] private List<CustomGravity> fallers = new List<CustomGravity>();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent(out CustomGravity faller))
        {
            fallers.Add(faller);
            faller.EnterGravityZone(gravityDir, gravityMultiplier);
            if (other.TryGetComponent(out RigidbodyController player))
                player.ChangeDir(-gravityDir);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CustomGravity faller))
        {
            fallers.Remove(faller);
            faller.LeaveGravityZone();
        }
    }

    private void Update()
    {
        if (timer < cooldown)
            timer += Time.deltaTime;
        else
        {
            timer = 0f;
            ChangeGravityDir();
            UpdateObjectsGravity();
        }
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

    private void UpdateObjectsGravity()
    {
        foreach (CustomGravity faller in fallers)
        {
            faller.ChangeGravity(gravityDir, gravityMultiplier);
            if (faller.gameObject.TryGetComponent(out RigidbodyController player))
                player.StartCoroutine("ChangeDir", -gravityDir);
        }
    }
}
