using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Patrol();
        FindObjectOfType<AudioManager>().Play("RatRun");
        //FindPlayer();
    }

    public override void Die()
    {
        base.Die();
        FindObjectOfType<AudioManager>().Play("RatDeath");
    }
}
