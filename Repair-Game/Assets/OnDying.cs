﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDying : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //ParticleSystem[] parts = animator.gameObject.GetComponent<Luobojing>().particleSystems;
        //foreach (ParticleSystem part in parts)
        //{
        //    if (!part.isPlaying)
        //    {
        //        part.Play();
        //    }
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Luobojing lbj = animator.GetComponentInParent<Luobojing>();
        for(int i = 0; i < Random.Range(0,lbj.avgLoot); i++)
        {
            Instantiate(lbj.coin, lbj.transform.position, Quaternion.identity);
        }
        animator.GetComponentInParent<Luobojing>().Die();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
