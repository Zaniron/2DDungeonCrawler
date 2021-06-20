using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Patrol();
    }
}
