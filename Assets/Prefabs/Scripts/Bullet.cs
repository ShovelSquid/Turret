using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : Entity
{
    public float baseDamage;
    public float maxDamage;
    public float damage;
    public bool spent;
    public float lifetime;

    private void Start() {
        maxDamage = baseDamage;
        damage = baseDamage;
        StartCoroutine(Lifetime());
    }
    public void Hit(Object target) {
        var healthleft = target.Damage(damage);
        if (healthleft < 0) {
            damage = healthleft;
            speed = maxSpeed*(damage/maxDamage);
        }
        else if (healthleft >= 0) {
            Expire();
        }
    }

    public void Expire() {
        spent = true;
        Destroy(gameObject);
    }

    private IEnumerator Lifetime() {
        yield return new WaitForSeconds(lifetime);
        Expire();
    }

}
