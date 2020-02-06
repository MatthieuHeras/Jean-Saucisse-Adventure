using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float mouseSensitivity = 50f;

    private CharacterController controller;
    private Transform playerTransform;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        controller.Move(speed * Time.deltaTime * Input.GetAxis("Vertical") * playerTransform.forward + speed * Time.deltaTime * Input.GetAxis("Horizontal") * playerTransform.right);
        playerTransform.Rotate(Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * Vector3.up);
    }
}