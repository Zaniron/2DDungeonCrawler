using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float airMoveSpeed;
    private float xDirInput;
    private bool facingRight = true;
    private bool isMoving;

    [Header("Jumping")]
    [SerializeField]float jumpForce;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckDist;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    private RaycastHit2D grounded;
    private bool canJump;

    [Header("Wall Sliding")]
    [SerializeField] float wallSlideSpeed = 0;
    [SerializeField] float wallCheckDist;
    //[SerializeField] float maxWallJumpCount;
    //[SerializeField] float wallFallTime;
    //private float currentWallJumpCount;
    private bool isTouchingWall;
    private bool isWallSliding;
    //private float resetWallCheck;

    [Header("Wall Jumping")]
    [SerializeField] float wallJumpForce = 28f;
    float wallJumpDirection = -1f;
    [SerializeField] Vector2 wallJumpAngle;

    [Header("Other")]
    [SerializeField]Animator anim;
    private Rigidbody2D rb2d;
    private int xWallDir = 1;

    AudioManager playerSounds;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        wallJumpAngle.Normalize();
        //playerSounds = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        Inputs();
        CheckWorld();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        AnimationControl();
        WallSlide();
        WallJump();
    }

    void Inputs()
    {
        xDirInput = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
        {
            canJump = true;
        }
    }

    void CheckWorld()
    {
        grounded = Physics2D.Raycast(transform.position, -Vector2.up, groundCheckDist, groundLayer);
        isTouchingWall = Physics2D.Raycast(transform.position, new Vector2(xWallDir, 0), wallCheckDist, groundLayer);
    }

    void Movement()
    {
        //forAnimation
        if(xDirInput != 0)
        {
            isMoving = true;
            
        }else
        {
            isMoving = false;
            FindObjectOfType<AudioManager>().Play("Running");
            //playerSounds.Play("Running");
        }

        if(grounded)
        {
            rb2d.velocity = new Vector2(xDirInput * moveSpeed, rb2d.velocity.y);
        }
        else if(!grounded && !isWallSliding && xDirInput != 0)
        {
            rb2d.AddForce(new Vector2(airMoveSpeed * xDirInput, 0));
            if(Mathf.Abs(rb2d.velocity.x) > moveSpeed)
            {
                rb2d.velocity = new Vector2(xDirInput * moveSpeed, rb2d.velocity.y);
            }
        }
        

        if(xDirInput < 0 && facingRight)
        {
            Flip();
        }else if(xDirInput > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        if(!isWallSliding)
        {
            wallJumpDirection *= -1;
            facingRight = !facingRight;
            xWallDir *= -1;
            transform.Rotate(0,180,0);
        }
    }

    void Jump()
    {
        if(canJump && grounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);

            canJump = false;
        }
        if(rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
            //animator.SetBool("jump", true);
        }else if(rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            //animator.SetBool("jump", true);
        }
    }

    void WallSlide()
    {
        if(isTouchingWall && !grounded && rb2d.velocity.y < 0)
        {
            isWallSliding = true;
        }else
        {
            isWallSliding = false;
        }

        //wall sliding
        if(isWallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, wallSlideSpeed);
        }
    }

    void WallJump()
    {
        if((isWallSliding || isTouchingWall) && canJump)
        {
            rb2d.AddForce(new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse); 
            canJump = false;               
        }
    }

    

    void AnimationControl()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetFloat("jump", rb2d.velocity.y);
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -Vector2.up * groundCheckDist);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, new Vector2(xWallDir, 0) * wallCheckDist);
    }
}
