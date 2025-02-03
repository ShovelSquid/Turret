using UnityEngine;
using UnityEngine.InputSystem;

public class Turret : Entity
{
    public GameObject rightArm;
    private Firearm rightFirearm;
    public GameObject leftArm;
    private Firearm leftFirearm;
    public GameObject head;

    public void Start() {
        rightFirearm = rightArm.GetComponentInChildren<Firearm>();
        leftFirearm = leftArm.GetComponentInChildren<Firearm>();
    }

    public void OnLook() {
        SetRotation(head, lookTarget.transform.position);
    }

    public void OnRightTrigger(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Started) {
            Debug.Log("right trigger for turret");
            if (rightFirearm != null) {
                rightFirearm.PullTrigger();
            }
            else {
                Debug.Log("right firearm null");
            }
        }
    }

    public void OnLeftTrigger(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Started) {
            Debug.Log("left trigger for turret");
            if (leftFirearm != null) {
                leftFirearm.PullTrigger();
            }
            else {
                Debug.Log("left firearm null");
            }
        }
    }
}
