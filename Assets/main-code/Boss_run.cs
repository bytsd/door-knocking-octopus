using UnityEngine;

public class Boss_run : StateMachineBehaviour
{
    Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       rb = animator.GetComponentInParent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TargetPlayerPosition(animator);

        if(TestBoss.Instance.attackCountdown <= 0)
        {
            TestBoss.Instance.AttackHandler();
            TestBoss.Instance.attackCountdown = Random.Range(TestBoss.Instance.attackTimer - 1, TestBoss.Instance.attackTimer + 1);
        }
    }

    void TargetPlayerPosition(Animator animator)
    {
        if(TestBoss.Instance.Grounded())
        {
            TestBoss.Instance.Flip();
            Vector2 _target = new Vector2(PlayerController.Instance.transform.position.x, rb.position.y);
            Vector2 _newPos = Vector2.MoveTowards(rb.position, _target, TestBoss.Instance.runSpeed * Time.fixedDeltaTime);
            rb.MovePosition(_newPos);
        }
        else
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -25);
        }
        if(Vector2.Distance(PlayerController.Instance.transform.position, rb.position) <= TestBoss.Instance.attackRange)
        {
            animator.SetBool("Run", false);
        }
        else
        {
            return;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Run", false);
    }
}
