using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Yuan Luo
public class Luobojing2 : Enemy
{
    public Animator animator;

    [SerializeField] private GameObject player;

    public float attackTicker;
    public float attackCooldown;

    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        state = State.Wander;
        gameObject.GetComponent<SphereCollider>().radius = fov;
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Character-girl");
        target = player;
        attackTicker += attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);
    }

    protected void FixedUpdate()
    {
        switch (state)
        {
            case State.Wander:
                Wandering();

                animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);
                transform.forward = rb.velocity.normalized;

                //If player in range and not in cooldown, attack
                if (Vector3.Distance(player.transform.position, rb.position) < fov && attackTicker <= 0 && reached)
                {
                    wanderRadius = 4;
                    state = State.Attack;
                }

                attackTicker -= Time.fixedDeltaTime;

                break;
            case State.Attack:

                rb.velocity = Vector3.zero;

                Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.yellow);

                rb.velocity = (player.transform.position - gameObject.transform.position).normalized * 0.03f;

                if (attackTicker <= 0 && !isAttacking)
                {
                    isAttacking = true;
                    animator.SetTrigger("Attack");
                }

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

    public override void Attack()
    {
        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
        obj.GetComponent<MonsterBullet>().direction = (player.transform.position - transform.position).normalized;

        rb.velocity = (player.transform.position - gameObject.transform.position).normalized * 0.03f;

        attackTicker = 0;
        attackTicker += attackCooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        return;
    }

    private void OnTriggerStay(Collider other)
    {
        return;
    }

    private void OnTriggerExit(Collider other)
    {
        return;
    }


    public void SetWanderState()
    {
        state = State.Wander;
    }

    public void SetAttackState()
    {
        state = State.Attack;
    }

    public void SetDefensiveState()
    {
        state = State.Defensive;
    }

    public void SetPursuitState()
    {
        state = State.Pursuit;
    }
    public void SetDeadState()
    {
        state = State.Dead;
    }
}
