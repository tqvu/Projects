using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichIdle3 : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    Enemy self;
    float cooldown = 2;
    float currentTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<Enemy>();
        currentTime = cooldown;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.Health <= 0)
        {
            self.health = 0;
            animator.SetTrigger("Death");
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
