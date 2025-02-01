using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // public bool startOnDown;
    public bool releaseOnDown;
    public bool doesCharge;
    public bool on;
    public bool faceTarget;
    public float chargeTime;
    private Vector3 direction;
    public float baseCharge;
    public float maxCharge;
    public float charge;
    public int damage;
    public Transform target;
    public float lifetime;
    public List<Object> damaged;
    public List<GameObject> collided;
    public List<String> friendlyTags;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        // StartCoroutine(End());
        Debug.Log("Attack start");
        chargeTime = 0;
    }
    protected virtual void FixedUpdate()
    {
        if (faceTarget) {
            SetRotation(target);
        }
        if (!on) {
            return;
        }
    }
    public virtual void SetCharge(float chrge)
    {
        charge = chrge;
    }

    public virtual void Launch(Transform target)
    {
        StartCoroutine(End());
        faceTarget = false;
    }

    public void OnTriggerEnter(Collider collider) {
        if (!on) {
            return;
        }
        Debug.Log($"detected trigger with {collider.gameObject.name}");
        if (CheckHit(collider.gameObject)) {
            Hit(collider.gameObject.GetComponent<Object>());
        }
    }

    // make it a return object, check for null type situation
    public bool CheckHit(GameObject gob) {
        // checks for object component and friendly tags to see if can hit
        var obj = gob.GetComponent<Object>();
        if (obj != null) {
            if (damaged.Contains(obj)) {
                return false;
            }
            else if (friendlyTags.Contains(gob.tag)) {
                return false;
            }
            return true;
        }
        else if (gob.layer == 6) {
            Debug.Log("hit ground layer");
            HitGround();
        }

        return false;
    }

    public void Hit(Object obj) {
        Damage(obj);
        damaged.Add(obj);
    }

    public void SetRotation(Transform target) {
        direction = target.position - transform.position;
        Quaternion rotation = transform.rotation;
        rotation.SetLookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    protected virtual void HitGround() {
        Debug.Log("hit ground virtual base!! wow!");
        // nothing
    }

    // public virtual int CalculateDamage()
    // {
    //     if (doesCharge) {
    //         float dam = damage * charge;
    //         return (int)Math.Round(dam);
    //     }
    //     return damage;
    // }

    public void Damage(Object thing)
    {
        thing.DealDamage(damage);
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator End() {
        yield return new WaitForSeconds(lifetime);
        Die();
    }
}
