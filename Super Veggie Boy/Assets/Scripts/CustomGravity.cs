using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = default;

    private Vector3 gravityDir = Vector3.down;
    private float gravityMultiplier = 1f;
    private List<int> zoneIDs = new List<int>();

    private void Awake()
    {
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        rb.AddForce(gravityDir * Const.gravityForce * gravityMultiplier * Time.fixedDeltaTime * 60f);
    }

    public void EnterGravityZone(Vector3 newGravityDir, float newGravityMultiplier, int zoneIndex)
    {
        zoneIDs.Add(zoneIndex);
        ChangeGravity(newGravityDir, newGravityMultiplier, zoneIndex);
    }

    public void LeaveGravityZone(int zoneIndex)
    {
        zoneIDs.Remove(zoneIndex);
        if (zoneIDs.Count == 0)
        {
            ChangeGravity(Vector3.down, 1, 0);
        }
    }
    
    public void ChangeGravity(Vector3 newGravityDir, float newGravityMultiplier, int zoneIndex) // put everything you want if zoneIndexes is empty
    {
        if (zoneIDs.Count == 0 || zoneIDs[zoneIDs.Count - 1] == zoneIndex)
        gravityDir = newGravityDir;
        gravityMultiplier = newGravityMultiplier;
    }

    public Vector3 GetGravityDir() { return gravityDir; }
    public float GetGravityMultiplier() { return gravityMultiplier; }
}
