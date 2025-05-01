using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    //---------------------------------------------------------

    [Header("Horizontal Movement Settings")]
    [SerializeField] private float walkSpeed = 1;

    //---------------------------------------------------------

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 45;
    private int JumpBufferCounter = 0;
    [SerializeField] private int jumpBufferFrames;
    private int airjumpCounter = 0;
    [SerializeField] private int maxAirJumps;

    //---------------------------------------------------------

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    //---------------------------------------------------------

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    //---------------------------------------------------------

    PlayerStateList pState;
    private Rigidbody2D rb;
    public static float xAxis;
    private float gravity;
    Animator anim;
    private bool canDash;
    private bool Dashed;

    //---------------------------------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        gravity = rb.gravityScale;

        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); 
        UpdateJumpVariables();
        if (pState.dashing) return;
        Move();
        Jump();
        StartDash();
    }

    void GetInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(xAxis * walkSpeed, rb.linearVelocity.y);
        anim.SetBool("Run", rb.linearVelocity.x != 0 && Grounded());
    }

    void StartDash()
    {
        if(Input.GetButtonDown("Dash") && canDash && !Dashed)
        {
            StartCoroutine(Dash());
            Dashed = true;
        }

        if(Grounded())
        {
            Dashed = false;
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        pState.dashing = true;
        anim.SetTrigger("Dashing");
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashSpeed, 0);
        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = gravity;
        pState.dashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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

    void Jump()
    {
        if(Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0);

            pState.jumping = false;
        }

        if (!pState.jumping)
        {
            if (JumpBufferCounter > 0 && Grounded())
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);

                pState.jumping = true;
            }
            else if(!Grounded() && airjumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
            {
                pState.jumping = true;

                airjumpCounter++;

                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce);
            }
        }

        anim.SetBool("Jump", !Grounded());
    }

    void UpdateJumpVariables()
    {
        if (Grounded())
        {
            pState.jumping = false;
            airjumpCounter = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            JumpBufferCounter = jumpBufferFrames;
        }
        else
        {
            JumpBufferCounter--;
        }
    }

}
