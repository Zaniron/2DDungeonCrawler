using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour
{
    public int gemValue;
    public LayerMask groundLayer;

    private RaycastHit2D grounded;
    private Rigidbody2D gemrb;

    void Awake()
    {
        gemrb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        grounded = Physics2D.Raycast(transform.position, -Vector2.up, 0.35f, groundLayer);

        if(grounded)
        {
            Invoke("StopVelocity", 0.3f);
        }
    }

    void StopVelocity()
    {
        gemrb.velocity = Vector2.zero;
        Debug.Log("Gem grounded");
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerController player = hitInfo.GetComponent<PlayerController>();
        //GemCounter gemCount = GetComponent<GemCounter>();
        if(hitInfo.CompareTag("Player"))
        {
            //gemCount.AddGem(gemValue);
            player.AddGem(gemValue);
            FindObjectOfType<AudioManager>().Play("GemPickup");
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -Vector2.up * 0.35f);
    }
}
