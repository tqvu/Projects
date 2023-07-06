using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing2 : StateMachineBehaviour
{
    Enemy self;
    float cooldown = 3;
    float currentTime;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        self = animator.GetComponent<Enemy>();
        currentTime = cooldown;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.Health <= self.maxHealth * .33f)
        {
            self.Health = Convert.ToInt32(self.maxHealth * .33f);
            self.GetComponent<SlimeKing>().stage += 1;
            animator.SetTrigger("Stage3");
        }
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            switch(UnityEngine.Random.Range(0, 2))
            {
                case 0:
                    animator.SetTrigger("Jump");
                    break;
                case 1:
                    animator.SetTrigger("Shoot");
                    break;
            }
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
