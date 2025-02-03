using System;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class player : Obj
{
    public bool looking;
    public ReticleScript reticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // lookDirection = reticle.transform.position - transform.position;
        // lookDirection = lookDirection.normalized;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 inputLook = context.ReadValue<Vector2>();
        if (looking) {
            Look(inputLook);
        }
        // if (reticle.distFromOrigin > reticle.thresholdDist) {
        //     turnSpeedScalar = 0.5f;
        // }
        // else {
        //     turnSpeedScalar = 1f;
        // }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) {
            Jump();
        }
    }

    public void OnEscape()
    {
        if (looking) {
            looking = false;
            reticle.looking = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!looking) {
            looking = true;
            reticle.looking = true;
        }
        if (context.phase == InputActionPhase.Started) {
            weapon.AttackDown(target.transform);
            if (weapon.launched) {
                anim.SetBool("launch", true);
                StartCoroutine(SetBoolAfter("launch", false, 0.03f));
            }
            else {
                anim.SetBool("summon", true);
                StartCoroutine(SetBoolAfter("summon", false, 0.03f));
            }
        }
        if (context.phase == InputActionPhase.Canceled) {
            weapon.AttackUp(target.transform);
            if (weapon.launched) {
                anim.SetBool("launch", true);
                StartCoroutine(SetBoolAfter("launch", false, 0.03f));
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Started) {
            weapon.AimDown();
        }
        if (context.phase == InputActionPhase.Canceled) {
            weapon.AimUp();
        }
    }
}
