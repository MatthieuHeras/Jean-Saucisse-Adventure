using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    [SerializeField] private Rigidbody rb = default;

    private Vector3 gravityDir = Vector3.down;
    private float gravityMultiplier = 1f;
    public int ZoneBuffer { get; private set; }

    private void Awake()
    {
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        rb.AddForce(gravityDir * Const.gravityForce * gravityMultiplier * Time.fixedDeltaTime * 60f);
    }

    public void EnterGravityZone(Vector3 newGravityDir, float newGravityMultiplier)
    {
        ZoneBuffer++;
        ChangeGravity(newGravityDir, newGravityMultiplier);
    }

    public void LeaveGravityZone()
    {
        if (--ZoneBuffer == 0)
        {
            ChangeGravity(Vector3.down, 1);
        }
    }
    
    public void ChangeGravity(Vector3 newGravityDir, float newGravityMultiplier)
    {
        gravityDir = newGravityDir;
        gravityMultiplier = newGravityMultiplier;
    }
}
