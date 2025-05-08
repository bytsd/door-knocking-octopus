using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] private float chaseDistance;
    [SerializeField] private float stunDuration;

    float timer;

    protected override void Start()
    {
        base.Start();
        ChangeState(EnemyStates.Bat_Idle);
    }

    protected override void UpdateEnemyStates()
    {
        float _dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        switch (currentEnemyState)
        {
            case EnemyStates.Bat_Idle:
                rb.linearVelocity = new Vector2(0, 0);
                if (_dist < chaseDistance)
                {
                    ChangeState(EnemyStates.Bat_Chase);
                }
                break;

            case EnemyStates.Bat_Chase:
                rb.MovePosition(Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, Time.deltaTime * speed));
                
                FlipBat();

                if (_dist > chaseDistance)
                {
                    ChangeState(EnemyStates.Bat_Idle);
                }
                break;
            
            case EnemyStates.Bat_Stunned:
                timer += Time.deltaTime;

                if(timer > stunDuration)
                {
                    ChangeState(EnemyStates.Bat_Idle);
                    timer = 0;
                }
                break;

            case EnemyStates.Bat_Death:

                break;
        }
    }

    public override void EnemyGetsHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        base.EnemyGetsHit(_damageDone, _hitDirection, _hitForce);

        if(health > 0)
        {
            ChangeState(EnemyStates.Bat_Stunned);
        }
        else
        {
            ChangeState(EnemyStates.Bat_Death);
        }
    }

    void FlipBat()
    {
        if(PlayerController.Instance.transform.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

}
