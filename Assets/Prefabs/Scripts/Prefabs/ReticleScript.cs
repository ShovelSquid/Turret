using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ReticleScript : MonoBehaviour
{
    Vector2 LookAmount;
    Vector2 CurrentPos;
    public Vector2 BasePos;
    public float thresholdDist;
    public float distFromOrigin;
    // Vector2 Threshold = new Vector2(100, 100);
    public float mouseSensitivity = 0.2f;
    public float controllerSensitivity = 0.2f;
    public bool looking = false;
    public bool lerpToBase;
    public bool locked;
    public float lerpToBaseStrength;
    public GameObject player;
    RectTransform rect;
    Vector3 worldPos;
    Camera cam;    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform target;
    public void Start()
    {
        cam = Camera.main;
        rect = gameObject.GetComponent<RectTransform>();
        BasePos = new Vector2(rect.position.x, rect.position.y);
        CurrentPos = BasePos;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (locked) {
            return;
        }
        // move reticle
        transform.position = Vector2.Lerp(transform.position, CurrentPos, 12 * Time.deltaTime);
        if (lerpToBase) {
            distFromOrigin = Vector2.Distance(CurrentPos,BasePos);
            if (distFromOrigin > thresholdDist) {
                Vector2 direction = (CurrentPos - BasePos).normalized;
                Vector2 thresholdPoint = BasePos + direction * thresholdDist;
                CurrentPos = Vector2.Lerp(CurrentPos, thresholdPoint, lerpToBaseStrength * Time.deltaTime);
            }
        }
        rect.position = CurrentPos;
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
            // draw camera ray
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rect.position);
            worldPos = cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, 10f));
            RaycastHit hit;
            Vector3 pos = rect.position;
            float dist = Vector3.Distance(cam.transform.position, worldPos);
            Vector3 scale = cam.transform.forward * dist;
            Ray ray = new Ray(worldPos, cam.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
            Physics.Raycast(ray, out hit);
            if (hit.collider == null) {
                target.transform.position = ray.GetPoint(100000f);
            }
            else {
                target.transform.position = hit.point;
            }

        }
    }
}
