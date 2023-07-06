using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichIdle2 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    Enemy self;
    float cooldown = 3;
    float currentTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<Enemy>();
        currentTime = cooldown;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.Health <= self.maxHealth * .33f)
        {
            Convert.ToInt32(self.maxHealth * .33f);
            animator.SetTrigger("Stage 3");
        }
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            animator.SetTrigger("Cast");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }


}
