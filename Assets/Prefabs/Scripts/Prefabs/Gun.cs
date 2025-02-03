using System;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : Weapon
{
    public Transform lerp;
    public Transform hand;
    public Transform ads;
    public float lerpSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lerp = hand;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform != lerp) {
            if (transform.position != lerp.position) {
                transform.position = Vector3.Lerp(transform.position, lerp.position, lerpSpeed * Time.deltaTime);
            }
            if (transform.rotation != lerp.rotation) {
                transform.rotation = Quaternion.Lerp(transform.rotation, lerp.rotation, lerpSpeed * Time.deltaTime);
            }
        }
    }

    public override void AimDown() {
        Debug.Log("aim down");
        lerp = ads;
    }

    public override void AimUp() {
        Debug.Log("aim up");
        lerp = hand;
    }

    public override void LaunchAttack(Transform target) {
        base.LaunchAttack(target);
        anim.Play("Shoot", 0, 0f);
    }
}
