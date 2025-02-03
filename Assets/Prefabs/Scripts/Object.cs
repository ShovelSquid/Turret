using System;
using UnityEngine;

public class Object : MonoBehaviour
{
    // an object is a thing that exists in the world which can be destroyed
    // is a bullet an object?
    // a bullet is an item
    public float baseIntegrity;
    private float maxIntegrity;
    private float integrity;
    public bool destroyed;
    public Resource resource;
    public Healthbar healthbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxIntegrity = baseIntegrity;
        integrity = baseIntegrity;
        destroyed = false;
    }

    public void AlterIntegrity(float amount , Resource type) {
        if (amount < 0) {
            Damage(amount);
        }
        else if (amount > 0) {
            Repair(amount);
        }
        else if (amount == 0) {
            Debug.Log("no effect, silly billy");
            return;
        }
        else {
            Debug.Log("you ain't got none");
            return;
        }
        healthbar.setHealth(integrity);
    }

    public float Damage(float damage) {
        integrity -= damage;
        if (integrity <= 0) {
            Die();
        }
        return integrity;
    }

    public float Repair(float health) {
        integrity += health;
        healthbar.setHealth(health);
        if (integrity >= maxIntegrity) {
            integrity = maxIntegrity;
        }
        return integrity;
    }

    public void Die() {
        destroyed = true;
        Destroy(gameObject);
    }
}
