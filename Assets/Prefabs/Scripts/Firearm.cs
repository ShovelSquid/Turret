using System.Collections;
using UnityEngine;

public class Firearm : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletExit;
    public float cooldown;
    public bool onCooldown;
    public int maxAmmo;
    public int ammo;
    public bool hasAmmo;


    public void PullTrigger() {
        Debug.Log("Pull trigger for "+ gameObject.name);
        if (onCooldown) {
            return;
        }
        if (!hasAmmo) {
            return;
        }
        FireBullet();
    }
    public void FireBullet() {
        Debug.Log("Shooting bullet "+bullet+" at "+ gameObject.name);
        var b = Instantiate(bullet, transform);
        b.transform.SetParent(null);
        b.transform.position = bulletExit.position;
        var blt = b.GetComponent<Bullet>();
        blt.SetVelocity(bulletExit.transform.forward, blt.maxSpeed);
        // if (attack.doesCharge) {
        //     instanceAtk.SetCharge(attackCharge);
        //     wielder.attackCharged = false;
        //     charging = false;
        //     charged = false;
        // }
        onCooldown = true;
        ammo -= 1;
        if (ammo <= 0) {
            hasAmmo = false;
        }
        StartCoroutine(Cooldown(cooldown));
    }

    public IEnumerator Cooldown(float cooldownTime) {
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
    }
}
