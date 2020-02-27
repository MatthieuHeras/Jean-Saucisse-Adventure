using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    private static int staticID = 0;

    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private List<Vector3> gravityDirs = new List<Vector3>();

    private Vector3 gravityDir;
    private int index = 0;
    private float timer = 0f;
    private int zoneID;
    private List<CustomGravity> fallers = new List<CustomGravity>();
    ParticleSystem.ShapeModule shape;

    private void Awake()
    {
        zoneID = staticID++;

        if (gravityDirs.Count == 0)
            Debug.LogError(gameObject.name + " needs at least one gravity direction ! Do not use gravityChanger without new gravity direction !");

        shape = GetComponent<ParticleSystem>().shape;
    }

    private void Start()
    {
        ChangeGravityDir();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CustomGravity faller))
        {
            fallers.Add(faller);
            faller.EnterGravityZone(gravityDir, gravityMultiplier, zoneID);
            if (other.TryGetComponent(out RigidbodyController player))
                player.EnterGravityZone(-gravityDir, zoneID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CustomGravity faller))
        {
            fallers.Remove(faller);
            faller.LeaveGravityZone(zoneID);
            if (other.TryGetComponent(out RigidbodyController player))
                player.LeaveGravityZone(zoneID);
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
        }
    }

    private void ChangeGravityDir()
    {
        index++;
        if (index >= gravityDirs.Count)
            index = 0;
        gravityDir = gravityDirs[index];

        shape.rotation = Quaternion.LookRotation(gravityDir).eulerAngles;
        shape.position = -gravityDir / 2f;
        UpdateObjectsGravity();
    }

    private void UpdateObjectsGravity()
    {
        foreach (CustomGravity faller in fallers)
        {
            faller.ChangeGravity(gravityDir, gravityMultiplier, zoneID);
            if (faller.gameObject.TryGetComponent(out RigidbodyController player))
                player.ChangeDir(-gravityDir, zoneID);
        }
    }
}
