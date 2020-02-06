using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float jumpHeight = 0f;
    [SerializeField] private float playerHeight = 0f;
    [SerializeField] private LayerMask groundLayer = default;

    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private float yVelocity = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        controller.Move(speed * Time.deltaTime * Input.GetAxis("Vertical") * playerTransform.forward + speed * Time.deltaTime * Input.GetAxis("Horizontal") * playerTransform.right);

        isGrounded = Physics.OverlapSphere(playerTransform.position - playerHeight / 2f * playerTransform.up, 0.2f, groundLayer).Length > 0;
        //isGrounded = Physics.Raycast(playerTransform.position, -playerTransform.up, (playerHeight / 2f) * playerTransform.localScale.y + 0.1f, groundLayer);

        controller.stepOffset = (isGrounded) ? 0.2f : 0;

        if (isGrounded && yVelocity < 0f)
            yVelocity = -2f;
        yVelocity += gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded)
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        controller.Move(playerTransform.up * yVelocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerTransform.position - playerHeight / 2f * playerTransform.up, 0.2f);
    }
}