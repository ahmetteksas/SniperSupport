using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCleaner : StateMachineBehaviour
{

    public string[] clearAtEnter;
    public string[] cleatAtExit;

    public AnimationSystem _AnimationSystem;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_AnimationSystem != null)
        {
            _AnimationSystem.canDoAction = true;
            _AnimationSystem._animator.SetFloat("AttackFloat", Random.Range(0f, 1f));
            _AnimationSystem._animator.SetFloat("JumpFloat", Random.Range(0f, 1f));
            _AnimationSystem._animator.SetFloat("SlideFloat", Random.Range(0f, 1f));
        }

        /*
        foreach (var signal in clearAtEnter)
        {
            animator.ResetTrigger(signal);//Reset trigger
        }
        */
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_AnimationSystem != null)
        {
            _AnimationSystem.canDoAction = false;
        }
        /*
        foreach (var signal in clearAtEnter)
        {
            animator.ResetTrigger(signal);//Reset trigger
        }
        */
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}