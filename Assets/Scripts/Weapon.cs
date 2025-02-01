using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Attack attack;
    public float attackCharge;
    public float attackRange;
    public Object wielder;
    public float cooldown;
    public bool onCooldown;
    public bool charging;
    public bool charged;
    public bool launched;
    private Coroutine chargeAttack;
    public GameObject instanceObj;
    public Attack instanceAtk;
    public Transform target;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AimDown() {
        // lerp = ads;
    }

    public virtual void AimUp() {
        // lerp = hand;
    }

    public void AttackDown(Transform target)
    {
        Debug.Log("Attack Down");
        CreateAttack(target);
        if (attack.releaseOnDown) {
            LaunchAttack(target);
        }
        else if (attack.doesCharge) {
            chargeAttack = StartCoroutine(ChargeAttack(attack.chargeTime, attack.maxCharge, attack.baseCharge));
        }
    }

    public void AttackUp(Transform target)
    {
        Debug.Log("Attack Up");
        if (!attack.releaseOnDown) {
            LaunchAttack(target);
            if (attack.doesCharge) {
                StopCoroutine(chargeAttack);
                chargeAttack = null;
            }
        }
    }

    public void CreateAttack(Transform target)
    {
        Debug.Log("Create Attack (should be setting instanceobj and instanceatk)");
        instanceObj = Instantiate(attack.gameObject, transform);
        instanceAtk = instanceObj.GetComponent<Attack>();
        instanceAtk.target = target;
    }

    public virtual void LaunchAttack(Transform target)
    {
        Debug.Log("Launch Attack; instance obj: " + instanceObj.name);
        instanceObj.transform.SetParent(null);
        instanceAtk.on = true;
        instanceAtk.damage = damage;
        instanceAtk.friendlyTags.Add(wielder.tag);
        // var angle = Mathf.Atan2(direction.y, direction.x);
        // angle = angle * Mathf.Rad2Deg;
        // obj.transform.Rotate(0, 0, angle);
        if (attack.doesCharge) {
            instanceAtk.SetCharge(attackCharge);
            wielder.attackCharged = false;
            charging = false;
            charged = false;
        }
        instanceAtk.Launch(target);
        onCooldown = true;
        wielder.attackOnCooldown = true;
        launched = true;
        StartCoroutine(AttackCooldown(CalculateCooldown()));
    }

    protected virtual int CalculateDamage()
    {
        if (attack.doesCharge) {
            float dam = damage * attackCharge;
            return (int)Math.Round(dam);
        }
        return damage;
    }

    protected virtual float CalculateCooldown()
    {
        if (attack.doesCharge) {
            float cool = cooldown * attackCharge;
            return cool;
        }
        return cooldown;
    }
    

    IEnumerator ChargeAttack(float chargeTime, float maxCharge, float baseCharge) {
        Debug.Log("Charging Attack");
        float time = 0f;
        attackCharge = baseCharge;
        charging = true;
        while (time < chargeTime) {
            yield return new WaitForSeconds(0.05f);
            time += 0.05f;
            if (time > chargeTime) {
                time = chargeTime;
            }
            attackCharge = maxCharge * (time / chargeTime);
            instanceAtk.SetCharge(attackCharge);
            wielder.anim.SetFloat("charge", attackCharge);
            instanceAtk.damage = CalculateDamage();
        }
        wielder.attackCharged = true;
        charging = false;
        charged = true;
        Debug.Log("Attack Charged");
    }

    IEnumerator AttackCooldown(float time) {
        Debug.Log("Attack on cooldown");
        yield return new WaitForSeconds(time);
        onCooldown = false;
        launched = false;
        wielder.attackOnCooldown = false;
    }

    public bool canAttack() {
        Debug.Log("can attack?");
        if (onCooldown) {
            Debug.Log("no cause on cooldown");
            return false;
        }
        // if (charging) {
        //     return true;
        // }
        // if (charged) {
        //     return true;
        // }
        Debug.Log("yes");
        return true;
    }
}
