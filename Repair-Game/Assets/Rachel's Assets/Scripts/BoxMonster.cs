using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMonster : Enemy
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        state = State.Wander;
        gameObject.GetComponent<SphereCollider>().radius = fov;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected void FixedUpdate()
    {
        switch (state)
        {
            case State.Wander:
                Wandering();
                break;
            case State.Attack:
                // Not yet implemented
                break;
            case State.Defensive:
                // Slowly back away
                break;
            case State.Pursuit:
                Seek(target);
                break;
            case State.Dead:
                // Insert dead animation
                gameObject.SetActive(false);
                break;
        }
        animator.SetFloat("Speed", rb.velocity.magnitude / maxSpeed);
        transform.forward = rb.velocity.normalized;

    public override void Attack()
    {
        // TODO
    }
}
