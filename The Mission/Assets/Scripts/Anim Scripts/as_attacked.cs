using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class as_attacked : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public GameObject attackedto;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AI_Enemy ai = animator.GetComponent<AI_Enemy>();

        if (ai != null)
        {
            animator.ResetTrigger("Hit");
            attackedto = ai.FocusOn;
            if (Vector3.Distance(ai.FocusOn.transform.position, animator.transform.position) > ai.DistanceBetweenHostage) return;

            ai.hasSeen.Remove(attackedto);
        }

        Debug.Log("The Attack Animation's Finished. " + animator + " " + attackedto);

        if (attackedto.tag == "Hostage")
        {
            

            AI_Hostage hostage = attackedto.GetComponent<AI_Hostage>();
            

            if (hostage.alive != false)
            {
                hostage.Kill();
              
            }

            ai.FocusOn = null;

        }


    }

    /*

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        


    }*/

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        animator.SetBool("IsMoving", false);
        animator.SetBool("IsWalking", false);

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
