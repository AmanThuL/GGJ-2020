using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Yuan Luo
public class Luobojing2 : Enemy
{
    public Animator animator;

    public GameObject bullet;

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
        animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);
        transform.forward = rb.velocity.normalized;
    }
}
