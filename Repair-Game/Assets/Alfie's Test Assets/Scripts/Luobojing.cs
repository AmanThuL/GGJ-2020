using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luobojing : Enemy
{

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        base.Wander();

        animator.SetFloat("Velocity", velocity.magnitude / speed);
    }
}
