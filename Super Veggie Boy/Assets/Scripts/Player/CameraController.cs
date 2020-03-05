using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool isLocked = false;

    [SerializeField] private Transform camTransform = default;
    [SerializeField] private Transform target = default;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private float distance = 6f;
    [SerializeField] private float reactivity = 0.7f;
    [SerializeField] private float mouseSensitivity = 50f;

    private float xCurrentRotation = 0f;
    private Vector2 targetRotation = Vector2.zero;

    private void Start()
    {
        transform.position = target.position + target.forward * distance;

        if (reactivity < 0.05f)
            reactivity = 0.05f;
        else if (reactivity > 1f)
            reactivity = 1f;
    }

    void Update()
    {
        if (isLocked)
            return;

        float xMouse = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float yMouse = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        targetRotation.x -= yMouse; 
        targetRotation.x = Mathf.Clamp(targetRotation.x + xCurrentRotation, -90f, 90f) - xCurrentRotation;
        targetRotation.y += xMouse;
        

        Vector2 realRotation = new Vector2(targetRotation.x * reactivity, targetRotation.y * reactivity);
        
        targetRotation.x -= realRotation.x;
        xCurrentRotation += realRotation.x;
        targetRotation.y -= realRotation.y;

        camTransform.RotateAround(target.position + offset, transform.right, realRotation.x);
        target.Rotate(target.up, realRotation.y, Space.World);
    }
}
