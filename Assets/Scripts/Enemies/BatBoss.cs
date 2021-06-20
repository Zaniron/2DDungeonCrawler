using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BatBoss : Enemy
{
    public enum BossState{WAIT, CHASE, ENRAGED, DEATH};
    public BossState ActiveState = BossState.WAIT;
    public MakeDark makeDark;
    public Light2D globalLight;

    void Start()
    {
        makeDark = FindObjectOfType<MakeDark>();
    }

    protected override void Update()
    {
        base.Update();
        //FindPlayer();
        //Patrol();

        switch(ActiveState)
        {
            case BossState.WAIT:
            {
                Debug.Log("WAIT");
            }
                break;
            
            case BossState.CHASE:
            {
                Debug.Log("CHASE");
                FindPlayer();
            }
                break;
            
            case BossState.ENRAGED:
            {
                Debug.Log("ENRAGED");
                FindPlayer();
                damage = 4;
                speed = 6;
                aggroRadius = 12;
            }
                break;
        }

        if(makeDark.lightOff && currentHealth >= 25)
        {
            ActiveState = BossState.CHASE;
        }else if(currentHealth < 25)
        {
            ActiveState = BossState.ENRAGED;
        }
    }

    public override void Die()
    {
        base.Die();
        globalLight.enabled = true;
    }
}
