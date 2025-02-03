using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Reticle : MonoBehaviour
{
    Vector2 LookAmount;
    Vector2 CurrentPos;
    public Vector2 BasePos;
    // Vector2 Threshold = new Vector2(100, 100);
    public float mouseSensitivity = 0.2f;
    public float controllerSensitivity = 0.2f;
    public bool looking = false;
    public bool lerpToBase;
    public float lerpToBaseStrength;
    Vector3 worldPos;
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BasePos = new Vector2(transform.position.x, transform.position.y);
        CurrentPos = BasePos;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, CurrentPos, 12 * Time.deltaTime);
        if (lerpToBase) {
            CurrentPos = Vector2.Lerp(CurrentPos, BasePos, lerpToBaseStrength * Time.deltaTime);
        }
    }

    public void OnClick()
    {
        looking = true;
    }

    public void OnEscape()
    {
        looking = false;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (looking) {
            LookAmount = context.ReadValue<Vector2>();
            var device = context.control.device;
            var motion = mouseSensitivity;
            if (device is Gamepad) {
                motion = controllerSensitivity;
            }
            CurrentPos = new Vector2(CurrentPos.x + LookAmount.x*motion, CurrentPos.y + LookAmount.y*motion);
        }
    }
}
