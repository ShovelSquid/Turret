using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class Projectile : Attack
{
    public float maxSpeed;
    public float speed;
    public Rigidbody rb;

    public override void Start()
    {
        // Debug.Log("projectile start");
        speed = CalculateSpeed();
    }

    public override void Launch(Transform target)
    {
        base.Launch(target);
        speed = CalculateSpeed();
        SetRotation(target);
        Debug.Log("Projectile Launch");
        rb.isKinematic = false;
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        FixedUpdate();
    }

    public float CalculateSpeed()
    {
        if (doesCharge) {
            var s = maxSpeed * charge;
            // Debug.Log("does charge, " + s);
            return s;
        }
        // Debug.Log("not firing? returning " + maxSpeed);
        return maxSpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        RaycastHit[] hits = rb.SweepTestAll(transform.forward, rb.linearVelocity.magnitude*Time.deltaTime);
        Debug.DrawRay(transform.position, transform.forward * rb.linearVelocity.magnitude * Time.deltaTime*1f, Color.red);
        if (hits.Length > 0)
        {
            // Debug.Log("trigger collision: " + hits[0].collider.gameObject.name);
            if (CheckHit(hits[0].collider.gameObject)) {
                Obj hit = hits[0].collider.gameObject.GetComponent<Obj>();
                Hit(hit);
            }
        }
        // Debug.DrawRay(transform.position, direction * 100f, Color.red);
    }

    protected override void HitGround()
    {
        // Debug.Log("hit ground!!");
        Die();
    }
}
