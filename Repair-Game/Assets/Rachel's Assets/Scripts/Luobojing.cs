using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Yuan Luo
public class Luobojing : Enemy
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
        player = GameObject.FindWithTag("Player");
        target = player;
        attackTicker += attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {

        base.Update();
        animator.SetFloat("Velocity", rb.velocity.magnitude / maxSpeed);

        attackTicker -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetDeadState();
        }
        switch (state)
        {
            case State.Wander:


                Wandering();

                transform.forward = rb.velocity.normalized;

                //If player in range and not in cooldown, attack
                if (Vector3.Distance(player.transform.position, rb.position) < fov && attackTicker <= 0 && reached)
                {

                    wanderRadius = 6;
                    state = State.Attack;
                }

                break;
            case State.Attack:

                rb.velocity = Vector3.zero;

                Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.yellow);

                rb.velocity = (player.transform.position - gameObject.transform.position).normalized * 0.03f;

                transform.forward = rb.velocity.normalized;

                if (attackTicker <= 0 && !isAttacking)
                {

                    rb.velocity = (player.transform.position - gameObject.transform.position).normalized * 0.05f;

                    transform.forward = rb.velocity.normalized;

                    isAttacking = true;
                    animator.SetTrigger("Attack");
                }

                break;
            case State.Defensive:
                // Slowly back away
                break;
            case State.Pursuit:

                AgroWandering();

                transform.forward = rb.velocity.normalized;

                //If player in range and not in cooldown, attack
                if (Vector3.Distance(player.transform.position, rb.position) < fov && attackTicker <= 0 && reached)
                {

                    wanderRadius = 6;
                    state = State.Attack;
                }

                break;
            case State.Dead:
                // Insert dead animation
                animator.SetTrigger("Death");
                gameObject.SetActive(false);
                break;
        }

        //transform.forward = rb.velocity.normalized;
    }

    public override void Attack()
    {
        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
        obj.GetComponent<MonsterBullet>().direction = (player.transform.position - transform.position).normalized;

        rb.velocity = (player.transform.position - gameObject.transform.position).normalized * 0.03f;

        attackTicker = 0;
        attackTicker += attackCooldown;
    }

    private void AgroWandering()
    {
        Debug.Log("AGRO");
        //Get a vector towards player
        Vector3 toPlayer = Vector3.ClampMagnitude(player.transform.position - rb.position, wanderRadiusOffset);

        //If not at destination, go to destination
        if (!reached)
        {
            RigidGoTo(wanderDestination + toPlayer);
        }

        //If reached
        if (Vector3.Distance(wanderDestination, rb.position) < 0.8f && !reached)
        {
            //if just reached, start cooldown
            if (reached == false)
            {
                wanderTicker += (wanderCooldown + Random.Range(-wanderCooldownOffset, wanderCooldownOffset));
            }

            reached = true;
            rb.velocity *= 0.1f; //Slow down
        }

        if (wanderTicker >= 0) wanderTicker -= Time.deltaTime;

        //if done chilling
        if (wanderTicker <= 0 && reached == true)
        {
            wanderTicker = 0;
            wanderDestination = GetRandomClosePosition(wanderRadius + Random.Range(-wanderRadiusOffset, wanderRadiusOffset)) + toPlayer;
            reached = false;
        }
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
