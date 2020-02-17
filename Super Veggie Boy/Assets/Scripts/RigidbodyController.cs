using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float movementDrag = 8f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private int jumpLimit = 1;
    [SerializeField] private CameraController camController = default;
    [SerializeField] private Transform camTransform = default;
    [SerializeField] private Transform feet = default;
    [SerializeField] private LayerMask groundLayer = default;

    private Rigidbody rb;
    private Transform playerTransform;
    private int jumpBuffer = 0;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isChangingDir = false;
    private Vector3 gravityDir;
    private GameObject ghost;

    private void Awake()
    {
        gravityDir = Physics.gravity.normalized;
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
        ghost = new GameObject("RotationGhost");
    }

    private void FixedUpdate()
    {
        // Controls
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Move
        rb.AddRelativeForce(new Vector3(speed * Time.fixedDeltaTime * xAxis, 0f, speed * Time.fixedDeltaTime * yAxis), ForceMode.VelocityChange);

        // Large jumps
        if (Input.GetButton("Jump") && Vector3.Dot(rb.velocity, Physics.gravity) < 0f)
            rb.AddForce(-Physics.gravity / 2f, ForceMode.Acceleration);

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
            if (isGrounded = Physics.OverlapSphere(feet.position, 0.3f, groundLayer).Length > 0)
                TouchGround();

        // Jump
        if (Input.GetButtonDown("Jump") && !isJumping && jumpBuffer < jumpLimit)
            Jump();
    }

    public IEnumerator ChangeDir(Vector3 newDir)
    {
        if (!isChangingDir)
        {
            isChangingDir = true;

            float startTime = Time.time;
            float currentTime = 0;

            while (currentTime < 1)
            {
                currentTime = (Time.time - startTime) / 1f;
                Debug.Log(currentTime);
                ghost.transform.rotation = Quaternion.LookRotation(newDir, -camTransform.forward);
                ghost.transform.Rotate(Vector3.right * 90f);

                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, ghost.transform.rotation, currentTime);
                yield return null;
            }

            ghost.transform.rotation = Quaternion.LookRotation(newDir, -camTransform.forward);
            ghost.transform.Rotate(Vector3.right * 90f);

            playerTransform.rotation = ghost.transform.rotation;

            isChangingDir = false;
        }
        yield return null;
    }

    private void Jump()
    {
        jumpBuffer++;
        isJumping = true;
        StartCoroutine(nameof(ResetIsJumping)); // Avoid spamming

        Vector3 localVelocity = playerTransform.InverseTransformDirection(rb.velocity); // Convert to local space
        if (localVelocity.y < jumpForce)
            localVelocity = new Vector3(localVelocity.x, jumpForce, localVelocity.z);
        rb.velocity = playerTransform.TransformDirection(localVelocity);
    }

    private void TouchGround()
    {
        jumpBuffer = 0;
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
