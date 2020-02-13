using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float movementDrag = 8f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private int jumpLimit = 1;
    [SerializeField] private Transform camTransform = default;
    [SerializeField] private Transform feet = default;
    [SerializeField] private LayerMask groundLayer = default;

    private Rigidbody rb;
    private Transform playerTransform;
    private int jumpBuffer = 0;
    private bool isGrounded = false;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
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
        rb.velocity -= 
            (rb.velocity.x * movementDrag * Time.fixedDeltaTime * Vector3.right +
            rb.velocity.z * movementDrag * Time.fixedDeltaTime * Vector3.forward);
    }

    private void Update()
    {
        if (!isJumping)
            if (isGrounded = Physics.OverlapSphere(feet.position, 0.3f, groundLayer).Length > 0)
                TouchGround();

        if (Input.GetButtonDown("Jump") && !isJumping && jumpBuffer < jumpLimit)
            Jump();
    }

    public IEnumerator ChangeGravity(Vector3 newDir)
    {
        Vector3 playerUp;
        Vector3 playerForward;
        while (playerTransform.up != -newDir)
        {
            playerUp = playerTransform.up;
            playerForward = camTransform.forward;
            Vector3 targetRotation = Quaternion.RotateTowards(Quaternion.LookRotation(playerForward), Quaternion.LookRotation(playerForward, -newDir), 180f * Time.deltaTime).eulerAngles;
            rb.MoveRotation(Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z));
            //camTransform.Rotate(Vector3.right * targetRotation.x);
            yield return null;
        }
    }

    private void Jump()
    {
        jumpBuffer++;
        isJumping = true;
        StartCoroutine(nameof(ResetIsJumping)); // Avoid spamming
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
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
