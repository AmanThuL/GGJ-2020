using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Yuan Luo
public class Luobojing : EnemyPhysics
{
    enum state
    {
        wander,
        attack
    }
    state currentState;
    public Animator animator;
    private Character player;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();

        player = Character.GetCharacter();

        currentState = state.wander;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);
    }

    private void FixedUpdate()
    {


        if (currentState == state.wander)
        {
            base.Wandering();

            if(Vector3.Distance(player.transform.position, rb.position) < 20f)
            {
                currentState = state.attack;
            }


        }

        if(currentState == state.attack)
        {
            rb.velocity = Vector3.zero;
            animator.SetTrigger("Attack");

     
            

        }
    }

    public override void Attack()
    {
        
    }
}
