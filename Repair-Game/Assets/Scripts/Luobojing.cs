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
    [SerializeField] private GameObject player;

    public float attackTicker;
    public float attackCooldown;

    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();

        player = GameObject.Find("Character-girl");
        Debug.Log(player.transform.position.x);

        currentState = state.wander;

        attackTicker += attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);

    }

    void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentState == state.wander)
        {
            base.Wandering();

            animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);
            transform.forward = rb.velocity.normalized;

            //If player in range and not in cooldown, attack
            if(Vector3.Distance(player.transform.position, rb.position) < 15f && attackTicker <= 0 && reached)
            {
                wanderRadius = 4;
                currentState = state.attack;
            }
            
            attackTicker -= Time.fixedDeltaTime;
        }

        if(currentState == state.attack)
        {
            rb.velocity = Vector3.zero;

            Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.yellow);

            rb.velocity = (player.transform.position - gameObject.transform.position).normalized * 0.03f;
            
            if(attackTicker <= 0 && !isAttacking)
            {
                isAttacking = true;
                animator.SetTrigger("Attack");
            }
            
            
            //currentState = state.wander;
        }
    }

    public override void Attack()
    {
        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
        obj.GetComponent<buuu>().direction = (player.transform.position - transform.position).normalized;

        rb.velocity = (player.transform.position - gameObject.transform.position).normalized * 0.03f;

        attackTicker = 0;
        attackTicker += attackCooldown;
    }

    public void SetAttackState()
    {
        currentState = state.attack;
    }

    public void SetWanderState()
    {
        currentState = state.wander;
    }
}
