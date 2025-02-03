using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Obj
{
    public GameObject target;
    public float detectionRadius;
    enum state {
        IDLE,
        LOOKING,
        HUNTING
    }
    state behavior = state.IDLE;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        behavior = state.IDLE;
        IdleState();
    }

    public void IdleState()
    {

    }

    public void LookingState()
    {

    }

    public void HuntingState()
    {

    }

    public void CheckTarget(GameObject target)
    {
        if (Vector2.Distance(target.transform.position, transform.position) <= detectionRadius) {
            behavior = state.HUNTING;
        }
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (behavior == state.IDLE) {
            CheckTarget(target);
        }
        if (behavior == state.LOOKING) {
            CheckTarget(target);
        }
        if (behavior == state.HUNTING) {
            lookDirection = target.transform.position - transform.position;
            Move(lookDirection);
            if (Vector2.Distance(transform.position, target.transform.position) < weapon.attackRange) {
                
            }
        }
    }
}
