using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeCameraController : MonoBehaviour
{
    [SerializeField] private bool hideMouse = true;

    [SerializeField] private float flatMoveSpeed = 10;
    [SerializeField] private float verticalMoveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    private float pitch;

    private void Awake()
    {
        HandleHideMouse();
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleHideMouse()
    {
        if (!hideMouse) return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleLook()
    {
        float mouseXAxis = Input.GetAxis("Mouse X");
        float mouseYAxis = Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch - mouseYAxis * mouseSensitivity, minPitch, maxPitch);
        float yaw = transform.eulerAngles.y + mouseXAxis * mouseSensitivity;

        transform.eulerAngles = new Vector3(pitch, yaw, transform.eulerAngles.z);
    }

    private void HandleMovement()
    {
        Vector3 moveVector = default;

        moveVector += transform.forward * Input.GetAxis("Vertical") * flatMoveSpeed;
        moveVector += transform.right * Input.GetAxis("Horizontal") * flatMoveSpeed;
        moveVector += Input.GetKey(KeyCode.E) ? Vector3.up * verticalMoveSpeed : Vector3.zero;
        moveVector += Input.GetKey(KeyCode.Q) ? Vector3.down * verticalMoveSpeed : Vector3.zero;

        transform.position += moveVector * Time.deltaTime; // Clamp position
    }
}
