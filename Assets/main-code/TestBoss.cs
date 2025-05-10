using UnityEngine;
using System.Collections;

public class TestBoss : Enemy
{
    public static TestBoss Instance;
    private Animator anim;

    public GameObject slashEffect;
    public Transform SideAttackTransform, UpAttackTransform, DownAttackTransform;
    public Vector2 SideAttackArea, UpAttackArea, DownAttackArea;

    public float attackRange;
    public float attackTimer;

    [SerializeField] public bool facingRight;

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    int hitCounter;
    bool stunned, canStun;
    bool alive;

    [HideInInspector] public float runSpeed;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    protected override void Start()
    {
        base.Start();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        ChangeState(EnemyStates.TB_Stage1);
        alive = true;
    }

    public bool Grounded()
    {
        if(Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckY, groundLayer)
         || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer)
         || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckX, 0, 0), Vector2.down, groundCheckY, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if(!attacking)
        {
            attackCountdown -= Time.deltaTime;
        }
    }

    public void Flip()
    {
        if(PlayerController.Instance.transform.position.x < transform.position.x && transform.localScale.x > 0)
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 180);
            facingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector2(transform.eulerAngles.x, 0);
            facingRight = true;
        }
    }

    protected override void UpdateEnemyStates()
    {
        if(PlayerController.Instance != null)
        {
            switch(GetCurrentEnemyState)
            {
                case EnemyStates.TB_Stage1:
                    attackTimer = 6;
                    runSpeed = speed;
                    break;

                case EnemyStates.TB_Stage2:
                    attackTimer = 5;
                    runSpeed = speed;
                    break;

                // case EnemyStates.TB_Stage3:
                //     break;
                // case EnemyStates.TB_Stage4:
                //     break;
            }
        }
    }
    protected override void OnCollisionStay2D(Collision2D _other)
    {

    }
    #region attacking
    #region variables
    [HideInInspector] public bool attacking;
    [HideInInspector] public float attackCountdown;
    [HideInInspector] public bool damagedPlayer = false;

    [HideInInspector] public Vector2 moveToPosition;
    [HideInInspector] public bool diveAttack;
    public GameObject divingCollider;
    public GameObject pillar;
    public GameObject barrageFireball;
    [HideInInspector] public bool outbreakAttack;


    #endregion

    #region Control

    public void AttackHandler()
    {
        if(GetCurrentEnemyState == EnemyStates.TB_Stage1)
        {
            if(Vector2.Distance(PlayerController.Instance.transform.position, rb.position) < attackRange)
            {
                StartCoroutine(TripleSlash());
            }
            else    
            {
                int _attackChosen = Random.Range(1, 3);
                    if(_attackChosen == 1)
                    {
                        DiveAttackJump();
                    }
                    else if(_attackChosen == 2)
                    {
                        StartCoroutine(Lunge());
                    }
            }
        }
        // if(currentEnemyState == EnemyStates.TB_Stage2)
        // {

        // }
    }
    public void ResetAllAttacks()
    {
        attacking = false;

        StopCoroutine(TripleSlash());
        StopCoroutine(Lunge());

        diveAttack = false;
        outbreakAttack = false;
    }
    
    #endregion

    #region Stage 3

    void OutbreakBendDown()
    {
        attacking = true;
        rb.linearVelocity = Vector2.zero;
        moveToPosition = new Vector2(transform.position.x, rb.position.y + 5);
        outbreakAttack = true;
        anim.SetTrigger("BendDown");
    }

    public IEnumerator Outbreak()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Cast", true);

        rb.linearVelocity = Vector2.zero;
        for(int i = 0; i < 30; i++)
        {
            Instantiate(barrageFireball, transform.position, Quaternion.Euler(0, 0, Random.Range(110, 130)));
            Instantiate(barrageFireball, transform.position, Quaternion.Euler(0, 0, Random.Range(50, 70)));
            Instantiate(barrageFireball, transform.position, Quaternion.Euler(0, 0, Random.Range(260, 280)));

            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.1f);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -10);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Cast", false);
        ResetAllAttacks();
    }

    #endregion

    #region Stage 2

    void DiveAttackJump()
    {
        attacking = true;
        moveToPosition = new Vector2(PlayerController.Instance.transform.position.x, rb.position.y + 10);
        diveAttack = true;
        anim.SetBool("Jump", true);
    }

    public void Dive()
    {
        anim.SetBool("Dive", true);
        anim.SetBool("Jump", false);
    }

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.GetComponent<PlayerController>() != null & diveAttack)
        {
            _other.GetComponent<PlayerController>().TakeDamage(damage * 2);
            PlayerController.Instance.pState.recoilingX = true;
        }
    }

    public void DivingPillars()
    {
        Vector2 _impactPoint = groundCheckPoint.position;
        float _spawnDistance = 5;

        for(int i = 0; i < 10; i++)
        {
            Vector2 _pillarSpawnPointRight = _impactPoint + new Vector2(_spawnDistance, 0);
            Vector2 _pillarSpawnPointLeft = _impactPoint - new Vector2(_spawnDistance, 0);

            Instantiate(pillar, _pillarSpawnPointRight, Quaternion.Euler(0, 0, -90));
            Instantiate(pillar, _pillarSpawnPointLeft, Quaternion.Euler(0, 0, -90));

            _spawnDistance += 5;
        }

        DestroyAllPillars();
        ResetAllAttacks();
    }

    public void DestroyAllPillars()
    {
        DivingPillar[] allPillars = FindObjectsByType<DivingPillar>(FindObjectsSortMode.None);
        foreach (DivingPillar pillar in allPillars)
        {
            Destroy(pillar.gameObject, 3f);
        }
    }

    
    #endregion

    #region Stage 1

    IEnumerator TripleSlash()
    {
        attacking = true;
        rb.linearVelocity = Vector2.zero;

        anim.SetTrigger("Slash");
        yield return new WaitForSeconds(0.3f);
        anim.ResetTrigger("Slash");

        anim.SetTrigger("Slash");
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("Slash");

        anim.SetTrigger("Slash");
        yield return new WaitForSeconds(0.2f);
        anim.ResetTrigger("Slash");

        ResetAllAttacks();
        
    }

    IEnumerator Lunge()
    {
        Flip();
        attacking = true;

        anim.SetTrigger("Lunge");
        yield return new WaitForSeconds(1.5f);
        anim.ResetTrigger("Lunge");
        damagedPlayer = false;
        ResetAllAttacks();
    }

    #endregion
    
    #endregion
}
