using UnityEngine;

public class Entity : Object
{
    // an entity is an object that has movement and is alive

    public float baseSpeed;
    public float maxSpeed;
    public float speed;
    public GameObject lookTarget;
    public Rigidbody rb;
    public GameObject body;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxSpeed = baseSpeed;
    }

    public void SetRotation(GameObject part, Vector3 target) {
        var direction = target - part.transform.position;
        Quaternion rotation = part.transform.rotation;
        rotation.SetLookRotation(direction, Vector3.up);
        part.transform.rotation = rotation;
    }

    public void SetVelocity(Vector3 direction, float speed) {
        rb.linearVelocity = direction * speed;
    }
}
