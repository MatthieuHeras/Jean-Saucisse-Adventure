using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private Rigidbody rb = default;
    [SerializeField] private Transform feet = default;
    [SerializeField] private LayerMask groundLayer = default;

    private bool isGrounded = false;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector3(speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0f, speed * Time.deltaTime * Input.GetAxis("Vertical")));    
    }

    private void Update()
    {
        isGrounded = Physics.OverlapSphere(feet.position, 0.3f, groundLayer).Length > 0;

        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            rb.AddRelativeForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isJumping = true;
            StartCoroutine(nameof(ResetIsJumping)); // Avoid spamming
        }
    }

    private IEnumerator ResetIsJumping()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        isJumping = false;
    }
}
