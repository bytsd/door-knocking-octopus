using UnityEngine;

public class Boss_Dive : StateMachineBehaviour
{
    Rigidbody2D rb;
    bool callOnce;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       rb = animator.GetComponentInParent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       TestBoss.Instance.divingCollider.SetActive(true);

       if(TestBoss.Instance.Grounded())
        {
            TestBoss.Instance.divingCollider.SetActive(false);

            if(!callOnce)
            {
                TestBoss.Instance.DivingPillars();
                animator.SetBool("Dive", false);
                TestBoss.Instance.ResetAllAttacks();
                callOnce = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       callOnce = false;
    }

}
