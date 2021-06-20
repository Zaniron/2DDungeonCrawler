using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float groundCheckDist;
    [SerializeField] protected float wallCheckDist;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float aggroRadius;
    [SerializeField] protected float edgeCheckStart;
    [SerializeField] protected int gemDrop;
    [SerializeField] protected GameObject gemPrefab;
    [SerializeField] protected Animator enemyAnim;

    protected GameObject player;
    protected PlayerController playerController;
    protected Vector2 playerPos;
    protected Vector2 enemyPos;
    protected RaycastHit2D grounded;
    protected RaycastHit2D isTouchingWall;
    protected RaycastHit2D edgeCheck;
    protected float xWallDir = 1;
    protected bool facingRight = true;
    public float currentHealth { get; private set;}
    protected Rigidbody2D rb2d;


    void Awake()
    {
        currentHealth = maxHealth;
        player = FindObjectOfType<PlayerController>().gameObject; 
        rb2d = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();       
    }

    protected virtual void Update()
    {
        //FindPlayer();
        CheckWorld();
        //Patrol();
        
    }

    public void CheckWorld()
    {
        playerPos = player.transform.position;
        enemyPos = transform.position;

        grounded = Physics2D.Raycast(enemyPos, -Vector2.up, groundCheckDist, groundLayer);
        isTouchingWall = Physics2D.Raycast(enemyPos, new Vector2(xWallDir, 0), wallCheckDist, groundLayer);
        edgeCheck = Physics2D.Raycast(new Vector2(enemyPos.x + (edgeCheckStart * -xWallDir), enemyPos.y), -Vector2.up, groundCheckDist, groundLayer);
    }
    
    public void Flip()
    {
        transform.Rotate(0,180,0);
        xWallDir *= -1;
        facingRight = !facingRight;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //rb2d.velocity = Vector2.zero;
        enemyAnim.SetTrigger("hurt");
        Debug.Log("Took " + damage + " damage");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag== "Player")
        {
            col.gameObject.SendMessage("TakeDamage", damage);
        }
    }

    public virtual void FindPlayer()
    {
        if(player != null)
        {
            if(enemyPos.x > playerPos.x && facingRight)
            {
                Flip();
            }else if(enemyPos.x < playerPos.x && !facingRight)
            {
                Flip();
            }
            if(Vector2.Distance(transform.position, playerPos) <= aggroRadius)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
            }
            

            /*
            if player in range
                AgroPlayer();
            else if player not in range
                Patrol();
            
            (this is for flying enemies)
            if player not in range and enemy is not at starting point
                return to starting point, and then Patrol();

            if enemy hits wall, check if it is only one tile high
            if only one tile high, it is jumpable. jump up onto platform.

            on patrol, check so enemy does not walk off edges and patrol back and forth.
            
            */
        }
    }

    protected virtual void Patrol()
    {
        enemyAnim.SetBool("running", true);
        if(xWallDir == 1)
        {
            rb2d.velocity = new Vector2(xWallDir * speed, rb2d.velocity.y);
                
        }else if (xWallDir == -1)
        {
            rb2d.velocity = new Vector2(xWallDir * speed, rb2d.velocity.y);
        }
        if(isTouchingWall || !edgeCheck)
        {
            Flip();
        }
    }

    public virtual void Die()
    {
        for (int i = 0; i < gemDrop; i++)
        {
            float pushx = Random.Range(-3, 3);
            float pushy = Random.Range(0, 6);

            GameObject gems = Instantiate(gemPrefab, transform.position, Quaternion.identity);
            Rigidbody2D gemrb = gems.GetComponent<Rigidbody2D>();
            //gemrb.MovePosition(gemrb.position + new Vector2(pushx, pushy) * Time.deltaTime);
            gemrb.AddForce(new Vector2(pushx, pushy), ForceMode2D.Impulse);
            Debug.Log(pushx + " " + pushy);
            
        }
        enemyAnim.SetTrigger("hurt");
        rb2d.velocity = Vector2.zero;
        Invoke("Destroy", 0.1f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -Vector2.up * groundCheckDist);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, new Vector2(xWallDir, 0) * wallCheckDist);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.DrawRay(new Vector2(transform.position.x + (edgeCheckStart * -xWallDir), transform.position.y), -Vector2.up * groundCheckDist);
    }
}
