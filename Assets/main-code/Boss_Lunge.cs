using UnityEngine;

public class Boss_Lunge : StateMachineBehaviour
{
    Rigidbody2D rb;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponentInParent<Rigidbody2D>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       rb.gravityScale = 0;
       int _dir = TestBoss.Instance.facingRight ? 1 : -1;
       rb.linearVelocity = new Vector2(_dir * (TestBoss.Instance.speed * 5), 0f);

       if(Vector2.Distance(PlayerController.Instance.transform.position, rb.position) <= TestBoss.Instance.attackRange &&
            TestBoss.Instance.damagedPlayer)
        {
            PlayerController.Instance.TakeDamage(TestBoss.Instance.damage);
            TestBoss.Instance.damagedPlayer = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}
