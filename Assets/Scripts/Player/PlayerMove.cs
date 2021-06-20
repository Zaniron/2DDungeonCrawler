using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float rememberGrounded;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundCheckDistance = 3f;

    bool isTouchingFront;
    bool wallSliding;
    public float wallSlidingSpeed;
    public float wallCheckDistance;
    public float wallStickTime;
    float timeToWallUnstick;

    bool wallJumping;
    public float wallLeap;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    public float maxWallJump;
    float currentWallJumpCount;    
    
    public LayerMask groundLayer;
    public Animator animator;
    
    private Rigidbody2D rb2d;

    private bool isGrounded = false;
    private float lastTimeGrounded;
    private bool facingRight = true;
    private Vector2 direction;
    private int wallDirX;
    private bool canMove = true;

    float input;
    float moveBy;
    Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(canMove);
        //Movement -----------------------------------------------------
        input = Input.GetAxisRaw("Horizontal");
        if(canMove)
        {
            
            moveBy = input * speed;
            velocity = new Vector2(moveBy, rb2d.velocity.y);
            rb2d.velocity = velocity;
            animator.SetFloat("speed", Mathf.Abs(moveBy));
        }else{
            canMove = false;
        }

        if(!facingRight && moveBy > 0)
        {
            Flip();
        }else if(facingRight && moveBy < 0)
        {
            Flip();
        }

        //Jumping ------------------------------------------------------
        if(Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastTimeGrounded <= rememberGrounded))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            //animator.SetBool("jump", true);
        }

        //Jump modifier -------------------------------------------------
        if(rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
            //animator.SetBool("jump", true);
        }else if(rb2d.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            //animator.SetBool("jump", true);
        }

        //Check if grounded ----------------------------------------------------------------------------
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, -Vector2.up, groundCheckDistance, groundLayer);

        if(groundCheck != false)
        {
            isGrounded = true;
            currentWallJumpCount = 0;
            //animator.SetBool("jump", false);
        }else{
            if(isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
            //animator.SetBool("jump", true);
        }

        //Wall Slide -------------------------------------------------------
        if(rb2d.velocity.x > 0)
        {
            direction = Vector2.right;
            wallDirX = 1;
        }else if (rb2d.velocity.x < 0)
        {
            direction = -Vector2.right;
            wallDirX = -1;
        }

        RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, direction, wallCheckDistance, groundLayer);

        if(wallCheck && !isGrounded)
        {
            wallSliding = true;
        }else{
            wallSliding = false;
        }

        if(wallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        //Wall Jump ------------------------------------------------------
        if(Input.GetButtonDown("Jump") && wallSliding && (currentWallJumpCount < maxWallJump))
        {
            //Debug.Log("That Jump");
            wallJumping = true;
            Invoke("SetWallJumpToFalse", wallJumpTime);
        }

        if(wallJumping)
        {
            rb2d.velocity = new Vector2(xWallForce * -rb2d.velocity.x, yWallForce);
        }

        if(direction != null && !isGrounded && velocity.y < 0)
        {
            //Debug.Log(canMove);
            canMove = false;
            if(timeToWallUnstick > 0)
            {   
                canMove = false;

                if(input != wallDirX && input != 0)
                {
                    
                    timeToWallUnstick -= Time.deltaTime;
                    if(Input.GetButtonDown("Jump"))
                    {
                        //Debug.Log("This Jump");
                        rb2d.velocity = new Vector2(wallLeap * -rb2d.velocity.x, yWallForce);
                    }
                    canMove = false;
                }else{
                    timeToWallUnstick = wallStickTime;
                    if(!wallSliding)
                        canMove = true;
                }    
  
            }else{
                timeToWallUnstick = wallStickTime;
                canMove = true;
            }
        }
        Debug.Log(rb2d.velocity.x);
        //Debug.Log(timeToWallUnstick);
    }

    void SetWallJumpToFalse()
    {
        wallJumping = false;
        currentWallJumpCount++;
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -Vector2.up * groundCheckDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector2.right * wallCheckDistance);
    }
}
