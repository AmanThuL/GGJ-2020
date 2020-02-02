using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Yuan Luo
public class Luobojing2 : EnemyPhysics
{
    enum state
    {
        wander,
        attack
    }
    state currentState;
    public Animator animator;
    private GameObject player;

    private float attackTicker;
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
            if(Vector3.Distance(player.transform.position, rb.position) < 20f && attackTicker <= 0 && rb.velocity.magnitude < 0.3f)
            {
                wanderRadius = 4;
                currentState = state.attack;
            }
            
            attackTicker -= Time.fixedDeltaTime;
            Debug.Log(attackTicker);
        }

        if(currentState == state.attack)
        {
            Debug.Log("attack");
            rb.velocity = Vector3.zero;
            Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.yellow);
            //transform.forward = (player.transform.position - gameObject.transform.position).normalized;
            animator.SetTrigger("Attack");
            //StartCoroutine(Attack1());

            Attack();

            attackTicker = 0;
            attackTicker += attackCooldown;

            currentState = state.wander;
        }
    }

    public override void Attack()
    {
        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
        obj.GetComponent<buuu>().direction = (player.transform.position - transform.position).normalized;
    }

    private IEnumerator Attack1()
    {
        yield return new WaitForSeconds(0.5f);  
    }
}
