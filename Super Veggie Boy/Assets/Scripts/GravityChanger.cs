using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private List<Vector3> gravityDirs = new List<Vector3>();

    private Vector3 gravityDir;
    private int index = 0;
    private float timer = 0f;

    private List<CustomGravity> fallers = new List<CustomGravity>();

    private void Awake()
    {
        if (gravityDirs.Count == 0)
            Debug.LogError(gameObject.name + " needs at least one gravity direction ! Do not use gravityChanger without new gravity direction !");
    }

    private void Start()
    {
        gravityDir = gravityDirs[0];
    }

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
            if (other.TryGetComponent(out RigidbodyController player))
                player.ChangeDir(-gravityDir);
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
        index++;
        if (index >= gravityDirs.Count)
            index = 0;
        gravityDir = gravityDirs[index];
    }

    private void UpdateObjectsGravity()
    {
        foreach (CustomGravity faller in fallers)
        {
            faller.ChangeGravity(gravityDir, gravityMultiplier);
            if (faller.gameObject.TryGetComponent(out RigidbodyController player))
                player.ChangeDir(-gravityDir);
        }
    }
}
