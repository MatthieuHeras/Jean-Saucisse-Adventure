using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{

    public float wishedSpeed = 0f; // between 0 and 1, for animation

    [SerializeField] private float speed = 50f;
    [SerializeField] private float movementDrag = 8f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private int jumpLimit = 1;
    [SerializeField] private CustomGravity customGravity = default;
    [SerializeField] private Transform camTransform = default;
    [SerializeField] private Transform feet = default;
    [SerializeField] private LayerMask groundLayer = default;

    [Range(0.01f, 2f)]
    [SerializeField] private float changeDirAnimationSpeed = 1f;

    private Rigidbody rb;
    private Transform playerTransform;
    private int jumpBuffer = 0;
    private List<int> zoneIDs = new List<int>();
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool hasLeftGround = false;
    private GameObject rotationGhost;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        rotationGhost = new GameObject("RotationGhost");
        if (changeDirAnimationSpeed < 0.01f)
            changeDirAnimationSpeed = 0.01f;
    }

    private void FixedUpdate()
    {
        // Controls
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        wishedSpeed = (Mathf.Abs(xAxis) + Mathf.Abs(yAxis)) / 2f;
        // Move
        rb.AddRelativeForce(new Vector3(speed * Time.fixedDeltaTime * xAxis, 0f, speed * Time.fixedDeltaTime * yAxis), ForceMode.VelocityChange);

        // Large jumps
        if (Input.GetButton("Jump") && Vector3.Dot(rb.velocity, Physics.gravity) < 0f)
            rb.AddForce(customGravity.GetGravityMultiplier() * Const.gravityForce / 2f * -customGravity.GetGravityDir(), ForceMode.Acceleration);

        // Drag
        Vector3 localVelocity = playerTransform.InverseTransformDirection(rb.velocity); // Convert to local space
        localVelocity -=
            (localVelocity.x * movementDrag * Time.fixedDeltaTime * Vector3.right +
            localVelocity.z * movementDrag * Time.fixedDeltaTime * Vector3.forward);
        rb.velocity = playerTransform.TransformDirection(localVelocity);
    }

    private void Update()
    {
        if (!isJumping)
        {
            if (isGrounded = Physics.OverlapSphere(feet.position, 0.3f, groundLayer).Length > 0)
                TouchGround();
            else if (!hasLeftGround)
                LeaveGround();
        }
        // Jump
        if (Input.GetButtonDown("Jump") && !isJumping && jumpBuffer < jumpLimit)
            Jump();
    }

    public void Teleport(Vector3 point)
    {
        playerTransform.position = point;
    }

    public void EnterGravityZone(Vector3 newDir, int zoneIndex)
    {
        zoneIDs.Add(zoneIndex);
        ChangeDir(newDir, zoneIndex);
    }
    public void LeaveGravityZone(int zoneIndex)
    {
        zoneIDs.Remove(zoneIndex);
        if (zoneIDs.Count == 0)
            ChangeDir(Vector3.up, -1);
    }
    public void ChangeDir(Vector3 newDir, int zoneIndex)
    {
        if (zoneIDs.Count == 0 || zoneIndex == zoneIDs[zoneIDs.Count - 1])
        {
            StopCoroutine(nameof(CoroutineChangeDir));
            StartCoroutine(nameof(CoroutineChangeDir), newDir);
        }
    }

    public IEnumerator CoroutineChangeDir(Vector3 newDir)
    {
        // Change the rotation of the player, depending on its current rotation and the gravity using Lerp
        // The animation will almost always finish before currentTime reaches 1, because the current rotation is the starting point of the lerp. 
        // This is not really an issue as you can still slow it down and the animation will stop when the new direction is set anyway

        
        float startTime = Time.time;
        float currentTime = 0;

        while (currentTime < 1 && !playerTransform.up.Equals(newDir))
        {
            currentTime = (Time.time - startTime) * changeDirAnimationSpeed;
            rotationGhost.transform.rotation = Quaternion.LookRotation(newDir, -camTransform.forward);  // For now, we are using a ghost, which is not very clean
            rotationGhost.transform.Rotate(Vector3.right * 90f);                                        // but this is the only (known) way we can apply a rotation on a certain axis

            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, rotationGhost.transform.rotation, currentTime);
            yield return null;
        }

        rotationGhost.transform.rotation = Quaternion.LookRotation(newDir, -camTransform.forward);
        rotationGhost.transform.Rotate(Vector3.right * 90f);

        playerTransform.rotation = rotationGhost.transform.rotation;

        yield return null;
    }

    private void Jump()
    {
        jumpBuffer++;
        isJumping = true;
        hasLeftGround = true;
        StartCoroutine(nameof(ResetIsJumping)); // Avoid spamming

        Vector3 localVelocity = playerTransform.InverseTransformDirection(rb.velocity); // Convert to local space
        if (localVelocity.y < jumpForce)
            localVelocity = new Vector3(localVelocity.x, jumpForce, localVelocity.z); // Use velocity instead of force, better for gameplay
        rb.velocity = playerTransform.TransformDirection(localVelocity);
    }

    private void TouchGround()
    {
        jumpBuffer = 0;
        hasLeftGround = false;
    }
    private void LeaveGround()
    {
        jumpBuffer++;
        hasLeftGround = true;
    }

    private IEnumerator ResetIsJumping()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        isJumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feet.position, 0.3f);
    }
}
