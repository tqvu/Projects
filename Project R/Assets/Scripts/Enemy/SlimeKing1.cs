using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing1 : StateMachineBehaviour
{
    Enemy self;
    float cooldown = 3;
    float currentTime;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 1.2f;
        self = animator.GetComponent<Enemy>();
        currentTime = cooldown;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (self.Health <= self.maxHealth * .67f && self.GetComponent<SlimeKing>().stage == 1)
        {
            self.Health = Convert.ToInt32(self.maxHealth * .67f);
            self.GetComponent<SlimeKing>().stage += 1;
            animator.SetTrigger("Stage2");
        }
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            animator.SetTrigger("Jump");
        }
        if (FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize < 1.2f)
        {
            FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize += Time.deltaTime;
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
