using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Obj : MonoBehaviour
{
    public GameObject head;
    public Rigidbody rb;
    public Weapon weapon;
    public Animator anim;
    public int health;
    public bool vulnerable = false;
    public bool resistant = false;
    public bool invincible = false;
    public float iFrames;
    public int speed;
    public int walkSpeed;
    public int sprintSpeed;
    public int groundJumpForce;
    public int airJumpForce;
    public bool mobile;
    public bool isStatic;
    public float moveLerp = 5f;
    public Vector3 CurrentPos;
    public Vector3 lookDirection;
    public Vector3 CurrentRotAngles;
    public Quaternion CurrentRot;
    public Vector2 inputMove;
    public Transform target;
    public bool attackOnCooldown = false;
    public bool attackCharging;
    public bool attackCharged;
    public float moveAcceleration;
    public Vector3 velocity;
    public Vector3 turnSpeed;
    public float turnSpeedScalar = 1f;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    public int airJumpsTotal;
    public int airJumps;
    public bool inAir;
    public bool onGround;
    public bool moving;
    // public Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        // head = transform.Find("head").gameObject;
        // rb = GetComponent<Rigidbody>();
        // weapon = GetComponentInChildren<Weapon>();
        // anim = GetComponentInChildren<Animator>();
        if (mobile == true) {
            mobile = false;
            CurrentPos = transform.position;
            mobile = true;
        }
        else {
            CurrentPos = transform.position;
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (!isStatic) {
            if (mobile) {
                // lerp to currentpos
                // if (Vector3.Distance(transform.position, CurrentPos) < 0.001 && Vector2.Distance(transform.position, CurrentPos) > 0.00000001) {
                //     transform.position = CurrentPos;
                // }
                // else if (Vector3.Distance(transform.position, CurrentPos) != 0) {
                //     transform.position = Vector3.Lerp(transform.position, CurrentPos, moveLerp * Time.deltaTime);
                // }
                // if (moving) {

                // }
                if (inputMove != Vector2.zero) {
                    // velocity = Vector3.Lerp()
                    ApplyMovement();
                }
            }
            if (!onGround) {
                anim.SetFloat("yVelocity", rb.linearVelocity.y);
            }
            Vector3 xyvelo = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            anim.SetFloat("speed", xyvelo.magnitude);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("collision detected!");
        if (collision.gameObject.layer == LayerMask.NameToLayer("groundLayer")) {
            if (inAir) {
                Debug.Log("landed on ground");
                inAir = false;
                onGround = true;
                anim.SetBool("onGround", onGround);
                LandOnGround();
            }
            // CurrentPos = transform.position;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("groundLayer")) {
            inAir = true;
            onGround = false;
            anim.SetBool("onGround", onGround);
        }
    }

    public void DealDamage(int damage)
    {
        if (vulnerable) {
            damage *= 2;
        }
        if (resistant) {
            var dam = damage * 0.5f;
            damage = Mathf.RoundToInt(dam);
        }
        if (invincible) {
            return;
        }
        Damage(damage);
        StartCoroutine(Invincibility(iFrames));
    }

    public void Damage(int damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage!");
        Debug.Log($"{gameObject.name} now has {health} health");
        if (health <= 0) {
            Die();
        }
    }

    public void Attack()
    {
        Debug.Log("attack!");
        if (weapon.canAttack()) {
            if (weapon.charged || weapon.charging) {
                weapon.AttackUp(target.transform);
                if (weapon.launched) {
                    anim.SetTrigger("launch");
                }
                else {
                    Debug.Log("wtf");
                }
            }
            else {
                weapon.AttackDown(target.transform);
                if (weapon.launched) {
                    Debug.Log("setting trigger launch");
                    anim.SetTrigger("launch");
                }
                else {
                    Debug.Log("cheese");
                    anim.SetTrigger("summon");
                }
                Debug.Log("weaponation");
            }
        }
    }



    // public void Drop()
    // {

    // }

    public void Die()
    {
        // Drop(inventory);
        Destroy(gameObject);
    }

    public void Move(Vector2 direction)
    {
        // uses x y planar direction; x correlates to left right, y to z
        // Vector3 forward = transform.forward * direction.y;
        // Vector3 side = transform.right * direction.x;
        inputMove = direction;
        if (direction == Vector2.zero) {
            moving = false;
            anim.SetBool("moving", moving);
            velocity = new Vector3(0, velocity.y, 0);
            // anim.SetFloat("speed", 0);
            // rb.linearDamping = 10000;
            if (weapon != null) {
            if (weapon.anim != null) {
                weapon.anim.SetBool("running", false);
            }
        }
            return;
        }
        if (!moving) {
            moving = true;
            anim.SetBool("moving", moving);
        }
        // rb.linearDamping = 0;
        Vector3 forward = transform.forward * inputMove.y;
        Vector3 right = transform.right * inputMove.x;
        Vector3 middle = (forward + right).normalized;
        var xyvelo = new Vector3(middle.x * speed, 0, middle.z * speed);
        // anim.SetFloat("speed", xyvelo.magnitude);
        velocity = new Vector3(xyvelo.x, velocity.y, xyvelo.z);

        if (weapon != null) {
            if (weapon.anim != null) {
                weapon.anim.SetBool("running", true);
            }
        }
    }

    public void Look(Vector2 direction)
    {
        // x rotates around y axis, y rotates around z axis
        // CurrentRotAngles += new Vector3(direction.y*turnSpeed.y*turnSpeedScalar*Time.deltaTime,
        //                                 direction.x*turnSpeed.x*turnSpeedScalar*Time.deltaTime, 0f);
        // CurrentRot.eulerAngles = CurrentRotAngles;
        Quaternion bodyrotation = transform.rotation;
        bodyrotation.eulerAngles += new Vector3(0, direction.x*turnSpeed.x*turnSpeedScalar*Time.deltaTime, 0f);
        Quaternion headrotation = head.transform.localRotation;
        headrotation.eulerAngles += new Vector3(direction.y*turnSpeed.y*turnSpeedScalar*Time.deltaTime, 0f, 0f);
        transform.rotation = bodyrotation;
        head.transform.localRotation = headrotation;
        Vector3 forward = transform.forward * inputMove.y;
        Vector3 right = transform.right * inputMove.x;
        Vector3 middle = (forward + right).normalized;
        velocity = new Vector3(middle.x * speed, velocity.y, middle.z * speed);
    }

    public void Jump()
    {
        Debug.Log("trying jump");
        if (canJump()) {
            var jumpForce = groundJumpForce;
            if (inAir) {
                airJumps -= 1;
                jumpForce = airJumpForce;
            }
            Debug.Log("jumping");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    public bool canJump() {
        if (inAir && !onGround && airJumps <= 0) {
            return false;
        }
        return true;
    }

    public void ApplyMovement()
    {
        // CurrentPos += velocity * Time.deltaTime;
        // rb.linearVelocity = velocity;
        Vector3 maxVelocity = transform.rotation * new Vector3(speed, 0, speed);
        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }

    public void ApplyMoveAcceleration()
    {
        // Vector3 dir = inputMove;
        // Vector3.RotateTowards(dir, transform.forward, 5f, 0f);
        // velocity += dir * moveAcceleration * Time.deltaTime;
    }

    public void ApplyGravity()
    {
        velocity += gravity * Time.deltaTime;
    }

    public void LandOnGround()
    {
        airJumps = airJumpsTotal;
    }

    public IEnumerator Invincibility(float time) {
        invincible = true;
        yield return new WaitForSeconds(time);
        invincible = false;
    }

    public IEnumerator SetBoolAfter(string boolname, bool truth, float time) {
        yield return new WaitForSeconds(time);
        anim.SetBool(boolname, truth);
    }
}
